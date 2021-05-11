using System.Collections.Generic;


using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Data.Entities;
using System;

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
                if(string.IsNullOrEmpty(RequestDTO.HsCode))
                {
                    return BadRequestReply("Please provide valid Hscode");
                }

                List<AgencyList> TempAgencyList = this.Command.UnitOfWork.RegulatedHSCodeRepository.GetAgencyListAgainstHscode(RequestDTO.HsCode);

                if(TempAgencyList == null || TempAgencyList.Count == 0)
                {
                    return BadRequestReply("Agency details not found against provided Hscode");
                }

                ResponseDTO = new GetListOfAgencyAgainstHscodeResponse
                {
                    AgencyList = TempAgencyList
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
}
