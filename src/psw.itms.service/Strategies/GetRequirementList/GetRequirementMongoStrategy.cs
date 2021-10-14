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
    public class GetRequirementMongoStrategy : ApiStrategy<GetDocumentRequirementRequest, GetDocumentRequirementResponse>
    {
        #region Constructors 
        public GetRequirementMongoStrategy(CommandRequest request) : base(request)
        {
            Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~GetRequirementMongoStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  

        public override CommandReply Execute()
        {
            try
            {
                if (RequestDTO.FactorCodeValuePair == null || RequestDTO.FactorCodeValuePair.Count == 0)
                {
                    return BadRequestReply("Please provide valid request parameters");
                }

                Log.Information("|{0}|{1}| Request DTO {@RequestDTO}", StrategyName, MethodID, RequestDTO);

                var tempHsCode = Command.UnitOfWork.RegulatedHSCodeRepository.Where(
                    new
                    {
                        HSCodeExt = RequestDTO.HsCode,
                        AgencyID = RequestDTO.AgencyId,
                        RequiredDocumentTypeCode = RequestDTO.documentTypeCode,
                        TradeTranTypeID = RequestDTO.TradeTranTypeID
                    }
                    ).FirstOrDefault();

                Log.Information("|{0}|{1}| RegulatedHSCode DbRecord {@tempHsCode}", StrategyName, MethodID, tempHsCode);

                if (tempHsCode == null)
                {
                    return BadRequestReply("Record for hscode does not exist");
                }

                var tempRule = Command.UnitOfWork.RuleRepository.Get(Convert.ToInt16(tempHsCode.RuleID));

                if (tempRule == null)
                {
                    return BadRequestReply("Record for rule against hscode does not exist");
                }

                Log.Information("|{0}|{1}| Rule DbRecord {@tempRule}", StrategyName, MethodID, tempRule);

                //var FactorsIDAppliedTORule = GetFactorAppliedInRule(TempRule);
                var factorsIDAppliedTORule = tempRule.GetFactorAppliedInRule();

                if (factorsIDAppliedTORule.Count == 0)
                {
                    return BadRequestReply("No factor found in rule");
                }

                Log.Information("|{0}|{1}| FactorID's Applied To Rule DbRecord {@factorsIDAppliedTORule}", StrategyName, MethodID, factorsIDAppliedTORule);

                var factorDataList = Command.UnitOfWork.FactorRepository.GetFactorsData(factorsIDAppliedTORule);

                if (factorDataList == null)
                {
                    return BadRequestReply("Factors data not found");
                }

                Log.Information("|{0}|{1}| FactorData DbRecord {@factorDataList}", StrategyName, MethodID, factorDataList);

                var mongoDoc = new BsonDocument();

                MongoDbRecordFetcher mongoDBRecordFetcher;

                try
                {
                    mongoDBRecordFetcher = new MongoDbRecordFetcher("TARP", tempHsCode.CollectionName, Environment.GetEnvironmentVariable("MONGODBConnString"));
                }
                catch (SystemException ex)
                {
                    Log.Error("|{0}|{1}| Error occured in connecting to MongoDB {@ex}", StrategyName, MethodID, ex.ToString());

                    return BadRequestReply("Error occured in connecting to MongoDB");
                }

                try
                {
                    mongoDoc = mongoDBRecordFetcher.GetFilteredRecord(RequestDTO.HsCode, RequestDTO.FactorCodeValuePair["PURPOSE"].FactorValue);
                }
                catch (SystemException ex)
                {
                    Log.Error("|{0}|{1}| Error occured in fetching record from MongoDB {@ex}", StrategyName, MethodID, ex.ToString());

                    return BadRequestReply("Error occured in fetching record from MongoDB");
                }

                Log.Information("|{0}|{1}| Mongo Record fetched {@mongoDoc}", StrategyName, MethodID, mongoDoc);

                if (mongoDoc == null)
                {
                    return BadRequestReply("No record found for HsCode : " + RequestDTO.HsCode);
                }

                var recordChecker = CheckFactorInMongoRecord(factorDataList, mongoDoc, RequestDTO.FactorCodeValuePair);

                var tempDocumentaryRequirementList = new List<DocumentaryRequirement>();

                if (recordChecker == "Checked")
                {
                    ResponseDTO = GetRequirements(mongoDoc, RequestDTO.documentTypeCode);
                    
                    Log.Information("|{0}|{1}| Documentary Requirements {@tempDocumentaryRequirementList}", StrategyName, MethodID, tempDocumentaryRequirementList);
                }
                else
                {
                    return BadRequestReply(recordChecker);
                }

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

        public string CheckFactorInMongoRecord(List<Factors> factorDataList, BsonDocument mongoDoc, Dictionary<string, FactorData> factorCodeValuePair)
        {
            var count = 0;
            foreach (var factor in factorDataList)
            {
                if (factorCodeValuePair.ContainsKey(factor.FactorCode))
                {
                    var ItemList = mongoDoc[factor.FactorCode].ToString().Split('|').ToList();

                    if (ItemList.Contains(factorCodeValuePair[factor.FactorCode].FactorValue) || ItemList.Contains("ALL"))
                    {
                        count += 1;
                    }
                    else
                    {
                        return $"Factor: {factor.Label} Value : {factorCodeValuePair[factor.FactorCode]} does not match in record";
                    }
                }
                else
                {
                    return $"Factors provided does not contain FactorData that is required in Rule {factor.FactorCode} not provided";
                }
            }

            if (count == factorDataList.Count)
            {
                return "Checked";
            }
            else
            {
                return " ";
            }
        }

        public GetDocumentRequirementResponse GetRequirements(BsonDocument mongoRecord, string requiredDocumentTypeCode)
        {
            GetDocumentRequirementResponse tarpRequirments = new GetDocumentRequirementResponse();

            var tarpDocumentRequirements = new List<DocumentaryRequirement>();
            var FinancialRequirement = new FinancialRequirement();
            var ValidityRequirement = new ValidityRequirement();

            string documentClassification = this.Command.UnitOfWork.DocumentTypeRepository.Where(new
            { Code = requiredDocumentTypeCode }).FirstOrDefault().DocumentClassificationCode;

            //for Import Permit = IMP
            if (documentClassification == "IMP")
            {
                var ipDocRequirements = mongoRecord["IP DOCUMENTARY REQUIREMENTS"].ToString().Split('|').ToList();
                var ipDocRequirementsTrimmed = new List<string>();

                foreach (var lpco in ipDocRequirements)
                {
                    ipDocRequirementsTrimmed.Add(lpco.Trim());
                }

                foreach (var doc in ipDocRequirementsTrimmed)
                {
                    var tempReq = new DocumentaryRequirement();

                    tempReq.Name = doc + " For Import Permit"; //replace DPP with collectionName 
                    tempReq.DocumentName = doc;
                    tempReq.IsMandatory = true;
                    tempReq.RequirementType = "Documentary";

                    tempReq.DocumentTypeCode = Command.UnitOfWork.DocumentTypeRepository.Where(new { Name = doc }).FirstOrDefault()?.Code;
                    tempReq.AttachedObjectFormatID = 1;

                    tarpDocumentRequirements.Add(tempReq);
                }

                //Financial Requirements
                FinancialRequirement.PlainAmount = mongoRecord["IP FEES"].ToString();
                FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(mongoRecord["IP FEES"].ToString());


                //ValidityTerm Requirements
                ValidityRequirement.UomName = "Month";
                var uomPeriod = mongoRecord["IP VALIDITY"].ToString();
                ValidityRequirement.Quantity = Convert.ToInt32(uomPeriod.Substring(0, 2));

            }
            //for ReleaseOrder = RO
            else if (documentClassification == "RO")
            {
                var roDocRequirements = mongoRecord["RO  DOCUMENTARY REQUIREMENTS"].ToString().Split('|').ToList();

                var roDocRequirementsTrimmed = new List<string>();

                foreach (var lpco in roDocRequirements)
                {
                    var removespaces = lpco.Trim();
                    roDocRequirementsTrimmed.Add(removespaces.TrimEnd('\n'));
                }

                // roDocRequirementsTrimmed.Remove("Application on DPP prescribed form 20 [Rule 44(1) of PQR 2019]");
                // roDocRequirementsTrimmed.Remove("Fee Challan");

                //DocumentaryRequirements
                foreach (var doc in roDocRequirementsTrimmed)
                {
                    var tempReq = new DocumentaryRequirement();

                    tempReq.Name = doc + " For " + " DPP Release Order"; //replace DPP with collectionName 
                    tempReq.DocumentName = doc;
                    tempReq.IsMandatory = true;
                    tempReq.RequirementType = "Documentary";

                    tempReq.DocumentTypeCode = Command.UnitOfWork.DocumentTypeRepository.Where(new { Name = doc }).FirstOrDefault()?.Code;
                    tempReq.AttachedObjectFormatID = 1;

                    tarpDocumentRequirements.Add(tempReq);
                }

                //Financial Requirements
                FinancialRequirement.PlainAmount = mongoRecord["RO FEES"].ToString();
                FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(mongoRecord["RO FEES"].ToString());
            }

            //for PythoCertificate = EC
            else if (documentClassification == "EC")
            {
                var roDocRequirements = mongoRecord["PHYTOSANITARY  DOCUMENTARY REQUIREMENTS"].ToString().Split('|').ToList();

                var roDocRequirementsTrimmed = new List<string>();

                foreach (var lpco in roDocRequirements)
                {
                    var removeSpaces = lpco.Trim();
                    roDocRequirementsTrimmed.Add(removeSpaces.TrimEnd('\n'));
                }

                // roDocRequirementsTrimmed.Remove("Application on DPP prescribed form 20 [Rule 44(1) of PQR 2019]");
                // roDocRequirementsTrimmed.Remove("Fee Challan");

                //DocumentaryRequirements
                foreach (var doc in roDocRequirementsTrimmed)
                {
                    var tempReq = new DocumentaryRequirement();

                    tempReq.Name = doc + " For Phythosanitary Certificate";
                    tempReq.DocumentName = doc;
                    tempReq.IsMandatory = true;
                    tempReq.RequirementType = "Documentary";

                    tempReq.DocumentTypeCode = Command.UnitOfWork.DocumentTypeRepository.Where(new { Name = doc }).FirstOrDefault()?.Code;
                    tempReq.AttachedObjectFormatID = 1;

                    tarpDocumentRequirements.Add(tempReq);
                }

                //Financial Requirements
                FinancialRequirement.PlainAmount = mongoRecord["PHYTOSANITARY  FEES"].ToString();
                FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(mongoRecord["PHYTOSANITARY  FEES"].ToString());
            }

            tarpRequirments.DocumentaryRequirementList = tarpDocumentRequirements;
            tarpRequirments.FinancialRequirement = FinancialRequirement;
            tarpRequirments.ValidityRequirement = ValidityRequirement;

            return tarpRequirments;
        }
    }
}
