using PSW.ITMS.Data;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.Strategies;
using PSW.Common.Crypto;

namespace PSW.ITMS.Service
{
    public interface IService
    {
        IUnitOfWork UnitOfWork { get; set; }
        StrategyFactory StrategyFactory { get; set; }
        CommandReply invokeMethod(CommandRequest request);
        ICryptoAlgorithm CryptoAlgorithm { get; set; }
    }

}