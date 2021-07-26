using System.Collections.Generic;


using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Data.Entities;
using System;
using PSW.Lib.Logs;

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
                List<ViewRegulatedHsCode> RegulatedHsCodeList = new List<ViewRegulatedHsCode>();

                //Get Regulated Hscode list filtered on base of AgencyId 
                if (RequestDTO.AgencyId != 0 && RequestDTO.DocumentTypeCode == null)
                {
                    RegulatedHsCodeList = SearchWithAgencyID(RequestDTO.AgencyId);

                    if (RegulatedHsCodeList == null || RegulatedHsCodeList.Count == 0)
                    {
                        return BadRequestReply("Hscodes not available against provided Agency");
                    }
                }

                //Get Regulated Hscode list filtered on base of AgencyId and DocumentTypeCode
                else if (RequestDTO.AgencyId != 0 && RequestDTO.DocumentTypeCode != null)
                {
                    RegulatedHsCodeList = SearchWithAgencyIDAndDocumentTypeCode(RequestDTO.AgencyId, RequestDTO.DocumentTypeCode);

                    if (RegulatedHsCodeList == null || RegulatedHsCodeList.Count == 0)
                    {
                        return BadRequestReply("Hscodes not available against provided Agency");
                    }
                }

                else
                {
                   RegulatedHsCodeList = this.Command.UnitOfWork.RegulatedHSCodeRepository.GetRegulatedHsCodeList();

                    if (RegulatedHsCodeList == null || RegulatedHsCodeList.Count == 0)
                    {
                        return BadRequestReply("Hscodes not available against provided Agency");
                    }
                }

                //Get hscodeDetails
                foreach (var regulatedHscode in RegulatedHsCodeList)
                {
                    regulatedHscode.HsCodeDetailsList = this.Command.UnitOfWork.RegulatedHSCodeRepository.GetHsCodeDetailList(regulatedHscode.HsCode);
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
                Log.Error("|{0}|{1}| Exception Occurred {@ex}", StrategyName, MethodID, ex);
                return InternalServerErrorReply(ex);
            }
        }
        #endregion

        public List<ViewRegulatedHsCode> SearchWithAgencyID(int agencyId)
        {
            List<ViewRegulatedHsCode> RegulatedHsCodeList = this.Command.UnitOfWork.RegulatedHSCodeRepository.GetRegulatedHsCodeList(agencyId);

            return RegulatedHsCodeList;
        }

        public List<ViewRegulatedHsCode> SearchWithAgencyIDAndDocumentTypeCode(int agencyId, string documentTypeCode)
        {
            List<ViewRegulatedHsCode> RegulatedHsCodeList = this.Command.UnitOfWork.RegulatedHSCodeRepository.GetRegulatedHsCodeList(agencyId, documentTypeCode);

            return RegulatedHsCodeList;
        }
    }
}