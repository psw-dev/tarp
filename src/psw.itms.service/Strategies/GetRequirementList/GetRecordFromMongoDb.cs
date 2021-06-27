using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.Json;

namespace PSW.ITMS.Service.MongoDB
{
    public class MongoDbRecordFetcher
    {
        public string DbName { get; set; }
        public string CollectionName { get; set; }
        public MongoClient MClient { get; set; }

        public MongoDbRecordFetcher (string dbName, string collectionName)
        {
            this.DbName = dbName;
            this.CollectionName = collectionName;
            this.MClient = new MongoClient("mongodb://localhost");
        }

        public BsonDocument GetFilteredRecord (string hscode, string purpose)
        {
            var database = MClient.GetDatabase(DbName);

            var collection = database.GetCollection<BsonDocument>(CollectionName);

            var hsCodeFilter = Builders<BsonDocument>.Filter.Eq("STATISTICAL SUFFIX", hscode);
            var purposeFilter = Builders<BsonDocument>.Filter.Eq("PURPOSE", purpose);

            var combinedFilter = Builders<BsonDocument>.Filter.And(hsCodeFilter, purposeFilter);

            BsonDocument FetchedRecord = collection.Find(combinedFilter).FirstOrDefault();

            return FetchedRecord;
        }
    }
}