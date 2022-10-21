using MongoDB.Bson;
using PSW.ITMS.Data.Entities;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.MongoDB;
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
                var RegulatedDPPImportHSCodes = Command.UnitOfWork.RegulatedHSCodeRepository.ValidateRegulatedHSCodes(RequestDTO.HSCodes, RequestDTO.AgencyID, RequestDTO.TradeTranTypeId);
                var responseList = new List<string>();

                var mongoDoc = new BsonDocument();
                MongoDbRecordFetcher mongoDBRecordFetcher;
                var DPPIMPORT = "DPP-Import";
                try
                {
                    mongoDBRecordFetcher = new MongoDbRecordFetcher("TARP", DPPIMPORT, Environment.GetEnvironmentVariable("MONGODBConnString"));
                    foreach(var x in RegulatedDPPImportHSCodes)
                    {
                        var PSIRequired = mongoDBRecordFetcher.GetPSIInfo(mongoDoc,x);
                        if(PSIRequired)
                        {
                            responseList.Add(x);
                        }    
                    }

                    ResponseDTO = new ValidateRegulatedHSCodesResponse{
                        HSCodes= responseList
                    };
                    
                }
                catch(Exception e)
                {
                    Log.Error("|{0}|{1}| Exception Occurred while establishing connection with MongoDB {@e}", StrategyName, MethodID, e);
                }
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