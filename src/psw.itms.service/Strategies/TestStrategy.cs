using MongoDB.Bson;
using psw.security.Encryption;
using PSW.ITMS.Data.Entities;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service;
using PSW.Lib.Logs;
using System;

namespace PSW.ITMS.Service.Strategies
{
    public class TestStrategy : ApiStrategy<CalculateECFeeRequest, CalculateECFeeResponse>
    {
        CommandRequest request;

        #region Constructors 
        public TestStrategy(CommandRequest request) : base(request)
        {
            Reply = new CommandReply();
            this.request = request;

        }
        #endregion 

        #region Distructors 
        ~TestStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  

        public override CommandReply Execute()
        {
            try
            {
                var ECFeeCalculation = new PSW.ITMS.Service.OGAItemCategory.ECFeeCalculation(request);
                ResponseDTO = ECFeeCalculation.GetECFee(RequestDTO);
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
