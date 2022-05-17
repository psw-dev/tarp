using PSW.ITMS.Service.DTO;
using PSW.ITMS.Data;
using System.Linq;

namespace PSW.ITMS.service
{
    public class LiveAnimalOtherThanPetAndBird
    {
        private CalculateECFeeRequest request { get; set; }
        private IUnitOfWork unitOfWork { get; set; }

        public LiveAnimalOtherThanPetAndBird(IUnitOfWork unitOfWork, CalculateECFeeRequest request)
        {
            this.unitOfWork = unitOfWork;
            this.request = request;
        }

        public string CalculateECFee()
        {
            var feeConf = unitOfWork.LPCOFeeConfigurationRepository.Where(new
            {
                HSCode = request.HsCode,
                OGAItemCategoryID = request.OGAItemCategoryID,
                Unit_ID = request.UoMId,
                TradeTranTypeID = request.TradeTranTypeID
            }).FirstOrDefault();

            if (feeConf is null)
                return null;

            decimal? Amount = feeConf.Rate * request.Quantity;
            return Amount.ToString();
        }
    }
}