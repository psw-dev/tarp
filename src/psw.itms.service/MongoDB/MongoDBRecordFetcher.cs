using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace PSW.ITMS.Service.MongoDB
{
    public class MongoDbRecordFetcher
    {
        public string DbName { get; set; }
        public string CollectionName { get; set; }
        public MongoClient MClient { get; set; }

        public MongoDbRecordFetcher(string dbName, string collectionName, string mongoDBConnectionString)
        {
            DbName = dbName;
            CollectionName = collectionName;
            //this.MClient = new MongoClient("mongodb://localhost");  //localhost
            MClient = new MongoClient(mongoDBConnectionString);
        }

        public BsonDocument GetFilteredRecord(string hscode, string purpose)
        {
            var database = MClient.GetDatabase(DbName);

            var collection = database.GetCollection<BsonDocument>(CollectionName);

            var hsCodeFilter = Builders<BsonDocument>.Filter.Eq("12 DIGIT PRODUCT CODE", hscode);
            var purposeFilter = Builders<BsonDocument>.Filter.Eq("PURPOSE", purpose);

            var combinedFilter = Builders<BsonDocument>.Filter.And(hsCodeFilter, purposeFilter);

            Collation collation = new Collation("en", caseLevel: false, strength: CollationStrength.Secondary);

            var FetchedRecord = collection.Find(combinedFilter, new FindOptions { Collation = collation }).FirstOrDefault();

            if (FetchedRecord == null)
            {
                return null;
            }

            return FetchedRecord;
        }

        public bool UpdateRecord(string hscode, string purpose, string propertyToBeUpdated, string updatedValue)
        {
            var recordFetch = GetFilteredRecord(hscode, purpose);

            if (recordFetch != null)
            {
                var database = MClient.GetDatabase(DbName);
                var collection = database.GetCollection<BsonDocument>(CollectionName);

                var update = Builders<BsonDocument>.Update.Set(propertyToBeUpdated, updatedValue);

                if (collection.UpdateOne(recordFetch, update) != null)
                {
                    return true;
                }
            }
            return false;
        }

        public IMongoCollection<BsonDocument> GetCollection()
        {
            var database = MClient.GetDatabase(DbName);
            var collection = database.GetCollection<BsonDocument>(CollectionName);

            return collection;
        }

        public bool CheckIfLPCORequired(BsonDocument mongoRecord, string requiredDocumentParentCode, out bool IsParenCodeValid)
        {
            switch (requiredDocumentParentCode)
            {
                case "IMP":
                    IsParenCodeValid = true;
                    return mongoRecord["IP REQUIRED"].ToString().ToLower() == "yes";          

                case "RO":
                    IsParenCodeValid = true;
                    return mongoRecord["RO REQUIRED"].ToString().ToLower() == "yes";

                case "EC":
                    IsParenCodeValid = true;
                    return mongoRecord["PHYTOSANITARY CERTIFICATION REQUIRED (Y /N)"].ToString().ToLower() == "yes";
                
                default:
                    IsParenCodeValid = false;
                    return false;
            }
        }

        public string GetFormNumber(BsonDocument mongoRecord, string requiredDocumentParentCode)
        {
            switch (requiredDocumentParentCode)
            {
                case "IMP":
                    return mongoRecord["IP CERTIFICATE FORM NUMBER"].ToString();

                case "RO":
                    return mongoRecord["RO CERTIFICATE FORM NUMBER"].ToString();
                    
                case "EC":
                    return mongoRecord["PHYTOSANTARY CERTIFICATE FORM NUMBER"].ToString();
            }
            return "";
        }
    }
}