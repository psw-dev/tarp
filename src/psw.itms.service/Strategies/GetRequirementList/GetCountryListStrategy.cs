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

                RegulatedHSCode tempHsCode = Command.UnitOfWork.RegulatedHSCodeRepository.GetActiveHsCode(RequestDTO.HsCode, RequestDTO.AgencyId, RequestDTO.TradeTranTypeID, RequestDTO.documentTypeCode);

                // Log.Information("|{0}|{1}| RegulatedHSCode DbRecord {@tempHsCode}", StrategyName, MethodID, tempHsCode);

                // if (tempHsCode == null)
                // {
                //     return BadRequestReply("Record for hscode does not exist");
                // }

                // var tempRule = Command.UnitOfWork.RuleRepository.Get(Convert.ToInt16(tempHsCode.RuleID));

                // if (tempRule == null)
                // {
                //     return BadRequestReply("Record for rule against hscode does not exist");
                // }


                var mongoDoc = new BsonDocument();
                MongoDbRecordFetcher mongoDBRecordFetcher;

                // try
                // {
                    mongoDBRecordFetcher = new MongoDbRecordFetcher("TARP", tempHsCode.CollectionName, Environment.GetEnvironmentVariable("MONGODBConnString"));
              //  }
                // catch (SystemException ex)
                // {
                //     Log.Error("|{0}|{1}| Error occured in connecting to MongoDB {@ex}", StrategyName, MethodID, ex.ToString());

                //     return BadRequestReply("Error occured in connecting to MongoDB");
                // }

                try
                {
                   //here is my change
                   if(mongoDBRecordFetcher!=null)
                       { mongoDoc = mongoDBRecordFetcher.GetFilteredRecordMFD(RequestDTO.HsCode);
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
