using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.DTO;
using PSW.Lib.Logs;
using System;
using System.Collections.Generic;

namespace PSW.ITMS.Service.Strategies
{
    public class GetPCTCodeListStrategy : ApiStrategy<GetPCTCodeListRequest, GetPCTCodeListResponse>
    {
        #region Constructors 
        public GetPCTCodeListStrategy(CommandRequest request) : base(request)
        {
            Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~GetPCTCodeListStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  

        public override CommandReply Execute()
        {
            try
            {
                if (string.IsNullOrEmpty(RequestDTO.HsCode))
                {
                    return BadRequestReply("Hscode cannot be null");
                }

                var tempPctCodeList = Command.UnitOfWork.RegulatedHSCodeRepository.GetPCTCodeList(RequestDTO.HsCode);

                if (tempPctCodeList == null || tempPctCodeList.Count == 0)
                {
                    ResponseDTO = new GetPCTCodeListResponse
                    {
                        PctCodeList = new List<string>(),
                        Message = "Product codes does not exist for the provided hscode."
                    };

                    Log.Information("|{0}|{1}| Response DTO : {@ResponseDTO}", StrategyName, MethodID, ResponseDTO);

                    return OKReply();
                }

                ResponseDTO = new GetPCTCodeListResponse
                {
                    Message = "Product codes exist for provided hscode.",
                    PctCodeList = tempPctCodeList
                };

                Log.Information("|{0}|{1}| Response DTO : {@ResponseDTO}", StrategyName, MethodID, ResponseDTO);

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
