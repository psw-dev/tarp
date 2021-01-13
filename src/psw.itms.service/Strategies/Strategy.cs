using AutoMapper;
using PSW.ITMS.Service.Command;
using System.IO;
using System;

namespace PSW.ITMS.Service.Strategies
{
    public class Strategy
    {
        public IMapper Mapper{get; protected set;}
        public CommandReply Reply{get; protected set;}
        public bool IsValidated {get; protected set;}

        

        public CommandRequest Command { get; set; }
        public Strategy(CommandRequest request)
        {
            this.Command = request;
            this.IsValidated=false;
        }
        public virtual CommandReply Execute()
        {
            return null;
        }

        public virtual bool Validate(){            
            return this.IsValidated;
        }

        public virtual void log(string message)
        {
            string logPath = "/tmp/itms.log";
            File.AppendAllText(logPath, DateTime.Now.ToString("hhmmss") + " " + message + Environment.NewLine);
        }
    }
}
