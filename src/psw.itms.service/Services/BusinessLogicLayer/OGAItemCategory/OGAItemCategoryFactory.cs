using PSW.ITMS.Service.DTO;
using PSW.ITMS.Data;
using PSW.ITMS.service;
using Constants = PSW.ITMS.Common.Enums;

namespace PSW.ITMS.Service
{
    public class OGAItemCategoryFactory
    {
        private CalculateECFeeRequest request { get; set; }
        private IUnitOfWork unitOfWork { get; set; }


        #region Constructors
        public OGAItemCategoryFactory(IUnitOfWork unitOfWork, CalculateECFeeRequest request)
        {
            this.unitOfWork = unitOfWork;
            this.request = request;
        }
        #endregion

        #region Public Methods
        public string CalculateECFee()
        {
            string response = "";
            switch (request.OGAItemCategoryID)
            {
                case (int)Constants.OGAItemCategoryTypes.LiveAnimalOtherThanPetAndBird:
                    var liveAnimalOtherThanPetAndBirdObj = new LiveAnimalOtherThanPetAndBird(unitOfWork, request);
                    response = liveAnimalOtherThanPetAndBirdObj.CalculateECFee();
                    return response;
                case (int)Constants.OGAItemCategoryTypes.LiveBird:
                    var liveBirdObj = new LiveBird(unitOfWork, request);
                    response = liveBirdObj.CalculateECFee();
                    return response;
                case (int)Constants.OGAItemCategoryTypes.AnimalProductOtherThanSemen:
                    var animalProductOtherThanSemenObj = new AnimalProductOtherThanSemen(unitOfWork, request);
                    response = animalProductOtherThanSemenObj.CalculateECFee();
                    return response;
                case (int)Constants.OGAItemCategoryTypes.Insecta:
                    var insectaObj = new Insecta(unitOfWork, request);
                    response = insectaObj.CalculateECFee();
                    return response;
                default:
                    return null;
            }
        }

        #endregion
    }
}