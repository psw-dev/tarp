using System.Net;
using System.Text.Json;

namespace PSW.ITMS.Service.Command
{
    public class CommandReply
    {
        public JsonElement data { get; set; }
        public string exception { set; get; }
        public string shortDescription { get; set; }
        public string fullDescription { get; set; }
        public string code { get; set; }
        public string message { set; get; }
         public CommandReply()
        {
            this.data = JsonDocument.Parse(JsonSerializer.SerializeToUtf8Bytes(new {})).RootElement;
        }
    }


}