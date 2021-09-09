using PSW.ITMS.Data;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.Strategies;

namespace PSW.ITMS.Service
{
    public interface IService
    {
        IUnitOfWork UnitOfWork { get; set; }
        StrategyFactory StrategyFactory { get; set; }
        CommandReply invokeMethod(CommandRequest request);
    }

}