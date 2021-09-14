using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.DTO;
using PSW.Lib.Logs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSW.ITMS.Service.Strategies
{
    public class GetFactorLOVItemsStrategy : ApiStrategy<GetFactorLovItemsRequest, GetFactorLovItemsResponse>
    {
        #region Constructors 
        public GetFactorLOVItemsStrategy(CommandRequest request) : base(request)
        {
            Reply = new CommandReply();
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
                if (RequestDTO.FactorList == null || RequestDTO.FactorList.Count == 0)
                {
                    return BadRequestReply("Please provide valid request parameters");
                }

                var tempFactorLOVItems = GetLOVItemsForProvidedFactors(RequestDTO.FactorList);

                if (tempFactorLOVItems == null || tempFactorLOVItems.Count == 0)
                {
                    ResponseDTO = new GetFactorLovItemsResponse
                    {
                        FactorLOVItemsList = new List<FactorLOVItemsData>()
                    };
                    return OKReply("LOV data not available for provided factor list");
                }

                ResponseDTO = new GetFactorLovItemsResponse
                {
                    FactorLOVItemsList = tempFactorLOVItems
                };

                // Send Command Reply 
                return OKReply();
            }
            catch (Exception ex)
            {
                Log.Error("|{0}|{1}| Exception Occurred {@ex}", StrategyName, MethodID, ex);
                return InternalServerErrorReply(ex);
            }
        }
        #endregion 

        public List<FactorLOVItemsData> GetLOVItemsForProvidedFactors(List<int> factorlabelList)
        {
            var tempFactorDatalist = new List<FactorLOVItemsData>();

            foreach (var factorID in factorlabelList)
            {
                var tempFactorData = new FactorLOVItemsData();

                tempFactorData.FactorID = factorID;


                var factorData = Command.UnitOfWork.FactorRepository?.Where(new { ID = factorID, ISLOV = 1 }).FirstOrDefault();

                if (factorData == null)
                {
                    continue;
                }
                else
                {

                    tempFactorData.FactorLabel = factorData.Label;
                    tempFactorData.FactorCode = factorData.FactorCode;

                    tempFactorData.FactorLOVItems = Command.UnitOfWork.LOVItemRepository.GetLOVItems(factorData.LOVID);

                    if (tempFactorData.FactorLOVItems != null || tempFactorData.FactorLOVItems.Count == 0)
                    {
                        tempFactorDatalist.Add(tempFactorData);
                    }
                }
            }

            return tempFactorDatalist;
        }
    }
}
