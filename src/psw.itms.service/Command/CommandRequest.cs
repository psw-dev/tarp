using AutoMapper;
using PSW.ITMS.Data;
using System.Text.Json;
using PSW.Common.Crypto;

namespace PSW.ITMS.Service.Command
{
    public class CommandRequest
    {
        public JsonElement data { get; set; }
        public string methodId { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }
        public IMapper _mapper { get; set; }
        public ICryptoAlgorithm CryptoAlgorithm { get; set; }
        
    }
}