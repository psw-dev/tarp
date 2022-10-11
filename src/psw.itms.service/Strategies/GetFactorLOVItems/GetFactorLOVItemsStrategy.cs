using MongoDB.Bson;
using MongoDB.Driver;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.MongoDB;
using PSW.Lib.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PSW.ITMS.Service.Strategies
{
    public class GetFactorLOVItemsStrategy : ApiStrategy<GetFactorLovItemsRequest, GetFactorLovItemsResponse>
    {
        #region Constructors 
        public GetFactorLOVItemsStrategy(CommandRequest request) : base(request)
        {
            Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~GetFactorLOVItemsStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  

        public override CommandReply Execute()
        {
            try
            {
                Log.Information("|{0}|{1}| Request DTO {@RequestDTO}", StrategyName, MethodID, RequestDTO);
                // USE MODEL VALIDATOR INSTEAD OF BELOW CHECK
                if (RequestDTO.AgencyId == 0 ||
                    string.IsNullOrEmpty(RequestDTO.DocumentTypeCode) ||
                    RequestDTO.TradeTranTypeID == 0 ||
                    RequestDTO.FactorList == null ||
                    RequestDTO.FactorList.Count == 0 ||
                    string.IsNullOrEmpty(RequestDTO.HSCodeExt))
                {
                    return BadRequestReply("Please provide valid request parameters");
                }

                var mongoDbCollection = Command.UnitOfWork.RegulatedHSCodeRepository.GetActiveHsCode(
                    RequestDTO.HSCodeExt.ToString(),
                    RequestDTO.AgencyId.ToString(),
                    RequestDTO.TradeTranTypeID,
                    RequestDTO.DocumentTypeCode
                ).CollectionName;

                if (string.IsNullOrEmpty(mongoDbCollection))
                {
                    Log.Error("[{0}.{1}] No record found for provided request parameters", GetType().Name, MethodBase.GetCurrentMethod().Name, ResponseDTO);
                    return BadRequestReply("No record found for provided request parameters");
                }

                Log.Information("|{0}|{1}| MongoDb Collection Name : {@mongoDbCollection}", StrategyName, MethodID, mongoDbCollection);

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

                var tempFactorLOVItems = GetLOVItemsForProvidedFactors(documentInCollection);

                if (tempFactorLOVItems == null || tempFactorLOVItems.Count == 0)
                {
                    ResponseDTO = new GetFactorLovItemsResponse
                    {
                        FactorLOVItemsList = new List<FactorLOVItemsData>()
                    };
                    return OKReply("LOV data not available for provided factor list");
                }

                ResponseDTO = new GetFactorLovItemsResponse
                {
                    FactorLOVItemsList = tempFactorLOVItems
                };

                Log.Information("[{0}.{1}] Respose: {@ResponseDTO}", GetType().Name, MethodBase.GetCurrentMethod().Name, ResponseDTO);
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

        public List<FactorLOVItemsData> GetLOVItemsForProvidedFactors(IMongoCollection<BsonDocument> documentInCollection)
        {
            Log.Information("[{0}.{1}] Started", GetType().Name, MethodBase.GetCurrentMethod().Name);
            var tempFactorDatalist = new List<FactorLOVItemsData>();

            foreach (var factorInfo in RequestDTO.FactorList)
            {
                var tempFactorData = new FactorLOVItemsData();
                tempFactorData.FactorID = factorInfo.FactorId;
                Log.Information("[{0}.{1}] FactorInfo : {@factorInfo}", GetType().Name, MethodBase.GetCurrentMethod().Name, factorInfo);
                var factorData = Command.UnitOfWork.FactorRepository?.Where(new { ID = factorInfo.FactorId, ISLOV = 1 }).FirstOrDefault();

                if (factorData == null)
                {
                    Log.Information("[{0}.{1}] Factor not exists", GetType().Name, MethodBase.GetCurrentMethod().Name);
                    continue;
                }
                else
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("12 DIGIT PRODUCT CODE", RequestDTO.HSCodeExt);
                    var projection = Builders<BsonDocument>.Projection.Include(factorData.FactorCode).Exclude("_id");
                    var lov = documentInCollection.Find<BsonDocument>(filter).Project(projection).ToList().Select(x => x.GetValue(factorData.FactorCode).ToString()).ToList();

                    if (factorData.FactorCode == "UNIT")
                    {
                        lov = lov.FirstOrDefault().Split('|').ToList();

                    }
                    Log.Information("[{0}.{1}] LOV : {@lov}", GetType().Name, MethodBase.GetCurrentMethod().Name, lov);
                    var factorLOVItems = Command.UnitOfWork.LOVItemRepository.GetLOVItems(factorInfo.LOVTableName, factorInfo.LOVColumnName);

                    tempFactorData.FactorLabel = factorData.Label;
                    tempFactorData.FactorCode = factorData.FactorCode;
                    tempFactorData.FactorLOVItems = factorLOVItems.Where(x => lov.ConvertAll(y => y.ToLower()).Contains(x.ItemValue.ToLower())).ToList();

                    if (tempFactorData.FactorLOVItems != null || tempFactorData.FactorLOVItems.Count == 0)
                    {
                        tempFactorDatalist.Add(tempFactorData);
                    }
                }
            }
            Log.Information("[{0}.{1}] TempFactorDatalist: {@tempFactorDatalist}", GetType().Name, MethodBase.GetCurrentMethod().Name, tempFactorDatalist);
            Log.Information("[{0}.{1}] Ended", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return tempFactorDatalist;
        }
    }
}
