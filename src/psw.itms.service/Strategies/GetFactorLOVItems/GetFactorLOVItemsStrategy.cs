using System.Collections.Generic;


using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.Command;
using System;

namespace PSW.ITMS.Service.Strategies
{
    public class GetFactorLOVItemsStrategy : ApiStrategy<GetFactorLovItemsRequest, GetFactorLovItemsResponse>
    {
        #region Constructors 
        public GetFactorLOVItemsStrategy(CommandRequest request) : base(request)
        {
            this.Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~GetFactorLOVItemsStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  

        public override CommandReply Execute()
        {
            try
            {
                if(RequestDTO.FactorList == null || RequestDTO.FactorList.Count == 0)
                {
                    return BadRequestReply("Please provide valid request parameters");
                }

                var TempFactorLOVItems =  GetLOVItemsForProvidedFactors(RequestDTO.FactorList);

                if(TempFactorLOVItems == null || TempFactorLOVItems.Count ==0)
                {
                    return BadRequestReply("LOV data not available for provided factor list");
                }

                ResponseDTO = new GetFactorLovItemsResponse
                {
                    FactorLOVItemsList = TempFactorLOVItems
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

        public List<FactorLOVItemsData> GetLOVItemsForProvidedFactors(List<string> factorlabelList)
        {
            List<FactorLOVItemsData> TempFactorDatalist = new List<FactorLOVItemsData>();

            foreach(var factorLabel in factorlabelList)
            {
                FactorLOVItemsData TempFactorData = new FactorLOVItemsData();

                TempFactorData.FactorLabel = factorLabel;

                TempFactorData.FactorLOVItems = Command.UnitOfWork.LOVItemRepository.GetLOVItems(factorLabel);

                if(TempFactorData.FactorLOVItems != null || TempFactorData.FactorLOVItems.Count ==0)
                {
                    TempFactorDatalist.Add(TempFactorData);
                }
            }

            return TempFactorDatalist;
        }
    }
}
