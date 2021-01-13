namespace PSW.Lib.Consul
{
    public class ConsulOptions
    {
        public string Id { get; set; }
       // TODO Remove Name Property later
        public string Name { get; set; }
        public string ServiceName { get; set; }
        public string[] Tags { get; set; }
        public string ConsulAddress { get; set; }
        public string ServiceAddress { get; set; }
        public bool DisableAgentCheck { get; set; }
    }
}
