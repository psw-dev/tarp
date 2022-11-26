using PSW.ITMS.Data;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.Strategies;
using PSW.Common.Crypto;
using StackExchange.Redis;

namespace PSW.ITMS.Service
{
    public interface IService
    {
        IUnitOfWork UnitOfWork { get; set; }
        IStrategyFactory StrategyFactory { get; set; }
        IConnectionMultiplexer RedisConnection { get; set; }
        CommandReply invokeMethod(CommandRequest request);
        ICryptoAlgorithm CryptoAlgorithm { get; set; }
    }

}