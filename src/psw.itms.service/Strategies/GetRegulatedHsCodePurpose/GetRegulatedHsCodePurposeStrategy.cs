using System.Collections.Generic;


using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.Command;
using System;
using System.Linq;
using PSW.Lib.Logs;
using PSW.ITMS.Service.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace PSW.ITMS.Service.Strategies
{
    public class GetRegulatedHsCodePurposeStrategy : ApiStrategy<RegulatedHsCodePurposeRequestDTO, RegulatedHsCodePurposeResponseDTO>
    {
        #region Constructors 
        public GetRegulatedHsCodePurposeStrategy(CommandRequest request) : base(request)
        {
            this.Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~GetRegulatedHsCodePurposeStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  

        public override CommandReply Execute()
        {
            try
            {
                if (RequestDTO.AgencyId == 0 || string.IsNullOrEmpty(RequestDTO.DocumentTypeCode) || RequestDTO.TradeTranTypeID == 0)
                {
                    return BadRequestReply("Please provide valid request parameters");
                }

                string MongoDbCollection = this.Command.UnitOfWork.RegulatedHSCodeRepository.Where(
                    new
                    {
                        AgencyId = RequestDTO.AgencyId,
                        RequiredDocumentTypeCode = RequestDTO.DocumentTypeCode,
                        TradeTranTypeId = RequestDTO.TradeTranTypeID
                    }
                    ).FirstOrDefault().CollectionName;

                if(string.IsNullOrEmpty(MongoDbCollection))
                {
                   return BadRequestReply("No record found for provided request parameters"); 
                }

                Log.Information("|{0}|{1}| MongoDb Collection Name : {@MongoDbCollection}", StrategyName, MethodID, MongoDbCollection);

                List<string> ExtHsCodeList = this.Command.UnitOfWork.RegulatedHSCodeRepository.GetExtHsCodeList(RequestDTO.AgencyId,RequestDTO.DocumentTypeCode, RequestDTO.TradeTranTypeID);

                if(ExtHsCodeList == null)
                {
                    string errorMessage = String.Format("No HsCode found in Db for AgencyID : '{0}', RequiredDocumentTypeCode : '{1}', TradeTranTypeId : '{2}'", RequestDTO.AgencyId, RequestDTO.DocumentTypeCode, RequestDTO.TradeTranTypeID); 

                    return BadRequestReply(errorMessage); 
                }

                MongoDbRecordFetcher MDbRecordFetcher;

                IMongoCollection<BsonDocument> documentInCollection;

                try
                {
                    MDbRecordFetcher = new MongoDbRecordFetcher("TARP", MongoDbCollection);
                }
                catch(SystemException ex)
                {
                    Log.Error("|{0}|{1}| Error occured in connecting to MongoDB {@ex}", StrategyName, MethodID, ex.ToString()); 

                   return BadRequestReply("Error occured in connecting to MongoDB");
                }

                try
                {
                    documentInCollection = MDbRecordFetcher.GetCollection();
                }
                catch(SystemException ex)
                {
                   Log.Error("|{0}|{1}| Error occured in fetching record from MongoDB {@ex}", StrategyName, MethodID, ex.ToString()); 

                   return BadRequestReply("Error occured in fetching record from MongoDB");
                }

                List<RegulatedHsCodePurpose> regulatedHsCodePurposeList = GetPurposeListAgainstRegulatedHsCode(ExtHsCodeList, documentInCollection);

                ResponseDTO = new RegulatedHsCodePurposeResponseDTO
                {
                    RegulatedHsCodePurposeList = regulatedHsCodePurposeList
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

        private static List<RegulatedHsCodePurpose> GetPurposeListAgainstRegulatedHsCode(List<string> ExtHsCodeList, IMongoCollection<BsonDocument> documentInCollection)
        {
            List<RegulatedHsCodePurpose> regulatedHsCodePurposes = new List<RegulatedHsCodePurpose>();

            foreach(string hsCode in ExtHsCodeList)
            {
                RegulatedHsCodePurpose hsCodePurpose = new RegulatedHsCodePurpose();
                hsCodePurpose.HsCode = hsCode;

                var filter = Builders<BsonDocument>.Filter.Eq("STATISTICAL SUFFIX", hsCode);
                var projection = Builders<BsonDocument>.Projection.Include("PURPOSE").Exclude("_id");

               hsCodePurpose.PurposeList  = documentInCollection.Find<BsonDocument>(filter).Project(projection)
                                                                                            .ToList()
                                                                                            .Select(
                                                                                                x => x.GetValue("PURPOSE")
                                                                                                .ToString())
                                                                                                .ToList();


                regulatedHsCodePurposes.Add(hsCodePurpose);
                
            }

            return regulatedHsCodePurposes;
        }
    }
}
