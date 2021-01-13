using System;

namespace PSW.RabbitMq.ServiceCommand
{
    [Serializable]
    public class ServiceRequest
    {
        public string data { get; set; }
        public string methodId { get; set; }
    }
}