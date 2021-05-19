using System.Collections.Generic;


using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Data.Entities;
using System;
using System.Linq;

namespace PSW.ITMS.Service.Strategies
{
    public class GetAgencyListStrategy : ApiStrategy<GetListOfAgencyAgainstHscodeRequest, GetListOfAgencyAgainstHscodeResponse>
    {
        #region Constructors 
        public GetAgencyListStrategy(CommandRequest request) : base(request)
        {
            this.Reply = new CommandReply();
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

                List<AgencyList> TempAgencyList = this.Command.UnitOfWork.RegulatedHSCodeRepository.GetAgencyListAgainstHscode(RequestDTO.HsCode, RequestDTO.DocumentCode);

                List<AgencyList> DistinctAgencyList = TempAgencyList.Distinct(new objCompare()).ToList();

                if (TempAgencyList == null || TempAgencyList.Count == 0)
                {
                    return BadRequestReply("Agency details not found against provided Hscode");
                }

                ResponseDTO = new GetListOfAgencyAgainstHscodeResponse
                {
                    AgencyList = DistinctAgencyList
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
