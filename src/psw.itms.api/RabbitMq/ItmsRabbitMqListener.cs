using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PSW.ITMS.Data;
using PSW.ITMS.Data.Sql;
using PSW.ITMS.Service;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.Strategies;
using PSW.RabbitMq;
using PSW.RabbitMq.ServiceCommand;



namespace PSW.ITMS.RabbitMq
{
    public class ITMSRabbitMqListener : RabbitMqListener
    {
        //TODO: Inject
        private IItmsService _service  { get; set; }

 

        public ITMSRabbitMqListener(IItmsService service, IUnitOfWork uow, IConfiguration configuration)
        : base(configuration)
        {
            this._service = service;
            this._service.UnitOfWork=uow;
            this._service.StrategyFactory=new StrategyFactory(uow);
        }
                
        public override void ProcessMessage(ServiceRequest request, IConfiguration configuration, IEventBus eventBus, string correlationId, string replyTo)
        {
            var cmdRequest = new CommandRequest(){
                methodId = request.methodId,
                data = JsonDocument.Parse(request.data).RootElement
            }; 

            _service.UnitOfWork = new UnitOfWork(configuration, eventBus);
            var commandReply = _service.invokeMethod(cmdRequest); 

            if(string.IsNullOrEmpty(correlationId))
            {
                return;
            }
            
            var svcReply = new ServiceReply(){
                data = commandReply.data.GetRawText(),
                code = commandReply.code,
                exception = commandReply.exception,
                shortDescription = commandReply.shortDescription,
                fullDescription = commandReply.fullDescription,
                message = commandReply.message
                
            }; 

            eventBus.PublishReply(svcReply, replyTo, correlationId);
        }
    }
}