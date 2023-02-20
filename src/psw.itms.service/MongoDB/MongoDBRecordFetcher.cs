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

        public BsonDocument GetFilteredRecordAQD(string hscode, string purpose)
        {
            var database = MClient.GetDatabase(DbName);

            var collection = database.GetCollection<BsonDocument>(CollectionName);

            var hsCodeFilter = Builders<BsonDocument>.Filter.Eq("12 DIGIT PRODUCT CODE", hscode);
            var purposeFilter = Builders<BsonDocument>.Filter.Eq("CATEGORY", purpose);

            var combinedFilter = Builders<BsonDocument>.Filter.And(hsCodeFilter, purposeFilter);

            Collation collation = new Collation("en", caseLevel: false, strength: CollationStrength.Secondary);

            var FetchedRecord = collection.Find(combinedFilter, new FindOptions { Collation = collation }).FirstOrDefault();

            if (FetchedRecord == null)
            {
                return null;
            }

            return FetchedRecord;
        }

        public BsonDocument GetFilteredRecordFSCRD(string hscode, string purpose)
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

        public BsonDocument GetFilteredRecordPSQCA(string hscode)
        {
            var database = MClient.GetDatabase(DbName);

            var collection = database.GetCollection<BsonDocument>(CollectionName);

            var hsCodeFilter = Builders<BsonDocument>.Filter.Eq("12 DIGIT PRODUCT CODE", hscode);

            Collation collation = new Collation("en", caseLevel: false, strength: CollationStrength.Secondary);

            var FetchedRecord = collection.Find(hsCodeFilter, new FindOptions { Collation = collation }).FirstOrDefault();

            if (FetchedRecord == null)
            {
                return null;
            }

            return FetchedRecord;
        }
        public BsonDocument GetFilteredRecordMFD(string hscode)
        {
            var database = MClient.GetDatabase(DbName);

            var collection = database.GetCollection<BsonDocument>(CollectionName);

            var hsCodeFilter = Builders<BsonDocument>.Filter.Eq("12 DIGIT PRODUCT CODE", hscode);

            Collation collation = new Collation("en", caseLevel: false, strength: CollationStrength.Secondary);

            var FetchedRecord = collection.Find(hsCodeFilter, new FindOptions { Collation = collation }).FirstOrDefault();

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

                case "PRR":
                    IsParenCodeValid = true;
                    return mongoRecord["REGISTRATION REQUIRED (YES/NO)"].ToString().ToLower() == "yes";

                default:
                    IsParenCodeValid = false;
                    return false;
            }
        }

        public bool CheckIfLPCORequiredAQD(BsonDocument mongoRecord, string requiredDocumentParentCode, out bool IsParenCodeValid)
        {
            switch (requiredDocumentParentCode)
            {
                case "IMP":
                    IsParenCodeValid = true;
                    return mongoRecord["IP REQUIRED"].ToString().ToLower() == "yes";

                case "RO":
                    IsParenCodeValid = true;
                    return mongoRecord["RELEASE ORDER"].ToString().ToLower() == "yes";

                case "EC":
                    IsParenCodeValid = true;
                    return mongoRecord["Health Certificate"].ToString().ToLower() == "yes";

                case "PRR":
                    IsParenCodeValid = true;
                    return mongoRecord["REGISTRATION REQUIRED (YES/NO)"].ToString().ToLower() == "yes";

                default:
                    IsParenCodeValid = false;
                    return false;
            }
        }

        public bool CheckIfLPCORequiredFSCRD(BsonDocument mongoRecord, string requiredDocumentParentCode, out bool IsParenCodeValid)
        {
            switch (requiredDocumentParentCode)
            {
                case "PRD":
                    IsParenCodeValid = true;
                    return mongoRecord["ENLISTMENT OF SEED VARIETY REQUIRED (Yes/No)"].ToString().ToLower() == "yes";

                case "RO":
                    IsParenCodeValid = true;
                    return mongoRecord["RELEASE ORDER REQUIRED (Yes/No)"].ToString().ToLower() == "yes";

                default:
                    IsParenCodeValid = false;
                    return false;
            }
        }

        public bool CheckIfLPCORequiredPSQCA(BsonDocument mongoRecord, string requiredDocumentParentCode, out bool IsParenCodeValid)
        {
            switch (requiredDocumentParentCode)
            {
                case "RO":
                    IsParenCodeValid = true;
                    return mongoRecord["RELEASE ORDER REQUIRED (Yes/No)"].ToString().ToLower() == "yes";

                default:
                    IsParenCodeValid = false;
                    return false;
            }
        }
        public bool CheckIfLPCORequiredMFD(BsonDocument mongoRecord, string requiredDocumentParentCode, out bool IsParenCodeValid)
        {
            switch (requiredDocumentParentCode)
            {
                case "IMP":
                    IsParenCodeValid = true;
                    return mongoRecord["IP REQUIRED"].ToString().ToLower() == "yes";

                case "RO":
                    IsParenCodeValid = true;
                    return mongoRecord["RELEASE ORDER"].ToString().ToLower() == "yes";

                case "EC":
                    IsParenCodeValid = true;
                    return mongoRecord["Is Certificate of Quality and Origin Required (Yes/No)"].ToString().ToLower() == "yes";

                case "PRR":
                    IsParenCodeValid = true;
                    return mongoRecord["REGISTRATION REQUIRED (YES/NO)"].ToString().ToLower() == "yes";
                    
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

        public string GetFormNumberAQD(BsonDocument mongoRecord, string requiredDocumentParentCode)
        {
            switch (requiredDocumentParentCode)
            {
                case "IMP":
                    return mongoRecord["IP CERTIFICATE FORM NUMBER"].ToString();

                case "RO":
                    return mongoRecord["RELEASE ORDER FORM NUMBER"].ToString();

                case "EC":
                    return mongoRecord["Form"].ToString();
            }
            return "";
        }

        public string GetFormNumberFSCRD(BsonDocument mongoRecord, string requiredDocumentParentCode)
        {
            switch (requiredDocumentParentCode)
            {
                case "RO":
                    return mongoRecord["RO Certificate Number"].ToString();
            }
            return "";
        }
        public string GetFormNumberMFD(BsonDocument mongoRecord, string requiredDocumentParentCode)
        {
            switch (requiredDocumentParentCode)
            {
                case "EC":
                    return mongoRecord["Certificate of Quality and Origin Print Form Number"].ToString();
            }
            return "";
        }

        public bool GetPSIInfo(BsonDocument mongoRecord, string hscode)
        {
            var database = MClient.GetDatabase(DbName);

            var collection = database.GetCollection<BsonDocument>(CollectionName);

            var hsCodeFilter = Builders<BsonDocument>.Filter.Eq("FINAL PCT CODE", hscode);
            var psiFilter = Builders<BsonDocument>.Filter.Eq("PSI REQUIRED (YES/NO)", "yes");
            var combinedFilter = Builders<BsonDocument>.Filter.And(hsCodeFilter, psiFilter);

            Collation collation = new Collation("en", caseLevel: false, strength: CollationStrength.Secondary);

            var FetchedRecord = collection.Find(combinedFilter, new FindOptions { Collation = collation }).FirstOrDefault();

            return FetchedRecord["PSI REQUIRED (YES/NO)"].ToString().ToLower() == "yes";

        }
    }
}