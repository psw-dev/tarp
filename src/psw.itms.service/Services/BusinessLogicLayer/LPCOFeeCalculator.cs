using System.Collections.Generic;
using PSW.ITMS.Data.Entities;
using PSW.ITMS.Service.DTO;

namespace PSW.ITMS.service
{
    public class LPCOFeeCalculator
    {
        private List<LPCOFeeConfiguration> LPCOFeeEntity { get; set; }
        private GetDocumentRequirementRequest Request { get; set; }

        public LPCOFeeCalculator(List<LPCOFeeConfiguration> lpcoFeeEntity, GetDocumentRequirementRequest request)
        {
            this.LPCOFeeEntity = lpcoFeeEntity;
            this.Request = request;
        }

        public LPCOFeeDTO Calculate()
        {
            LPCOFeeDTO calculatedFee = new LPCOFeeDTO();
            calculatedFee.Fee = 0m;
            calculatedFee.AdditionalAmount = 0m;
            calculatedFee.AdditionalAmountOn = string.Empty;

            if(LPCOFeeEntity == null || LPCOFeeEntity.Count <= 0)
            {
                return calculatedFee;
            }

            var calculationBasis = LPCOFeeEntity[0].CalculationBasis;

            switch(calculationBasis)
            {
                case "Quantity":
                    break;
                case "Fixed":
                    break;
                case "AdVal":
                    calculatedFee = CalculateAsAdVal(LPCOFeeEntity[0], Request.ImportExportValue);     // Handled only for PSQCA right now
                    break;
                default:
                    break;
            }

            return calculatedFee;
        }

        // Handled only for PSQCA right now
        private LPCOFeeDTO CalculateAsAdVal(LPCOFeeConfiguration lpcoFeeEntity, decimal importExportValue)
        {
            LPCOFeeDTO lpcoFee = new LPCOFeeDTO();
            var result = 0m;
            var percentage = lpcoFeeEntity.Rate == null ? 0m : (lpcoFeeEntity.Rate/100);
            var minAmount = lpcoFeeEntity.MinAmount ?? 0m;
            var additionalAmount = lpcoFeeEntity.AdditionalAmount ?? 0m;
            var additionalAmountOn = lpcoFeeEntity.AdditionalAmountOn ?? string.Empty;

            var percentageOfValue = percentage * importExportValue;

            if(percentageOfValue > minAmount)
            {
                result = (decimal)(percentageOfValue);
            }
            else
            {
                result = (decimal)(minAmount);
            }

            lpcoFee.Fee = result;
            lpcoFee.AdditionalAmount = additionalAmount;
            lpcoFee.AdditionalAmountOn = additionalAmountOn;

            return lpcoFee;
        }

        private decimal CalculateAsFixed(LPCOFeeConfiguration lpcoFeeEntity)
        {
            var result = 0m;
            return result;
        }

        private decimal CalculateAsQuantity(LPCOFeeConfiguration lpcoFeeEntity)
        {
            var result = 0m;
            return result;
        }
    }
}