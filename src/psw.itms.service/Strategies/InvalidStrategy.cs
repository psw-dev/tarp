using System.Text.Json;
using System.Text.Json.Serialization;
using PSW.ITMS.Service.Command;

namespace PSW.ITMS.Service.Strategies
{
    public class InvalidStrategy:Strategy
    {
        public InvalidStrategy(CommandRequest request):base(request)
        {
            
        }

        public override CommandReply Execute(){
            //this.Command
            string json = "{}";
                    
            return new CommandReply(){
                message = "Invalid Method",
                data = JsonDocument.Parse(json).RootElement, 
                code = "404"
            };
        }
    }
}
