using PSW.ITMS.Service.DTO;
using PSW.ITMS.Data;
using PSW.ITMS.Common.Model;
using PSW.ITMS.Common.Constants;
using System.Linq;
using Constants = PSW.ITMS.Common.Enums;


namespace PSW.ITMS.service
{
    public class AQDECFeeCalculation
    {
        private AQDECFeeCalculateRequestDTO request { get; set; }
        private IUnitOfWork unitOfWork { get; set; }

        public int? Quantity { get; set; }

        public decimal? Amount { get; set; }

        public AQDECFeeCalculation(IUnitOfWork unitOfWork, AQDECFeeCalculateRequestDTO request)
        {
            this.unitOfWork = unitOfWork;
            this.request = request;
        }

        public SingleResponseModel<AQDECFeeCalculateResponseDTO> CalculateECFee()
        {
            // var test = unitOfWork.UV_UnitAQDRepository.Where(new { ID = "100001" }).FirstOrDefault();
            var response = new SingleResponseModel<AQDECFeeCalculateResponseDTO>();
            var model = new AQDECFeeCalculateResponseDTO();
            var feeConfiguration = unitOfWork.LPCOFeeConfigurationRepository.Where(new
            {
                HSCodeExt = request.HsCodeExt,
                Unit_ID = request.AgencyUOMId,
                TradeTranTypeID = request.TradeTranTypeID
            }).FirstOrDefault();

            if (feeConfiguration is null)
            {
                var errorMessage = "No Data found ";

                response.IsError = true;
                response.Error = new ErrorResponseModel()
                                     .Category(ErrorCategories.BadRequest)
                                     .Message(errorMessage);
                return response;
            }

            Quantity = request.Quantity == 0 ? request.UserSelectedQuantity : request.Quantity;

            if (request.AgencyUOMId == (int)Constants.AgencyUOMTypes.PackingUnits)
            {
                var feeList = unitOfWork.LPCOFeeConfigurationRepository.Where(new
                {
                    HSCodeExt = request.HsCodeExt,
                    Unit_ID = request.AgencyUOMId,
                    TradeTranTypeID = request.TradeTranTypeID
                }).ToList();

                //Select Fee from list on the basis of quantity
                var feeObj = feeList.Where(d =>
                 (double?)d.QtyRangeFrom >= Quantity
                  && (double?)d.QtyRangeTo <= Quantity)
                  .FirstOrDefault();

                if (feeObj is null) //When quantity exceed max(201) limit
                {
                    Quantity = Quantity / 100;
                    Quantity = Quantity - 1;
                    Amount = ((Quantity * (int)Constants.LPCOConfiguration.IncreasedRate) + 250);
                }
                else
                {
                    Amount = feeObj.Rate;
                }
            }
            else
            {
                Amount = feeConfiguration.Rate * request.Quantity;
            }

            model.Amount = Amount.ToString();
            response.Model = model;
            return response;
        }
    }
}