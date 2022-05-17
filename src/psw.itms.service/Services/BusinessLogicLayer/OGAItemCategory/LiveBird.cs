using System;
using PSW.ITMS.Service.DTO;
using PSW.ITMS.Data;
using System.Linq;
using Constants = PSW.ITMS.Common.Enums;


namespace PSW.ITMS.service
{
    public class LiveBird
    {
        private CalculateECFeeRequest request { get; set; }
        private IUnitOfWork unitOfWork { get; set; }

        public LiveBird(IUnitOfWork unitOfWork, CalculateECFeeRequest request)
        {
            this.unitOfWork = unitOfWork;
            this.request = request;
        }

        public string CalculateECFee()
        {
            double quantity;
            var feeConf = unitOfWork.LPCOFeeConfigurationRepository.Where(new
            {
                HSCode = request.HsCode,
                OGAItemCategoryID = request.OGAItemCategoryID,
                Unit_ID = request.UoMId,
                TradeTranTypeID = request.TradeTranTypeID
            }).FirstOrDefault();

            if (feeConf is null)
                return null;

            if (request.UoMId == (int)Constants.LPCOConfiguration.CageUoM) //In case of cages
            {
                quantity = Convert.ToDouble(request.Quantity) / (int)Constants.LPCOConfiguration.PerCageRate;
                request.Quantity = (int)Math.Ceiling(quantity);
            }

            decimal? Amount = feeConf.Rate * request.Quantity;
            return Amount.ToString();
        }
    }
}