using MongoDB.Bson;
using psw.security.Encryption;
using PSW.ITMS.Common.Enums;
using PSW.ITMS.Common.Model;
using PSW.ITMS.Data.Entities;
using PSW.ITMS.service;
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
    public class GetCountryListStrategy : ApiStrategy<GetCountryListRequest, GetCountryListResponse>
    {
        #region Constructors 
        public GetCountryListStrategy(CommandRequest request) : base(request)
        {
            Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~GetCountryListStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  

        public override CommandReply Execute()
        {
            try
            {


                Log.Information("|{0}|{1}| Request DTO {@RequestDTO}", StrategyName, MethodID, RequestDTO);

                var regulatedHSCode = this.Command.UnitOfWork.RegulatedHSCodeRepository.GetActiveHsCode(
                         RequestDTO.AgencyId.ToString(),
                         RequestDTO.TradeTranTypeID,
                         RequestDTO.documentTypeCode
                     );
                var mongoDoc = new BsonDocument();
                MongoDbRecordFetcher mongoDBRecordFetcher;

                mongoDBRecordFetcher = new MongoDbRecordFetcher("TARP", regulatedHSCode.CollectionName, Environment.GetEnvironmentVariable("MONGODBConnString"));

                try
                {
                    //here is my change
                    if (mongoDBRecordFetcher != null)
                    {
                        mongoDoc = mongoDBRecordFetcher.GetFilteredRecordMFD(regulatedHSCode.HSCodeExt);
                        if (mongoDoc == null)
                        {
                            return BadRequestReply(String.Format("No record found for HsCode : {0}", RequestDTO.HsCode));
                        }
                    }

                }
                catch (SystemException ex)
                {
                    Log.Error("|{0}|{1}| Error occured in fetching record from MongoDB {@ex}", StrategyName, MethodID, ex.ToString());

                    return BadRequestReply("Error occured in fetching record from MongoDB");
                }



                Log.Information("|{0}|{1}| Mongo Record fetched {@mongoDoc}", StrategyName, MethodID, mongoDoc);


                var docType = this.Command.UnitOfWork.DocumentTypeRepository.Where(new
                { Code = RequestDTO.documentTypeCode }).FirstOrDefault();

                var response = GetRequirements(mongoDoc, docType.DocumentClassificationCode);
                Log.Information("|{0}|{1}| Required LPCO Parent Code {@documentClassification}", StrategyName, MethodID, docType.DocumentClassificationCode);

                ResponseDTO = response;


                Log.Information("|{0}|{1}| LPCO required {2}", StrategyName, MethodID, "true");

                Log.Information("|{0}|{1}| Response {@ResponseDTO}", StrategyName, MethodID, ResponseDTO);


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


        public GetCountryListResponse GetRequirements(BsonDocument mongoRecord, string documentClassification)
        {
            Log.Information("[{0}.{1}] Started", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Log.Information("|{0}|{1}| documentClassification {documentClassification}", StrategyName, MethodID, documentClassification);
            GetCountryListResponse response = new GetCountryListResponse();
            List<string> countryList = new List<string>();
            Log.Information("|{0}|{1}| documentClassification {documentClassification}", StrategyName, MethodID, documentClassification);
            var countries = new List<string>();
            string ECFeeString = mongoRecord["Certificate of Quality and Origin Processing Fee (PKR)"].ToString();
            Log.Information("|{0}|{1}| ECFeeString {ECFeeString}", StrategyName, MethodID, ECFeeString);



            // The column that tells if Health Certificate is Fee Required (Conditional)
            // Condition: If the destination country is from one of the countries in the following column, then fee is applied.
            // "Names of Countries Requiring Health Certificate on prescribed format"
            countries = mongoRecord["Codes of Countries Requiring Health Certificate on prescribed format"].ToString().Split('|').ToList();
            if (countries.Count > 0)
            {
                countries = countries.Select(t => t.Trim()).ToList();
                countries = countries.Select(t => t.Replace("\n", "").Replace("\r", "")).ToList();
            }
            Log.Information("|{0}|{1}| countries {@countries}", StrategyName, MethodID, countries);
          //  Log.Information("|{0}|{1}| RequestDTO.DestinationCountryCode {RequestDTO.DestinationCountryCode}", StrategyName, MethodID, RequestDTO.DestinationCountryCode);

            foreach (var country in countries)
            {
                countryList.Add(country);
              

            }
            response.CountryList = countryList;
            Log.Information("[{0}.{1}] Ended", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return response;
        }
    }
}
