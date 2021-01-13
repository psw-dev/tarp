using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using PSW.RabbitMq.ServiceCommand;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PSW.RabbitMq
{
    public class RabbitMqListener : IListener
    {
        private IConnectionFactory _factory;
        private EventingBasicConsumer _consumer;
        private IModel _channel;
        private string _queueName;
        private IConfiguration _configuration;

        public RabbitMqListener(IConfiguration configuration)
        {
            _configuration = configuration;
            var rabbitMqConfig = _configuration.GetSection("RabbitMqConfig");

            _factory = new ConnectionFactory(){
                HostName = rabbitMqConfig["HostName"]?.ToString(),
                UserName = rabbitMqConfig["UserName"]?.ToString(),
                Password = rabbitMqConfig["Password"]?.ToString(),
                Port = Int32.Parse(rabbitMqConfig["Port"]),
                VirtualHost = rabbitMqConfig["VirtualHost"]?.ToString()
            };
        }

        public void CreateConsumer()
        {
            try
            {
                _channel = _factory.CreateConnection().CreateModel();
                _channel.QueueDeclare(_queueName, false, false, false, null);
                _consumer = new EventingBasicConsumer(_channel);
            }

            //TODO : handle connection exception.
            catch(Exception ex)
            {
                return;
            }
        }

        public void Consume()
        {
            _channel.BasicConsume(_queueName, true, _consumer);
        }

        public void SubscribeProcess(string queueName, IConfiguration configuration, IEventBus eventBus)
        {
            try
            {
                _queueName = queueName;
                CreateConsumer();

                _consumer.Received += ((s,e) => {
                    var body = e.Body.ToArray();
                    var correlationId = e.BasicProperties.CorrelationId;
                    var replyTo = e.BasicProperties.ReplyTo;
                    var svcRequest = (ServiceRequest)RabbitMqHelper.ByteArrayToObject(body);
                    ProcessMessage(svcRequest, configuration, eventBus, correlationId, replyTo);
                    var mess = Encoding.UTF8.GetString(body);
                    Console.WriteLine("Message subscribed on queue: {0}", _queueName);
                });

                Consume();
            }

            //TODO : handle connection exception.
            catch(Exception ex)
            {
                return;
            }            
        }

        public virtual void ProcessMessage(ServiceRequest request, IConfiguration configuration, IEventBus eventBus, string correlationId, string replyTo)
        {
            //Work to do on message
        }
    }
}
