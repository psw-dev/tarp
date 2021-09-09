using PSW.ITMS.Data.Entities;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.DTO;
using PSW.Lib.Logs;
using System;
using System.Collections.Generic;

namespace PSW.ITMS.Service.Strategies
{
    public class GetRegulatedHSCodeListStrategy : ApiStrategy<GetRegulatedHscodeListRequest, GetRegulatedHscodeListResponse>
    {
        #region Constructors 
        public GetRegulatedHSCodeListStrategy(CommandRequest request) : base(request)
        {
            Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~GetRegulatedHSCodeListStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  

        public override CommandReply Execute()
        {
            try
            {
                var regulatedHSCodeList = new List<ViewRegulatedHsCode>();

                //Get Regulated Hscode list filtered on base of AgencyId 
                if (RequestDTO.AgencyId != 0 && RequestDTO.DocumentTypeCode == null)
                {
                    regulatedHSCodeList = Command.UnitOfWork.RegulatedHSCodeRepository.GetRegulatedHsCodeList(RequestDTO.AgencyId);

                    if (regulatedHSCodeList == null || regulatedHSCodeList.Count == 0)
                    {
                        return BadRequestReply("Hscodes not available against provided Agency");
                    }
                }

                //Get Regulated Hscode list filtered on base of AgencyId and DocumentTypeCode
                else if (RequestDTO.AgencyId != 0 && RequestDTO.DocumentTypeCode != null)
                {
                    regulatedHSCodeList = Command.UnitOfWork.RegulatedHSCodeRepository.GetRegulatedHsCodeList(RequestDTO.AgencyId, RequestDTO.DocumentTypeCode);

                    if (regulatedHSCodeList == null || regulatedHSCodeList.Count == 0)
                    {
                        return BadRequestReply("Hscodes not available against provided Agency");
                    }
                }

                else
                {
                    regulatedHSCodeList = Command.UnitOfWork.RegulatedHSCodeRepository.GetRegulatedHsCodeList();

                    if (regulatedHSCodeList == null || regulatedHSCodeList.Count == 0)
                    {
                        return BadRequestReply("Hscodes not available against provided Agency");
                    }
                }

                //Get hscodeDetails
                foreach (var regulatedHscode in regulatedHSCodeList)
                {
                    regulatedHscode.HsCodeDetailsList = Command.UnitOfWork.RegulatedHSCodeRepository.GetHsCodeDetailList(regulatedHscode.HsCode, RequestDTO.DocumentTypeCode, RequestDTO.AgencyId);
                }

                ResponseDTO = new GetRegulatedHscodeListResponse
                {
                    RegulatedHsCodeList = regulatedHSCodeList
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
    }
}