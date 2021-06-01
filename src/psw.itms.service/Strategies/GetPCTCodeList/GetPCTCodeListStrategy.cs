using System.Collections.Generic;


using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.Command;
using System;

namespace PSW.ITMS.Service.Strategies
{
    public class GetPCTCodeListStrategy : ApiStrategy<GetPCTCodeListRequest, GetPCTCodeListResponse>
    {
        #region Constructors 
        public GetPCTCodeListStrategy(CommandRequest request) : base(request)
        {
            this.Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~GetPCTCodeListStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  

        public override CommandReply Execute()
        {
            try
            {
                if(string.IsNullOrEmpty(RequestDTO.HsCode))
                {
                    return BadRequestReply("Hscode cannot be null");
                }

                List<string> TempPctCodeList = this.Command.UnitOfWork.RegulatedHSCodeRepository.GetPCTCodeList(RequestDTO.HsCode);

                if(TempPctCodeList == null || TempPctCodeList.Count == 0)
                {
                    ResponseDTO = new GetPCTCodeListResponse 
                    {
                        Message = "Product codes does not exist for the provided hscode."
                    };

                    return OKReply();
                }

                ResponseDTO = new GetPCTCodeListResponse
                {
                    Message = "Product codes exist for provided hscode.",
                    PctCodeList = TempPctCodeList
                };
                
                // Send Command Reply 
                return OKReply();
            }
            catch (Exception ex)
            {
                return InternalServerErrorReply(ex);
            }
        }
        #endregion 

        public List<FactorLOVItemsData> GetLOVItemsForProvidedFactors(List<int> factorlabelList)
        {
            List<FactorLOVItemsData> TempFactorDatalist = new List<FactorLOVItemsData>();

            foreach(var factorID in factorlabelList)
            {
                FactorLOVItemsData TempFactorData = new FactorLOVItemsData();

                TempFactorData.FactorID = factorID;

                TempFactorData.FactorLabel = Command.UnitOfWork.FactorRepository.Get(factorID).Label;

                TempFactorData.FactorLOVItems = Command.UnitOfWork.LOVItemRepository.GetLOVItems(factorID);

                if(TempFactorData.FactorLOVItems != null || TempFactorData.FactorLOVItems.Count ==0)
                {
                    TempFactorDatalist.Add(TempFactorData);
                }
            }

            return TempFactorDatalist;
        }
    }
}
