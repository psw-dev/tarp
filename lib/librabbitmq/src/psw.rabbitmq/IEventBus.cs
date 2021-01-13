using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PSW.RabbitMq.ServiceCommand;

namespace PSW.RabbitMq
{
    public interface IEventBus
    {
        void PublishRequest(ServiceRequest request, string queueName);
        void PublishReply(ServiceReply reply, string replyQueueName, string correlationId);
        void Subscribe(string queueName, RabbitMqListener listener, IConfiguration configuration, IEventBus eventBus);
        ServiceReply Call(ServiceRequest request, string queueName, string replyQueueName, string correlationId);
    }
}