// using Microsoft.Extensions.Configuration;
// using PSW.RabbitMq;
// using PSW.RabbitMq.ServiceCommand;
// using PSW.ITMS.Common.Command;
// using PSW.ITMS.Data.Sql.UnitOfWork;
// using PSW.ITMS.Data.UnitOfWork;
// using PSW.ITMS.Service.Factories;
// using PSW.ITMS.Service.Services.UPS;

// namespace PSW.OGARabbitMq
// {
//     public class UPSRabbitMqListener : RabbitMqListener
//     {
//         private IOgaService _service  { get; set; }

//         public UPSRabbitMqListener(IOgaService service, IUnitOfWork uow, IConfiguration configuration)
//         : base(configuration)
//         {
//             uow.BeginTransaction();
//             this._service = service;
//             this._service.UnitOfWork=uow;
//             this._service.StrategyFactory= new StrategyFactory(uow);
//         }

//         public override void ProcessMessage(ServiceRequest request, IConfiguration configuration, IEventBus eventBus, string correlationId, string replyTo)
//         {
//             _service.UnitOfWork = new UnitOfWork(configuration, eventBus);

//             //Adding ServiceRequest data to CommandRequest and
//             // invoking service method
//             var commandReply = _service.invokeMethod(new CommandRequest(){
//                 methodId = request.methodId,
//                 data = request.data
//             });

//             //Checking if there is a need to reply back after processing request
//             if(string.IsNullOrEmpty(correlationId))
//             {
//                 return;
//             }

//             //Adding CommandReply data to ServiceReply and
//             //publishing reply to provided queue name in replyTo
//             eventBus.PublishReply(new ServiceReply(){
//                 code = commandReply.code,
//                 data = commandReply.data,
//                 shortDescription = commandReply.shortDescription,
//                 fullDescription = commandReply.fullDescription,
//                 message = commandReply.message,
//                 exception = commandReply.exception
//             }, replyTo, correlationId);
//         }
//     }
// }
