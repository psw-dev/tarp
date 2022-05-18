using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using PSW.RabbitMq;
using PSW.RabbitMq.ServiceCommand;
using PSW.RabbitMq.Task;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System;
using PSW.RabbitMq.RPC;
using PSW.Common.Crypto;
using System.Security.Cryptography;
using AutoMapper;
using PSW.ITMS.Service;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Data.Sql;
using PSW.ITMS.Service.Strategies;

public class RPCQueueService : RPCServer, IHostedService, IDisposable
{

    #region Hosted Service Implementation
    private Task _executingTask;
    private CancellationTokenSource _cts;
    private IItmsService _service { get; set; }
    private IConfiguration _configuration;
    private ICryptoAlgorithm _cryptoAlgorithm { get; set; }    
    private IMapper _mapper {get;set;}

    public RPCQueueService(IConfiguration configuration, IMapper mapper)
    {
        this._configuration = configuration;       
        this._mapper = mapper;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _executingTask = ExecuteAsync(cancellationToken);
        if (_executingTask.IsCompleted)
        {
            return _executingTask;
        }
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {

        if (_executingTask == null)
        {
            return;
        }
        try
        {
            _cts.Cancel();
            this.Stop();
        }
        finally
        {
            await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }
    }
    protected Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() => { this.Start(MessageQueues.TARPRCQueue); });
    }

    #endregion

    //protected override ServiceReply ProcessMessage(ServiceRequest svcRequest)
    public override ServiceReply ProcessMessage(ServiceRequest svcRequest)
    {
         System.Console.WriteLine("SERVICE RECIEVED");
        string salt = Environment.GetEnvironmentVariable("ENCRYPTION_SALT");
        string password = Environment.GetEnvironmentVariable("ENCRYPTION_PASSWORD");
        _cryptoAlgorithm = new CryptoFactory().Create<AesManaged>(password, salt);
        System.Console.WriteLine("SERVICE RECIEVED");
        var cmdRequest = new CommandRequest()
        {
            methodId = svcRequest.methodId,
            data = JsonDocument.Parse(svcRequest.data).RootElement,
            CryptoAlgorithm = _cryptoAlgorithm
        };

        _service = new ItmsService(_mapper, _cryptoAlgorithm);
        _service.UnitOfWork = new UnitOfWork(_configuration);
        _service.StrategyFactory = new StrategyFactory(_service.UnitOfWork);

        var commandReply = _service.invokeMethod(cmdRequest);

        var svcReply = new ServiceReply()
        {
            data = commandReply.data.GetRawText(),
            code = commandReply.code,
            exception = commandReply.exception,
            shortDescription = commandReply.shortDescription,
            fullDescription = commandReply.fullDescription,
            message = commandReply.message
        };

        return svcReply;
    }

    public virtual void Dispose()
    {
        this.Stop();
    }
}