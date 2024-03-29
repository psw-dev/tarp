

using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.MongoDB;
using PSW.Lib.Logs;
using System;

namespace PSW.ITMS.Service.Strategies
{
    public class UpdateMongoRecordStrategy : ApiStrategy<UpdateMongoDbRecordRequestDTO, UpdateMongoDbRecordResponsetDTO>
    {
        #region Constructors 
        public UpdateMongoRecordStrategy(CommandRequest request) : base(request)
        {
            Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~UpdateMongoRecordStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  

        public override CommandReply Execute()
        {
            try
            {
                Log.Information("|{0}|{1}| Request DTO {@RequestDTO}", StrategyName, MethodID, RequestDTO);
                if (RequestDTO.HsCode == null || RequestDTO.Purpose == null || RequestDTO.UpdateKey == null || RequestDTO.UpdateValue == null || RequestDTO.Collection == null)
                {
                    return BadRequestReply("Please provide proper Request parameters");
                }

                var mongoDBRecordFetcher = new MongoDbRecordFetcher("TARP", RequestDTO.Collection, Environment.GetEnvironmentVariable("MONGODBConnString"));

                if (mongoDBRecordFetcher.UpdateRecord(RequestDTO.HsCode, RequestDTO.Purpose, RequestDTO.UpdateKey, RequestDTO.UpdateValue) == true)
                {
                    ResponseDTO = new UpdateMongoDbRecordResponsetDTO
                    {
                        RecordUpdated = true
                    };

                    return OKReply("Record updated successfully");
                }

                return BadRequestReply("Record update failed");

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