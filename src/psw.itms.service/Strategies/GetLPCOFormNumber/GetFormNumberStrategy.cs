using MongoDB.Bson;
using psw.security.Encryption;
using PSW.ITMS.Data.Entities;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.MongoDB;
using PSW.Lib.Logs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSW.ITMS.Service.Strategies
{
    public class GetFormNumberStrategy : ApiStrategy<GetFormNumberRequestDTO, GetFormNumberResponseDTO>
    {
        #region Constructors 
        public GetFormNumberStrategy(CommandRequest request) : base(request)
        {
            Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~GetFormNumberStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  

        public override CommandReply Execute()
        {
            try
            {
                if (string.IsNullOrEmpty(RequestDTO.HsCode) && string.IsNullOrEmpty(RequestDTO.documentTypeCode) && string.IsNullOrEmpty(RequestDTO.TradePurpose))
                {
                    return BadRequestReply("Please provide valid request parameters");
                }

                var hsCode = Command.UnitOfWork.RegulatedHSCodeRepository.GetActiveHsCode(
                    RequestDTO.HsCode,
                    RequestDTO.AgencyId.ToString(),
                    RequestDTO.TradeTranTypeID,
                    RequestDTO.documentTypeCode
                );
                var mongoDoc = new BsonDocument();

                MongoDbRecordFetcher mongoDBRecordFetcher;

                try
                {
                    mongoDBRecordFetcher = new MongoDbRecordFetcher("TARP", hsCode.CollectionName, Environment.GetEnvironmentVariable("MONGODBConnString"));
                }
                catch (SystemException ex)
                {
                    Log.Error("|{0}|{1}| Error occured in connecting to MongoDB {@ex}", StrategyName, MethodID, ex.ToString());

                    return BadRequestReply("Error occured in connecting to MongoDB");
                }

                try
                {
                    mongoDoc = mongoDBRecordFetcher.GetFilteredRecord(RequestDTO.HsCode, RequestDTO.TradePurpose);
                }
                catch (SystemException ex)
                {
                    Log.Error("|{0}|{1}| Error occured in fetching record from MongoDB {@ex}", StrategyName, MethodID, ex.ToString());

                    return BadRequestReply("Error occured in fetching record from MongoDB");
                }

                if (mongoDoc == null)
                {
                    return BadRequestReply(String.Format("No record found for HsCode : {0}  Purpose : {1}",RequestDTO.HsCode, RequestDTO.TradePurpose));
                }

                Log.Information("|{0}|{1}| Mongo Record fetched {@mongoDoc}", StrategyName, MethodID, mongoDoc);

                var docType = this.Command.UnitOfWork.DocumentTypeRepository.Where(new
                { Code = RequestDTO.documentTypeCode }).FirstOrDefault();

                Log.Information("|{0}|{1}| Required LPCO Parent Code {@documentClassification}", StrategyName, MethodID, docType.DocumentClassificationCode);
                string formNumber = string.Empty;
                if (RequestDTO.AgencyId == 2)
                {
                    formNumber = mongoDBRecordFetcher.GetFormNumber(mongoDoc, docType.DocumentClassificationCode);
                }
                else if (RequestDTO.AgencyId == 3)
                {
                    formNumber = mongoDBRecordFetcher.GetFormNumberAQD(mongoDoc, docType.DocumentClassificationCode);
                }
                else if (RequestDTO.AgencyId == 4)
                {
                    formNumber = mongoDBRecordFetcher.GetFormNumberFSCRD(mongoDoc, docType.DocumentClassificationCode);
                }

                if (string.IsNullOrEmpty(formNumber))
                {
                    return BadRequestReply("Form number not found in record");
                }
                Log.Information("|{0}|{1}| Required Form number {@formNumber}", StrategyName, MethodID, formNumber);

                ResponseDTO = new GetFormNumberResponseDTO
                {
                    FormNumber = formNumber
                };

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
    }
}
