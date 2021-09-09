using PSW.ITMS.Data.Entities;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.DTO;
using PSW.Lib.Logs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSW.ITMS.Service.Strategies
{
    public class GetAgencyListStrategy : ApiStrategy<GetListOfAgencyAgainstHscodeRequest, GetListOfAgencyAgainstHscodeResponse>
    {
        #region Constructors 
        public GetAgencyListStrategy(CommandRequest request) : base(request)
        {
            Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~GetAgencyListStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  

        public override CommandReply Execute()
        {
            try
            {
                if (string.IsNullOrEmpty(RequestDTO.HsCode) || string.IsNullOrEmpty(RequestDTO.DocumentCode))
                {
                    return BadRequestReply("Please provide valid Hscode");
                }

                var tempAgencyList = Command.UnitOfWork.RegulatedHSCodeRepository.GetAgencyListAgainstHscode(RequestDTO.HsCode, RequestDTO.DocumentCode);

                Log.Information("|{0}|{1}| Agency list fetched for database {@tempAgencyList}", StrategyName, MethodID, tempAgencyList);

                if (tempAgencyList == null || tempAgencyList.Count == 0)
                {
                    return BadRequestReply("Agency details not found against provided Hscode");
                }

                var distinctAgencyList = tempAgencyList.Distinct(new objCompare()).ToList();

                ResponseDTO = new GetListOfAgencyAgainstHscodeResponse
                {
                    AgencyList = distinctAgencyList
                };

                Log.Information("|{0}|{1}| Response {@ResponseDTO}", StrategyName, MethodID, ResponseDTO);

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
    }

    public class objCompare : IEqualityComparer<AgencyList>
    {
        public bool Equals(AgencyList x, AgencyList y)
        {
            return Equals(x.Id, y.Id);
        }

        public int GetHashCode(AgencyList obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
