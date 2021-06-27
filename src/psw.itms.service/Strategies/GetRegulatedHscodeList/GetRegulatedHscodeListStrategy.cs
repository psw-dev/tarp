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
                    List<ViewRegulatedHsCode> RegulatedHsCodeListSearchedByAgency = SearchWithAgencyID(RequestDTO.AgencyId);

                    if (RegulatedHsCodeListSearchedByAgency == null || RegulatedHsCodeListSearchedByAgency.Count == 0)
                    {
                        return BadRequestReply("Hscodes not available against provided Agency");
                    }

                    ResponseDTO = new GetRegulatedHscodeListResponse
                    {
                        RegulatedHsCodeList = RegulatedHsCodeListSearchedByAgency
                    };
                }

                List<ViewRegulatedHsCode> RegulatedHsCodeList = this.Command.UnitOfWork.RegulatedHSCodeRepository.GetRegulatedHsCodeList();

                if (RegulatedHsCodeList == null || RegulatedHsCodeList.Count == 0)
                {
                    return BadRequestReply("Hscodes not available against provided Agency");
                }

                //Get ProductCodeList of Hscode

                foreach(var regulatedHscode in RegulatedHsCodeList)
                {
                    regulatedHscode.ProductCode = this.Command.UnitOfWork.RegulatedHSCodeRepository.GetPCTCodeList(regulatedHscode.HsCode);
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

        public List<ViewRegulatedHsCode> SearchWithAgencyID(string agencyId)
        {
            List<ViewRegulatedHsCode> RegulatedHsCodeList = this.Command.UnitOfWork.RegulatedHSCodeRepository.GetRegulatedHsCodeList(agencyId);

            return RegulatedHsCodeList;
        }
    }
}