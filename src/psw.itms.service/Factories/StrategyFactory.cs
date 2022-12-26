
using PSW.ITMS.Data;
using PSW.ITMS.Service.Command;
//using PSW.ITMS.Service.Strategies;

namespace PSW.ITMS.Service.Strategies
{
    public class StrategyFactory : IStrategyFactory
    {
        #region Private Variables

        #endregion

        #region Properties
        public IUnitOfWork UnitOfWork { get; protected set; }
        #endregion

        #region Constructor & Destructor
        public StrategyFactory(IUnitOfWork uow)
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
                // case "1710": return new MockDataStrategy(request);
                // case "1711": return new GetHSCodeListStrategy(request);
                // case "1712": return new GetHsCodeRequirementStrategy(request);
                // case "1713": return new GetRequiredDocumentTypeStrategy(request);
                // case "1714": return new SearchHSCodesStrategy(request);
                case "1716": return new GetAgencyListStrategy(request);
                case "1718": return new GetFactorListStrategy(request);
                case "1720": return new GetFactorLOVItemsStrategy(request);
                case "1722": return new GetRequirementMongoStrategy(request);
                case "1724": return new GetRegulatedHSCodeListStrategy(request);
                case "1726": return new GetPCTCodeListStrategy(request);
                case "1728": return new UpdateMongoRecordStrategy(request);
                case "1729": return new GetRegulatedHsCodePurposeStrategy(request);
                case "1730": return new GetFormNumberStrategy(request);
                case "1731": return new TestStrategy(request);
                case "1733": return new GetFinancialRequirementStrategy(request);
                case "1737": return new ValidateRegulatedHSCodes(request);
                case "1738": return new GetCountryListStrategy(request);
               
                

                case "removelater": return null;

                default: break;
            }

            return new InvalidStrategy(request);
        }

        #endregion

    }

}
