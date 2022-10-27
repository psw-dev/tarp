using PSW.ITMS.Data;
using PSW.ITMS.Service.Command;

namespace PSW.ITMS.Service.Strategies
{
    public class SecureStrategyFactory : IStrategyFactory
    {
        #region Private Variables

        #endregion

        #region Properties
        public IUnitOfWork UnitOfWork { get; protected set; }
        #endregion

        #region Constructor & Destructor
        public SecureStrategyFactory(IUnitOfWork uow)
        {
            UnitOfWork = uow;
        }
        #endregion

        #region Private Method

        #endregion

        #region Public Methods

        public Strategy CreateStrategy(CommandRequest request)
        {
            switch (request.methodId)
            {
                case "1732": return new GetRegulatedHSCodeExtListStrategy(request);                
                default: break;
            }

            return new InvalidStrategy(request);
        }

        #endregion

    }

}
