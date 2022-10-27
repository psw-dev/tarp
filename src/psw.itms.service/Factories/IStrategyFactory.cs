
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.Strategies;

namespace PSW.ITMS.Service
{
    public interface IStrategyFactory
    {
        Strategy CreateStrategy(CommandRequest request);
    }
}