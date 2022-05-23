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
    public class GetHSCodeBasedRequirementStrategy : ApiStrategy<GetHSCodeBasedRequirementListRequest, GetHSCodeBasedRequirementResponse>
    {
        #region Constructors 
        public GetHSCodeBasedRequirementStrategy(CommandRequest request) : base(request)
        {
            Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~GetHSCodeBasedRequirementStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  

        public override CommandReply Execute()
        {
            try
            {
                if (RequestDTO.HsCodeFactorList == null || RequestDTO.HsCodeFactorList.Count == 0)
                {
                    return BadRequestReply("Please provide valid request parameters");
                }
                ResponseDTO = new GetHSCodeBasedRequirementResponse();

                Log.Information("|{0}|{1}| Request DTO {@RequestDTO}", StrategyName, MethodID, RequestDTO);

                var responseList = new List<GetHSCodeBasedRequirementList>();
                decimal totalPlainAmount = 0;
                foreach (HSCodeFactor hsCodeFactor in RequestDTO.HsCodeFactorList)
                {

                    var tempHsCode = Command.UnitOfWork.RegulatedHSCodeRepository.Where(
                        new
                        {
                            HSCodeExt = hsCodeFactor.HsCode,
                            AgencyID = RequestDTO.AgencyId,
                            RequiredDocumentTypeCode = RequestDTO.documentTypeCode,
                            TradeTranTypeID = RequestDTO.TradeTranTypeID
                        }
                        ).FirstOrDefault();

                    // List<RegulatedHSCode> tempHsCodeList = Command.UnitOfWork.RegulatedHSCodeRepository.GetActiveHsCodeList(RequestDTO.HsCodeList, RequestDTO.AgencyId, RequestDTO.TradeTranTypeID, RequestDTO.documentTypeCode);

                    Log.Information("|{0}|{1}| RegulatedHSCode DbRecord {@tempHsCode}", StrategyName, MethodID, tempHsCode);

                    if (tempHsCode == null)
                    {
                        continue;

                        // return BadRequestReply("Record for hscode does not exist");
                    }
                    // foreach(RegulatedHSCode tempHsCode in tempHsCodeList){
                    var tempRule = Command.UnitOfWork.RuleRepository.Get(Convert.ToInt16(tempHsCode.RuleID));

                    if (tempRule == null)
                    {
                        continue;
                        // return BadRequestReply("Record for rule against hscode does not exist");
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
                    // }
                    // var mongoDocList = new List<BsonDocument>();
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
                        mongoDoc = mongoDBRecordFetcher.GetFilteredRecord(hsCodeFactor.HsCode, hsCodeFactor.FactorCodeValuePair["PURPOSE"].FactorValue);

                        // mongoDocList = mongoDBRecordFetcher.GetFilteredRecord(RequestDTO.HsCodeList);
                    }
                    catch (SystemException ex)
                    {
                        Log.Error("|{0}|{1}| Error occured in fetching record from MongoDB {@ex}", StrategyName, MethodID, ex.ToString());

                        return BadRequestReply("Error occured in fetching record from MongoDB");
                    }

                    if (mongoDoc == null)
                    {
                        // return BadRequestReply(String.Format("No record found for HsCode : {0}", RequestDTO.HsCodeList));
                        //return BadRequestReply(String.Format("No record found for HsCode : {0}  Purpose : {1}", hsCodeFactor.HsCode, hsCodeFactor.FactorCodeValuePair["PURPOSE"].FactorValue));
                        continue;
                    }

                    Log.Information("|{0}|{1}| Mongo Record fetched {@mongoDoc}", StrategyName, MethodID, mongoDoc);


                    var docType = this.Command.UnitOfWork.DocumentTypeRepository.Where(new
                    { Code = RequestDTO.documentTypeCode }).FirstOrDefault();

                    Log.Information("|{0}|{1}| Required LPCO Parent Code {@documentClassification}", StrategyName, MethodID, docType.DocumentClassificationCode);

                    // ResponseDTO = new List<GetHSCodeBasedRequirementListResponse>();
                    // var responseList = new List<GetHSCodeBasedRequirementListResponse>();
                    // foreach (BsonDocument mongoDoc in mongoDocList)
                    // {
                    var hsCodeBasedReq = new GetHSCodeBasedRequirementList();
                    bool DocumentIsRequired = mongoDBRecordFetcher.CheckIfLPCORequired(mongoDoc, docType.DocumentClassificationCode, out bool IsParenCodeValid);

                    if (!IsParenCodeValid)
                    {
                        Log.Information("|{0}|{1}| Parent Code is Not Valid", StrategyName, MethodID);
                        continue;

                        // return BadRequestReply("Document does not belong to a supported document Classification");
                    }
                    else if (!DocumentIsRequired)
                    {
                        Log.Information("|{0}|{1}| LPCO required {2}", StrategyName, MethodID, "false");

                        hsCodeBasedReq.isLPCORequired = false;

                        // return OKReply(string.Format("{0} not required for HsCode : {1}", docType.Name, RequestDTO.HsCodeList));
                        // return OKReply(string.Format("{0} not required for HsCode : {1} and Purpose : {2}", docType.Name, RequestDTO.HsCode, RequestDTO.FactorCodeValuePair["PURPOSE"].FactorValue));


                    }
                    else if (DocumentIsRequired)
                    {
                        var tempDocumentaryRequirementList = new List<DocumentaryRequirement>();
                        var recordChecker = CheckFactorInMongoRecord(factorDataList, mongoDoc, hsCodeFactor.FactorCodeValuePair);

                        if (recordChecker == "Checked")
                        {
                            hsCodeBasedReq = GetRequirements(mongoDoc, docType.DocumentClassificationCode);

                            hsCodeBasedReq.FormNumber = mongoDBRecordFetcher.GetFormNumber(mongoDoc, docType.DocumentClassificationCode);

                            Log.Information("|{0}|{1}| Documentary Requirements {@tempDocumentaryRequirementList}", StrategyName, MethodID, tempDocumentaryRequirementList);
                        }
                        else
                        {
                            continue;
                            //return BadRequestReply(recordChecker);
                        }
                        // var hsCodeDocReq = responseList.Find(x => x.HSCodeExt == hsCodeBasedReq.HSCodeExt);
                        // if (hsCodeDocReq != null)
                        // {
                        //     hsCodeDocReq.DocumentaryRequirementList.Union(hsCodeBasedReq.DocumentaryRequirementList);
                        // }
                        // else
                        // {
                        responseList.Add(hsCodeBasedReq);
                        totalPlainAmount += Convert.ToInt32(hsCodeBasedReq.FinancialRequirement.PlainAmount);
                    }
                    Log.Information("|{0}|{1}| LPCO required {2}", StrategyName, MethodID, "true");

                }
                ResponseDTO.HSCodeBasedRequirementList = responseList;
                ResponseDTO.TotalFinancialRequirement = new FinancialRequirement();
                ResponseDTO.TotalFinancialRequirement.PlainAmount = totalPlainAmount.ToString();
                ResponseDTO.TotalFinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(ResponseDTO.TotalFinancialRequirement.PlainAmount.ToString());

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
                    var ItemList = mongoDoc[factor.FactorCode].ToString().ToLower().Split('|').ToList();

                    if (ItemList.Contains(factorCodeValuePair[factor.FactorCode].FactorValue.ToLower()) || ItemList.Contains("ALL"))
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

        public GetHSCodeBasedRequirementList GetRequirements(BsonDocument mongoRecord, string documentClassification)
        {
            GetHSCodeBasedRequirementList tarpRequirments = new GetHSCodeBasedRequirementList();

            var tarpDocumentRequirements = new List<DocumentaryRequirement>();
            var FinancialRequirement = new FinancialRequirement();
            var ValidityRequirement = new ValidityRequirement();

            tarpRequirments.isLPCORequired = true;
            tarpRequirments.HSCodeExt = mongoRecord["12 DIGIT PRODUCT CODE"].ToString();
            tarpRequirments.Purpose = mongoRecord["PURPOSE"].ToString();

            //for Import Permit = IMP
            if (documentClassification == "IMP")
            {
                var ipDocRequirements = mongoRecord["IP DOCUMENTARY REQUIREMENTS"].ToString().Split('|').ToList();
                var ipDocRequirementsTrimmed = new List<string>();

                var ipDocOptional = mongoRecord["IP DOCUMENTARY REQUIREMENTS(Optional)"].ToString().Split('|').ToList();
                var ipDocOptionalTrimmed = new List<string>();

                if (ipDocOptional != null && !ipDocOptional.Contains("NaN"))
                {
                    foreach (var lpco in ipDocOptional)
                    {
                        ipDocOptionalTrimmed.Add(lpco.Trim());
                    }

                    foreach (var doc in ipDocOptionalTrimmed)
                    {
                        var tempReq = new DocumentaryRequirement();

                        tempReq.Name = doc + " For Import Permit"; //replace DPP with collectionName 
                        tempReq.DocumentName = doc;
                        tempReq.IsMandatory = false;
                        tempReq.RequirementType = "Documentary";

                        tempReq.DocumentTypeCode = Command.UnitOfWork.DocumentTypeRepository.Where(new { Name = doc }).FirstOrDefault()?.Code;
                        tempReq.AttachedObjectFormatID = 1;

                        tarpDocumentRequirements.Add(tempReq);
                    }
                }

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
                ValidityRequirement.Quantity = Convert.ToInt32(uomPeriod.Length > 1 ? uomPeriod.Substring(0, 2) : uomPeriod);

            }
            //for ReleaseOrder = RO
            else if (documentClassification == "RO")
            {
                var roDocRequirements = mongoRecord["RO  DOCUMENTARY REQUIREMENTS"].ToString().Split('|').ToList();

                var roDocRequirementsTrimmed = new List<string>();

                var roDocOptional = mongoRecord["RO  DOCUMENTARY REQUIREMENTS(Optional)"].ToString().Split('|').ToList();
                var roDocOptionalTrimmed = new List<string>();

                if (roDocOptional != null && !roDocOptional.Contains("NaN"))
                {
                    foreach (var lpco in roDocOptional)
                    {
                        roDocOptionalTrimmed.Add(lpco.Trim());
                    }

                    foreach (var doc in roDocOptionalTrimmed)
                    {
                        var tempReq = new DocumentaryRequirement();

                        tempReq.Name = doc + " For " + "Release Order"; //replace DPP with collectionName 
                        tempReq.DocumentName = doc;
                        tempReq.IsMandatory = false;
                        tempReq.RequirementType = "Documentary";

                        tempReq.DocumentTypeCode = Command.UnitOfWork.DocumentTypeRepository.Where(new { Name = doc }).FirstOrDefault()?.Code;
                        tempReq.AttachedObjectFormatID = 1;

                        tarpDocumentRequirements.Add(tempReq);
                    }
                }

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

                    tempReq.Name = doc + " For " + "Release Order"; //replace DPP with collectionName 
                    tempReq.DocumentName = doc;
                    tempReq.IsMandatory = true;
                    tempReq.RequirementType = "Documentary";

                    tempReq.DocumentTypeCode = Command.UnitOfWork.DocumentTypeRepository.Where(new { Name = doc }).FirstOrDefault()?.Code;
                    tempReq.AttachedObjectFormatID = 1;

                    tarpDocumentRequirements.Add(tempReq);
                }
                if (mongoRecord["IP REQUIRED"].ToString().ToLower() == "yes")
                {
                    var tempReq = new DocumentaryRequirement();
                    var ipDocRequired = Command.UnitOfWork.DocumentTypeRepository.Where(new { AgencyID = RequestDTO.AgencyId, documentClassificationCode = "IMP", AttachedObjectFormatID = 2, AltCode = "C" }).FirstOrDefault();

                    tempReq.Name = ipDocRequired.Name + " For " + "Release Order"; //replace DPP with collectionName 
                    tempReq.DocumentName = ipDocRequired.Name;
                    tempReq.IsMandatory = true;
                    tempReq.RequirementType = "Documentary";
                    tempReq.DocumentTypeCode = ipDocRequired.Code;
                    tempReq.AttachedObjectFormatID = ipDocRequired.AttachedObjectFormatID;

                    tarpDocumentRequirements.Add(tempReq);

                }

                //Financial Requirements
                FinancialRequirement.PlainAmount = mongoRecord["RO FEES"].ToString();
                FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(mongoRecord["RO FEES"].ToString());
            }

            //for PythoCertificate = EC
            else if (documentClassification == "EC")
            {
                var ecDocRequirements = mongoRecord["PHYTOSANITARY  DOCUMENTARY REQUIREMENTS"].ToString().Split('|').ToList();

                var ecDocRequirementsTrimmed = new List<string>();

                var ecDocOptional = mongoRecord["PHYTOSANITARY  DOCUMENTARY REQUIREMENTS(optional)"].ToString().Split('|').ToList();
                var ecDocOptionalTrimmed = new List<string>();

                if (ecDocOptional != null && !ecDocOptional.Contains("NaN"))
                {
                    foreach (var lpco in ecDocOptional)
                    {
                        ecDocOptionalTrimmed.Add(lpco.Trim());
                    }

                    foreach (var doc in ecDocOptionalTrimmed)
                    {
                        var tempReq = new DocumentaryRequirement();

                        tempReq.Name = doc + " For Phythosanitary Certificate";
                        tempReq.DocumentName = doc;
                        tempReq.IsMandatory = false;
                        tempReq.RequirementType = "Documentary";

                        tempReq.DocumentTypeCode = Command.UnitOfWork.DocumentTypeRepository.Where(new { Name = doc }).FirstOrDefault()?.Code;
                        tempReq.AttachedObjectFormatID = 1;

                        tarpDocumentRequirements.Add(tempReq);
                    }
                }

                foreach (var lpco in ecDocRequirements)
                {
                    var removeSpaces = lpco.Trim();
                    ecDocRequirementsTrimmed.Add(removeSpaces.TrimEnd('\n'));
                }

                // roDocRequirementsTrimmed.Remove("Application on DPP prescribed form 20 [Rule 44(1) of PQR 2019]");
                // roDocRequirementsTrimmed.Remove("Fee Challan");

                //DocumentaryRequirements
                foreach (var doc in ecDocRequirementsTrimmed)
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
                FinancialRequirement.PlainAmmendmentFee = mongoRecord["PHYTOSANITARY AMENDMENT FEES"].ToString();
                FinancialRequirement.AmmendmentFee = Command.CryptoAlgorithm.Encrypt(mongoRecord["PHYTOSANITARY AMENDMENT FEES"].ToString());
            }
            //for Seed enlistment = SE
            if (documentClassification == "SE")
            {
                //CURRENTLY USING IMPORTPERMIT DATA: UPDATE REQUIRED WHEN FSCRD DATA ADDED
                var ipDocRequirements = mongoRecord["IP DOCUMENTARY REQUIREMENTS"].ToString().Split('|').ToList();
                var ipDocRequirementsTrimmed = new List<string>();

                var ipDocOptional = mongoRecord["IP DOCUMENTARY REQUIREMENTS(Optional)"].ToString().Split('|').ToList();
                var ipDocOptionalTrimmed = new List<string>();

                if (ipDocOptional != null && !ipDocOptional.Contains("NaN"))
                {
                    foreach (var lpco in ipDocOptional)
                    {
                        ipDocOptionalTrimmed.Add(lpco.Trim());
                    }

                    foreach (var doc in ipDocOptionalTrimmed)
                    {
                        var tempReq = new DocumentaryRequirement();

                        tempReq.Name = doc + " For Seed Enlistment"; //replace DPP with collectionName 
                        tempReq.DocumentName = doc;
                        tempReq.IsMandatory = false;
                        tempReq.RequirementType = "Documentary";

                        tempReq.DocumentTypeCode = Command.UnitOfWork.DocumentTypeRepository.Where(new { Name = doc }).FirstOrDefault()?.Code;
                        tempReq.AttachedObjectFormatID = 1;

                        tarpDocumentRequirements.Add(tempReq);
                    }
                }

                foreach (var lpco in ipDocRequirements)
                {
                    ipDocRequirementsTrimmed.Add(lpco.Trim());
                }

                foreach (var doc in ipDocRequirementsTrimmed)
                {
                    var tempReq = new DocumentaryRequirement();

                    tempReq.Name = doc + " For Seed Enlistment"; //replace DPP with collectionName 
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

            tarpRequirments.DocumentaryRequirementList = tarpDocumentRequirements;
            tarpRequirments.FinancialRequirement = FinancialRequirement;
            tarpRequirments.ValidityRequirement = ValidityRequirement;

            return tarpRequirments;
        }
    }
}