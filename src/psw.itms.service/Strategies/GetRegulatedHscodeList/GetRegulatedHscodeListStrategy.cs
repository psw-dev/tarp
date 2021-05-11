using System.Collections.Generic;


using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Data.Entities;
using System;

namespace PSW.ITMS.Service.Strategies
{
    public class GetRegulatedHscodeListStrategy : ApiStrategy<GetRegulatedHscodeListRequest, GetRegulatedHscodeListResponse>
    {
        #region Constructors 
        public GetRegulatedHscodeListStrategy(CommandRequest request) : base(request)
        {
            this.Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~GetRegulatedHscodeListStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  

        public override CommandReply Execute()
        {
            try
            {
                if (RequestDTO.AgencyId != null)
                {
                    List<RegulatedHsCode> RegulatedHsCodeListSearchedByAgency = SearchWithAgencyID(RequestDTO.AgencyId);

                    if (RegulatedHsCodeListSearchedByAgency == null || RegulatedHsCodeListSearchedByAgency.Count == 0)
                    {
                        return BadRequestReply("Hscodes not available against provided Agency");
                    }

                    ResponseDTO = new GetRegulatedHscodeListResponse
                    {
                        RegulatedHsCodeList = RegulatedHsCodeListSearchedByAgency
                    };
                }

                List<RegulatedHsCode> RegulatedHsCodeList = GetHsCodeList();

                if (RegulatedHsCodeList == null || RegulatedHsCodeList.Count == 0)
                {
                    return BadRequestReply("Hscodes not available against provided Agency");
                }

                ResponseDTO = new GetRegulatedHscodeListResponse
                {
                    RegulatedHsCodeList = RegulatedHsCodeList
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

        public List<RegulatedHsCode> GetHsCodeList()
        {
            List<RegulatedHsCode> RegulatedHsCodeList = this.Command.UnitOfWork.RegulatedHSCodeRepository.GetRegulatedHsCodeList();

            return RegulatedHsCodeList;
        }

        public List<RegulatedHsCode> SearchWithAgencyID(string agencyId)
        {
            List<RegulatedHsCode> RegulatedHsCodeList = this.Command.UnitOfWork.RegulatedHSCodeRepository.GetRegulatedHsCodeList(agencyId);

            return RegulatedHsCodeList;
        }
    }
}