using MongoDB.Bson;
using MongoDB.Driver;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.MongoDB;
using PSW.Lib.Logs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSW.ITMS.Service.Strategies
{
    public class GetRegulatedHsCodePurposeStrategy : ApiStrategy<RegulatedHsCodePurposeRequestDTO, RegulatedHsCodePurposeResponseDTO>
    {
        #region Constructors 
        public GetRegulatedHsCodePurposeStrategy(CommandRequest request) : base(request)
        {
            Reply = new CommandReply();
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
                Log.Information("|{0}|{1}| Request DTO {@RequestDTO}", StrategyName, MethodID, RequestDTO);
                if (RequestDTO.AgencyId == 0 || string.IsNullOrEmpty(RequestDTO.DocumentTypeCode) || RequestDTO.TradeTranTypeID == 0)
                {
                    return BadRequestReply("Please provide valid request parameters");
                }

                string mongoDbCollection = null;
                if (RequestDTO.TradeTranTypeID == -1)
                {
                    mongoDbCollection = Command.UnitOfWork.RegulatedHSCodeRepository.GetActiveHsCode(
                      RequestDTO.AgencyId.ToString(),
                      RequestDTO.DocumentTypeCode
                  ).CollectionName;
                }
                else
                {
                    mongoDbCollection = Command.UnitOfWork.RegulatedHSCodeRepository.GetActiveHsCode(
                       RequestDTO.AgencyId.ToString(),
                       RequestDTO.TradeTranTypeID,
                       RequestDTO.DocumentTypeCode
                   ).CollectionName;
                }

                if (string.IsNullOrEmpty(mongoDbCollection))
                {
                    return BadRequestReply("No record found for provided request parameters");
                }

                Log.Information("|{0}|{1}| MongoDb Collection Name : {@mongoDbCollection}", StrategyName, MethodID, mongoDbCollection);

                List<string> extHsCodeList = null;
                if (RequestDTO.TradeTranTypeID == -1)
                {
                    extHsCodeList = Command.UnitOfWork.RegulatedHSCodeRepository.GetExtHsCodeList(RequestDTO.AgencyId, RequestDTO.DocumentTypeCode);
                }
                else
                {
                    extHsCodeList = Command.UnitOfWork.RegulatedHSCodeRepository.GetExtHsCodeList(RequestDTO.AgencyId, RequestDTO.DocumentTypeCode, RequestDTO.TradeTranTypeID);
                }

                if (extHsCodeList == null)
                {
                    var errorMessage = String.Format("No HsCode found in Db for AgencyID : '{0}', RequiredDocumentTypeCode : '{1}', TradeTranTypeId : '{2}'", RequestDTO.AgencyId, RequestDTO.DocumentTypeCode, RequestDTO.TradeTranTypeID);

                    return BadRequestReply(errorMessage);
                }

                MongoDbRecordFetcher mongoDBRecordFetcher;

                IMongoCollection<BsonDocument> documentInCollection;

                try
                {
                    mongoDBRecordFetcher = new MongoDbRecordFetcher("TARP", mongoDbCollection, Environment.GetEnvironmentVariable("MONGODBConnString"));
                }
                catch (SystemException ex)
                {
                    Log.Error("|{0}|{1}| Error occured in connecting to MongoDB {@ex}", StrategyName, MethodID, ex.ToString());

                    return BadRequestReply("Error occured in connecting to MongoDB");
                }

                try
                {
                    documentInCollection = mongoDBRecordFetcher.GetCollection();
                }
                catch (SystemException ex)
                {
                    Log.Error("|{0}|{1}| Error occured in fetching record from MongoDB {@ex}", StrategyName, MethodID, ex.ToString());

                    return BadRequestReply("Error occured in fetching record from MongoDB");
                }

                var regulatedHsCodePurposeList = GetPurposeListAgainstRegulatedHsCode(extHsCodeList, documentInCollection, RequestDTO.AgencyId);

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

        private static List<RegulatedHsCodePurpose> GetPurposeListAgainstRegulatedHsCode(List<string> extHsCodeList, IMongoCollection<BsonDocument> documentInCollection, int agencyId)
        {
            var regulatedHsCodePurposes = new List<RegulatedHsCodePurpose>();

            foreach (var hsCode in extHsCodeList)
            {
                var hsCodePurpose = new RegulatedHsCodePurpose();
                hsCodePurpose.HsCode = hsCode;

                var filter = Builders<BsonDocument>.Filter.Eq("12 DIGIT PRODUCT CODE", hsCode);
                var projection = Builders<BsonDocument>.Projection.Include("PURPOSE").Exclude("_id");

                if (agencyId != 11)
                {
                    hsCodePurpose.PurposeList = documentInCollection.Find<BsonDocument>(filter).Project(projection).ToList().Select(x => x.GetValue("PURPOSE").ToString()).ToList();
                }

                regulatedHsCodePurposes.Add(hsCodePurpose);

            }

            return regulatedHsCodePurposes;
        }
    }
}
