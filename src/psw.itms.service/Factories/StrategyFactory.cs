
using PSW.ITMS.Service.Command;
using PSW.ITMS.Data;
//using PSW.ITMS.Service.Strategies;

namespace PSW.ITMS.Service.Strategies
{
    public class StrategyFactory
    {
        #region Private Variables

        #endregion

        #region Properties
        public IUnitOfWork UnitOfWork { get; protected set; }
        #endregion

        #region Constructor & Destructor
        public StrategyFactory(IUnitOfWork uow)
        {
            this.UnitOfWork = uow;
        }
        #endregion

        #region Private Method

        #endregion
        #region Public Methods
        public Strategy CreateStrategy(CommandRequest request)
        {

            switch (request.methodId)
            {
                case "1710": return new MockDataStrategy(request);
                case "removelater": return null;
                
                default: break;
            }


            return new InvalidStrategy(request);

        }
        #endregion

    }

}
