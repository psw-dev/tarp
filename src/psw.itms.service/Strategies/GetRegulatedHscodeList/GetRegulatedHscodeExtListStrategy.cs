using PSW.ITMS.Data.Entities;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.DTO;
using PSW.Lib.Logs;
using System;
using System.Collections.Generic;

namespace PSW.ITMS.Service.Strategies
{
    public class GetRegulatedHSCodeExtListStrategy : ApiStrategy<GetRegulatedHscodeListRequest, GetRegulatedHSCodeExtListResponse>
    {
        #region Constructors 
        public GetRegulatedHSCodeExtListStrategy(CommandRequest request) : base(request)
        {
            Reply = new CommandReply();
        }
        #endregion 

        #region Strategy Excecution  
        public override CommandReply Execute()
        {
            try
            {
                var regulatedHSCodeList = new List<ViewRegulatedHsCodeExt>();

                //Get Regulated Hscode list filtered on base of AgencyId and chapter
                if (RequestDTO.AgencyId != 0 && RequestDTO.Chapter != null && RequestDTO.DocumentTypeCode == null)
                {
                    regulatedHSCodeList = Command.UnitOfWork.RegulatedHSCodeRepository.GetRegulatedHsCodeExtList(RequestDTO.AgencyId, RequestDTO.Chapter);

                    if (regulatedHSCodeList == null || regulatedHSCodeList.Count == 0)
                    {
                        return BadRequestReply("Hscodes not available against provided Agency and chapter");
                    }
                }

                //Get Regulated Hscode list filtered on base of AgencyId 
                if (RequestDTO.AgencyId != 0 && RequestDTO.DocumentTypeCode == null)
                {
                    regulatedHSCodeList = Command.UnitOfWork.RegulatedHSCodeRepository.GetRegulatedHsCodeExtList(RequestDTO.AgencyId);

                    if (regulatedHSCodeList == null || regulatedHSCodeList.Count == 0)
                    {
                        return BadRequestReply("Hscodes not available against provided Agency");
                    }
                }

                else
                {
                    regulatedHSCodeList = Command.UnitOfWork.RegulatedHSCodeRepository.GetRegulatedHsCodeExtList();

                    if (regulatedHSCodeList == null || regulatedHSCodeList.Count == 0)
                    {
                        return BadRequestReply("Hscodes not available against provided Agency");
                    }
                }

                ResponseDTO = new GetRegulatedHSCodeExtListResponse
                {
                    RegulatedHsCodeExtList = regulatedHSCodeList
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