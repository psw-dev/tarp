using System.Collections.Generic;
using System.Linq;


using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Data.Entities;
using System;
using MongoDB.Bson;
using PSW.ITMS.Service.MongoDB;
using PSW.Lib.Logs;

namespace PSW.ITMS.Service.Strategies
{
    public class UpdateMongoRecordStrategy : ApiStrategy<UpdateMongoDbRecordRequestDTO, UpdateMongoDbRecordResponsetDTO>
    {
        #region Constructors 
        public UpdateMongoRecordStrategy(CommandRequest request) : base(request)
        {
            this.Reply = new CommandReply();
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
                if(RequestDTO.HsCode == null || RequestDTO.Purpose == null || RequestDTO.UpdateKey == null || RequestDTO.UpdateValue == null || RequestDTO.Collection == null)
                {
                    return BadRequestReply("Please provide proper Request parameters");
                }

                MongoDbRecordFetcher MClient = new MongoDbRecordFetcher("TARP", RequestDTO.Collection);

                if(MClient.UpdateRecord(RequestDTO.HsCode, RequestDTO.Purpose, RequestDTO.UpdateKey, RequestDTO.UpdateValue) == true)
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
                Log.Information("|{0}|{1}| Exception Occurred {@ex}", StrategyName, MethodID, ex);
                return InternalServerErrorReply(ex);
            }

        }

        #endregion
    }
}