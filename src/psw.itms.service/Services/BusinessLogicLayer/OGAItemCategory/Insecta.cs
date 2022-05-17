using System;
using PSW.ITMS.Service.DTO;
using PSW.ITMS.Data;
using System.Linq;
using Constants = PSW.ITMS.Common.Enums;


namespace PSW.ITMS.service
{
    public class Insecta
    {
        private CalculateECFeeRequest request { get; set; }
        private IUnitOfWork unitOfWork { get; set; }

        public Insecta(IUnitOfWork unitOfWork, CalculateECFeeRequest request)
        {
            this.unitOfWork = unitOfWork;
            this.request = request;
        }

        public string CalculateECFee()
        {
            string Amount = "" ; 
            var insectFeeList = unitOfWork.LPCOFeeConfigurationRepository.Where(new
            {
                HSCode = request.HsCode,
                OGAItemCategoryID = request.OGAItemCategoryID,
                Unit_ID = request.UoMId,
                TradeTranTypeID = request.TradeTranTypeID
            }).ToList();

            //Select Fee from list on the basis of quantity
            var insectFeeConf = insectFeeList.Where(d =>
             (double?)d.QtyRangeFrom >= request.Quantity && (double?)d.QtyRangeTo <= request.Quantity)
              .FirstOrDefault();

            if (insectFeeConf is null) //When quantity exceed 201 limit
            {
                request.Quantity = request.Quantity / 100;
                request.Quantity = request.Quantity - 1;
                Amount = ((request.Quantity * (int)Constants.LPCOConfiguration.IncreasedRate) + 250).ToString();
            }
            
            if (insectFeeConf != null)
                Amount = insectFeeConf.Rate.ToString();

             return Amount;
        }
    }
}