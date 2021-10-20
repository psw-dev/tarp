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

            var hsCodeFilter = Builders<BsonDocument>.Filter.Eq("STATISTICAL SUFFIX", hscode);
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

        public bool CheckIfLPCORequired(BsonDocument mongoRecord, string requiredDocumentParentCode)
        {
            switch (requiredDocumentParentCode)
            {
                case "IMP":
                    if (mongoRecord["IP REQUIRED"].ToString().ToLower() == "yes")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case "RO":
                    if (mongoRecord["RO REQUIRED"].ToString().ToLower() == "yes")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case "EC":
                    if (mongoRecord["PHYTOSANITARY CERTIFICATION REQUIRED (Y /N)"].ToString().ToLower() == "yes")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
            return false;
        }
    }
}