using PSW.ITMS.Common;
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
        RedisCacheService _RedisCacheService = null;
        public RedisCacheService RedisService
        {
            get
            {
                if (_RedisCacheService == null)
                {
                    _RedisCacheService = new RedisCacheService(RedisConnection);
                }
                return _RedisCacheService;
            }
        }

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
            string keyHSCodeWithAgency = "TARP:REGULATEDHSCODES:" + RequestDTO.AgencyId;
            string keyHSCodeWithAgencyAndDocumentType = "TARP:REGULATEDHSCODES:" + RequestDTO.AgencyId + ":" + RequestDTO.DocumentTypeCode;
            string keyHSCodeOnly = "TARP:REGULATEDHSCODES";
            try
            {
                Log.Information("|{0}|{1}| Request DTO {@RequestDTO}", StrategyName, MethodID, RequestDTO);
                var regulatedHSCodeList = new List<ViewRegulatedHsCode>();

                //Get Regulated Hscode list filtered on base of AgencyId 
                if (RequestDTO.AgencyId != 0 && RequestDTO.DocumentTypeCode == null)
                {
                    if (RedisService.KeyExists(keyHSCodeWithAgency))
                    {
                        Log.Information($"Getting Key:{keyHSCodeWithAgency} data from Redis");
                        regulatedHSCodeList = RedisService.Get<List<ViewRegulatedHsCode>>(keyHSCodeWithAgency);
                        ResponseDTO = new GetRegulatedHscodeListResponse
                        {
                            RegulatedHsCodeList = regulatedHSCodeList
                        };

                        // Send Command Reply 
                        return OKReply();
                    }
                    else
                    {
                        regulatedHSCodeList = Command.UnitOfWork.RegulatedHSCodeRepository.GetRegulatedHsCodeList(RequestDTO.AgencyId);
                    }
                    if (regulatedHSCodeList == null || regulatedHSCodeList.Count == 0)
                    {
                        return BadRequestReply("Hscodes not available against provided Agency");
                    }
                }

                //Get Regulated Hscode list filtered on base of AgencyId and DocumentTypeCode
                else if (RequestDTO.AgencyId != 0 && RequestDTO.DocumentTypeCode != null)
                {
                    if (RedisService.KeyExists(keyHSCodeWithAgencyAndDocumentType))
                    {
                        Log.Information($"Getting Key:{keyHSCodeWithAgencyAndDocumentType} data from Redis");
                        regulatedHSCodeList = RedisService.Get<List<ViewRegulatedHsCode>>(keyHSCodeWithAgencyAndDocumentType);
                        ResponseDTO = new GetRegulatedHscodeListResponse
                        {
                            RegulatedHsCodeList = regulatedHSCodeList
                        };

                        // Send Command Reply 
                        return OKReply();
                    }
                    else
                    {
                        regulatedHSCodeList = Command.UnitOfWork.RegulatedHSCodeRepository.GetRegulatedHsCodeList(RequestDTO.AgencyId, RequestDTO.DocumentTypeCode);
                    }
                    if (regulatedHSCodeList == null || regulatedHSCodeList.Count == 0)
                    {
                        return BadRequestReply("Hscodes not available against provided Agency and Document");
                    }
                }

                else
                {
                    if (RedisService.KeyExists(keyHSCodeOnly))
                    {
                        Log.Information($"Getting Key:{keyHSCodeOnly} data from Redis");
                        regulatedHSCodeList = RedisService.Get<List<ViewRegulatedHsCode>>(keyHSCodeOnly);
                        ResponseDTO = new GetRegulatedHscodeListResponse
                        {
                            RegulatedHsCodeList = regulatedHSCodeList
                        };

                        // Send Command Reply 
                        return OKReply();
                    }
                    else
                    {
                        regulatedHSCodeList = Command.UnitOfWork.RegulatedHSCodeRepository.GetRegulatedHsCodeList();
                    }
                    if (regulatedHSCodeList == null || regulatedHSCodeList.Count == 0)
                    {
                        return BadRequestReply("Hscodes not available");
                    }
                }

                //Get hscodeDetails
                foreach (var regulatedHscode in regulatedHSCodeList)
                {
                    regulatedHscode.HsCodeDetailsList = Command.UnitOfWork.RegulatedHSCodeRepository.GetHsCodeDetailList(regulatedHscode.HsCode, RequestDTO.DocumentTypeCode, RequestDTO.AgencyId);
                }

                if (RequestDTO.AgencyId != 0 && RequestDTO.DocumentTypeCode == null)
                {
                    RedisService.Set(keyHSCodeWithAgency, regulatedHSCodeList, TimeSpan.FromHours(24));
                    Log.Information($"Setting Key:{keyHSCodeWithAgency} data in Redis");
                }
                else if (RequestDTO.AgencyId != 0 && RequestDTO.DocumentTypeCode != null)
                {
                    RedisService.Set(keyHSCodeWithAgencyAndDocumentType, regulatedHSCodeList, TimeSpan.FromHours(24));
                    Log.Information($"Setting Key:{keyHSCodeWithAgencyAndDocumentType} data in Redis");
                }
                else
                {
                    RedisService.Set(keyHSCodeOnly, regulatedHSCodeList, TimeSpan.FromHours(24));
                    Log.Information($"Setting Key:{regulatedHSCodeList} data in Redis");
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