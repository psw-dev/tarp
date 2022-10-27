using AutoMapper;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.Mapper;
using System;
using System.IO;

namespace PSW.ITMS.Service.Strategies
{
    public class Strategy
    {
        public IMapper Mapper { get; protected set; }
        public CommandReply Reply { get; protected set; }
        public bool IsValidated { get; protected set; }



        public CommandRequest Command { get; set; }
        public Strategy(CommandRequest request)
        {
            Command = request;
            this.IsValidated = true;
            Mapper = new ObjectMapper().GetMapper();
        }
        public virtual CommandReply Execute()
        {
            return null;
        }
        
        public string ValidationMessage { get; set; }
        public virtual bool Validate()
        {
            //return IsValidated;
            return this.IsValidated;
        }

        public virtual void log(string message)
        {
            var logPath = "/tmp/itms.log";
            File.AppendAllText(logPath, DateTime.Now.ToString("hhmmss") + " " + message + Environment.NewLine);
        }
        public virtual CommandReply BadRequestReply(string message)
        {
            Reply.code = "400"; // Bad Request | Error
            Reply.message = message;
            Reply.exception = null;
            return Reply;
        }
    }
}
