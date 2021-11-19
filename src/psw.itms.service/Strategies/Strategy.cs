using AutoMapper;
using PSW.ITMS.Service.Command;
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
            IsValidated = false;
        }
        public virtual CommandReply Execute()
        {
            return null;
        }

        public virtual bool Validate()
        {
            return IsValidated;
        }

        public virtual void log(string message)
        {
            var logPath = "/tmp/itms.log";
            File.AppendAllText(logPath, DateTime.Now.ToString("hhmmss") + " " + message + Environment.NewLine);
        }
    }
}
