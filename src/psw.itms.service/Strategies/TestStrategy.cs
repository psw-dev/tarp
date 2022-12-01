using PSW.ITMS.Common.Model;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.DTO;
using PSW.Lib.Logs;
using System;

namespace PSW.ITMS.Service.Strategies
{
    public class TestStrategy : ApiStrategy<AQDECFeeCalculateRequestDTO, string>
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
                var AQDECFeeCalculation = new PSW.ITMS.service.AQDECFeeCalculation(Command.UnitOfWork,RequestDTO);
                var responseModel = AQDECFeeCalculation.CalculateECFee();
                if (!responseModel.IsError)
                {
                    ResponseDTO = responseModel.Model.Amount;
                    return OKReply();
                }
                else
                {
                    Log.Information("Response {@message}", responseModel.Error.InternalError.Message);
                    return BadRequestReply(responseModel.Error.InternalError.Message);
                }
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
