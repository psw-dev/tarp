using PSW.Lib.Logs;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.DTO;
 

namespace PSW.ITMS.Service.OGAItemCategory
{
    public class ECFeeCalculation
    {
        CommandRequest Command;
        public ECFeeCalculation(CommandRequest commandRequest)
        {
            this.Command = commandRequest;
        }

        #region Methods 
        public CalculateECFeeResponse GetECFee(CalculateECFeeRequest RequestDTO)
        {
            Log.Information("|ECFeeCalculation|  request: {@RequestDTO}", RequestDTO);
            var ogaItemCategoryFactory = new OGAItemCategoryFactory(Command.UnitOfWork, RequestDTO);
            var response = ogaItemCategoryFactory.CalculateECFee();
            var FeeCalculationResponse = new CalculateECFeeResponse();
            FeeCalculationResponse.Amount = response;
            Log.Information("|ECFeeCalculation|d response: {@RequestDTO}", FeeCalculationResponse);


        
            return (FeeCalculationResponse);
        }

        #endregion
    }
}