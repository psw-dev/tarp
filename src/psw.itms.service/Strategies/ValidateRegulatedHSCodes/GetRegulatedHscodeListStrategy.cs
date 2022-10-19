using PSW.ITMS.Data.Entities;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.DTO;
using PSW.Lib.Logs;
using System;
using System.Collections.Generic;

namespace PSW.ITMS.Service.Strategies
{
    public class ValidateRegulatedHSCodes : ApiStrategy<ValidateRegulatedHSCodesRequest, ValidateRegulatedHSCodesResponse>
    {
        #region Constructors 
        public ValidateRegulatedHSCodes(CommandRequest request) : base(request)
        {
            Reply = new CommandReply();
        }
        #endregion 
        

        #region Strategy Excecution  

        public override CommandReply Execute()
        {
            try
            {   
                var data = Command.UnitOfWork.RegulatedHSCodeRepository.ValidateRegulatedHSCodes(RequestDTO.HSCodes, RequestDTO.AgencyID, RequestDTO.TradeTranTypeId);
                ResponseDTO = new ValidateRegulatedHSCodesResponse{
                    HSCodes= data
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