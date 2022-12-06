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
                // if (RequestDTO.FactorCodeValuePair == null || RequestDTO.FactorCodeValuePair.Count == 0)
                // {
                //     return BadRequestReply("Please provide valid request parameters");
                // }

                Log.Information("|{0}|{1}| Request DTO {@RequestDTO}", StrategyName, MethodID, RequestDTO);

                // var tempHsCode = Command.UnitOfWork.RegulatedHSCodeRepository.Where(
                //     new
                //     {
                //         HSCodeExt = RequestDTO.HsCode,
                //         AgencyID = RequestDTO.AgencyId,
                //         RequiredDocumentTypeCode = RequestDTO.documentTypeCode,
                //         TradeTranTypeID = RequestDTO.TradeTranTypeID
                //     }
                //     ).FirstOrDefault();

                RegulatedHSCode tempHsCode = Command.UnitOfWork.RegulatedHSCodeRepository.GetActiveHsCode(RequestDTO.HsCode, RequestDTO.AgencyId, RequestDTO.TradeTranTypeID, RequestDTO.documentTypeCode);

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
                var factorDataList = new List<Factors>();

                Log.Information("|{0}|{1}| FactorID's Applied To Rule DbRecord {@factorsIDAppliedTORule}", StrategyName, MethodID, factorsIDAppliedTORule);

                if (factorsIDAppliedTORule.Count > 0)
                {
                    factorDataList = Command.UnitOfWork.FactorRepository.GetFactorsData(factorsIDAppliedTORule);

                    if (factorDataList == null)
                    {
                        return BadRequestReply("Factors data not found");
                    }

                    Log.Information("|{0}|{1}| FactorData DbRecord {@factorDataList}", StrategyName, MethodID, factorDataList);
                }

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
                    if (RequestDTO.AgencyId == "2")
                    {
                        mongoDoc = mongoDBRecordFetcher.GetFilteredRecord(RequestDTO.HsCode, RequestDTO.FactorCodeValuePair["PURPOSE"].FactorValue);
                        if (mongoDoc == null)
                        {
                            return BadRequestReply(String.Format("No record found for HsCode : {0}  Purpose : {1}", RequestDTO.HsCode, RequestDTO.FactorCodeValuePair["PURPOSE"].FactorValue));
                        }
                    }
                    else if (RequestDTO.AgencyId == "3")
                    {
                        mongoDoc = mongoDBRecordFetcher.GetFilteredRecordAQD(RequestDTO.HsCode, RequestDTO.FactorCodeValuePair["CATEGORY"].FactorValue);
                        if (mongoDoc == null)
                        {
                            return BadRequestReply(String.Format("No record found for HsCode : {0}  Category : {1}", RequestDTO.HsCode, RequestDTO.FactorCodeValuePair["CATEGORY"].FactorValue));
                        }
                    }
                    else if (RequestDTO.AgencyId == "4")
                    {
                        mongoDoc = mongoDBRecordFetcher.GetFilteredRecordFSCRD(RequestDTO.HsCode, RequestDTO.FactorCodeValuePair["PURPOSE"].FactorValue);
                        if (mongoDoc == null)
                        {
                            return BadRequestReply(String.Format("No record found for HsCode : {0}  Purpose : {1}", RequestDTO.HsCode, RequestDTO.FactorCodeValuePair["PURPOSE"].FactorValue));
                        }
                    }
                    else if (RequestDTO.AgencyId == "5")
                    {
                        mongoDoc = mongoDBRecordFetcher.GetFilteredRecordPSQCA(RequestDTO.HsCode);
                        if (mongoDoc == null)
                        {
                            return BadRequestReply(String.Format("No record found for HsCode : {0}", RequestDTO.HsCode));
                        }
                    }
                    else if (RequestDTO.AgencyId == "10")
                    {
                        mongoDoc = mongoDBRecordFetcher.GetFilteredRecordMFD(RequestDTO.HsCode);
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

                Log.Information("|{0}|{1}| Required LPCO Parent Code {@documentClassification}", StrategyName, MethodID, docType.DocumentClassificationCode);

                ResponseDTO = new GetDocumentRequirementResponse();

                bool DocumentIsRequired = false;
                bool IsParenCodeValid = false;

                if (RequestDTO.AgencyId == "2")
                {
                    DocumentIsRequired = mongoDBRecordFetcher.CheckIfLPCORequired(mongoDoc, docType.DocumentClassificationCode, out IsParenCodeValid);
                }
                else if (RequestDTO.AgencyId == "3")
                {
                    DocumentIsRequired = mongoDBRecordFetcher.CheckIfLPCORequiredAQD(mongoDoc, docType.DocumentClassificationCode, out IsParenCodeValid);
                }
                else if (RequestDTO.AgencyId == "4")
                {
                    DocumentIsRequired = mongoDBRecordFetcher.CheckIfLPCORequiredFSCRD(mongoDoc, docType.DocumentClassificationCode, out IsParenCodeValid);
                }
                else if (RequestDTO.AgencyId == "5")
                {
                    DocumentIsRequired = mongoDBRecordFetcher.CheckIfLPCORequiredPSQCA(mongoDoc, docType.DocumentClassificationCode, out IsParenCodeValid);
                }
                else if (RequestDTO.AgencyId == "10")
                {
                    DocumentIsRequired = mongoDBRecordFetcher.CheckIfLPCORequiredMFD(mongoDoc, docType.DocumentClassificationCode, out IsParenCodeValid);
                }

                if (!IsParenCodeValid)
                {
                    Log.Information("|{0}|{1}| Parent Code is Not Valid", StrategyName, MethodID);

                    return BadRequestReply("Document does not belong to a supported document Classification");
                }
                else if (!DocumentIsRequired)
                {
                    Log.Information("|{0}|{1}| LPCO required {2}", StrategyName, MethodID, "false");

                    ResponseDTO.isLPCORequired = false;

                    return OKReply(string.Format("{0} not required for HsCode : {1} and Purpose : {2}", docType.Name, RequestDTO.HsCode, RequestDTO.FactorCodeValuePair["PURPOSE"].FactorValue));
                }

                Log.Information("|{0}|{1}| LPCO required {2}", StrategyName, MethodID, "true");

                var recordChecker = CheckFactorInMongoRecord(factorDataList, mongoDoc, RequestDTO.FactorCodeValuePair);

                var tempDocumentaryRequirementList = new List<DocumentaryRequirement>();

                if (recordChecker == "Checked")
                {
                    var response = GetRequirements(mongoDoc, docType.DocumentClassificationCode);

                    if (!response.IsError)
                    {
                        ResponseDTO = response.Model;
                    }
                    else
                    {
                        Log.Error("|{0}|{1}| Error ", StrategyName, MethodID, response.Error.InternalError.Message);
                        return InternalServerErrorReply(response.Error.InternalError.Message);
                    }
                    if (RequestDTO.AgencyId == "2")
                    {
                        ResponseDTO.FormNumber = mongoDBRecordFetcher.GetFormNumber(mongoDoc, docType.DocumentClassificationCode);
                    }
                    else if (RequestDTO.AgencyId == "3")
                    {
                        ResponseDTO.FormNumber = mongoDBRecordFetcher.GetFormNumberAQD(mongoDoc, docType.DocumentClassificationCode);
                    }
                    else if (RequestDTO.AgencyId == "4")
                    {
                        ResponseDTO.FormNumber = mongoDBRecordFetcher.GetFormNumberFSCRD(mongoDoc, docType.DocumentClassificationCode);
                    }
                    else if (RequestDTO.AgencyId == "10")
                    {
                        ResponseDTO.FormNumber = mongoDBRecordFetcher.GetFormNumberMFD(mongoDoc, docType.DocumentClassificationCode);
                    }


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
            Log.Information("[{0}.{1}] Started", GetType().Name, MethodBase.GetCurrentMethod().Name);
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
            Log.Information("[{0}.{1}] Ended", GetType().Name, MethodBase.GetCurrentMethod().Name);
            if (count == factorDataList.Count)
            {
                return "Checked";
            }
            else
            {
                return " ";
            }
        }

        public SingleResponseModel<GetDocumentRequirementResponse> GetRequirements(BsonDocument mongoRecord, string documentClassification)
        {
            Log.Information("[{0}.{1}] Started", GetType().Name, MethodBase.GetCurrentMethod().Name);
            GetDocumentRequirementResponse tarpRequirments = new GetDocumentRequirementResponse();
            var response = new SingleResponseModel<GetDocumentRequirementResponse>();

            var tarpDocumentRequirements = new List<DocumentaryRequirement>();
            var FinancialRequirement = new FinancialRequirement();
            var ValidityRequirement = new ValidityRequirement();

            tarpRequirments.isLPCORequired = true;

            //for Import Permit = IMP
            if (documentClassification == "IMP" || documentClassification == "PRD")
            {
                var ipDocRequirements = new List<string>();
                var ipDocRequirementsTrimmed = new List<string>();
                var ipDocOptional = new List<string>();
                var ipDocOptionalTrimmed = new List<string>();

                if (RequestDTO.AgencyId == "4")
                {
                    ipDocRequirements = mongoRecord["ENLISTMENT OF SEED VARIETY MANDATORY DOCUMENTARY REQURIMENTS"].ToString().Split('|').ToList();
                    ipDocOptional = mongoRecord["ENLISTMENT OF SEED VARIETY OPTIONAL DOCUMENTARY REQURIMENTS"].ToString().Split('|').ToList();

                    //Financial Requirements
                    FinancialRequirement.PlainAmount = mongoRecord["ENLISTMENT OF SEED VARIETY  FEES"].ToString();
                    FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(mongoRecord["ENLISTMENT OF SEED VARIETY  FEES"].ToString());
                }
                else
                {
                    ipDocRequirements = mongoRecord["IP DOCUMENTARY REQUIREMENTS"].ToString().Split('|').ToList();
                    ipDocOptional = mongoRecord["IP DOCUMENTARY REQUIREMENTS(Optional)"].ToString().Split('|').ToList();

                    //Financial Requirements
                    FinancialRequirement.PlainAmount = mongoRecord["IP FEES"].ToString();
                    FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(mongoRecord["IP FEES"].ToString());
                    FinancialRequirement.PlainAmmendmentFee = mongoRecord["IP Amendment Fees"].ToString();
                    FinancialRequirement.AmmendmentFee = Command.CryptoAlgorithm.Encrypt(mongoRecord["IP Amendment Fees"].ToString());
                    FinancialRequirement.PlainExtensionFee = mongoRecord["IP Extention Fees"].ToString();
                    FinancialRequirement.ExtensionFee = Command.CryptoAlgorithm.Encrypt(mongoRecord["IP Extention Fees"].ToString());


                    //ValidityTerm Requirements
                    ValidityRequirement.UomName = "Month";
                    ValidityRequirement.Quantity = Convert.ToInt32(mongoRecord["IP VALIDITY"]);
                    ValidityRequirement.ExtensionAllowed = mongoRecord["IP Extention Allowed"].ToString().ToLower() == "yes" ? true : false;
                    ValidityRequirement.ExtensionPeriod = Convert.ToInt32(mongoRecord["IP Extention Period (Months)"]);
                    ValidityRequirement.ExtensionPeriodUnitName = "Months";     // Hard coded till we have a separate column in sheet for this

                    //Quantity Allowed
                    if (RequestDTO.FactorCodeValuePair["PURPOSE"].FactorValue.ToString().Trim().ToLower() == Common.Constants.TradePurpose.ScreeningResearchTrial)
                    {
                        tarpRequirments.AllowedQuantity = mongoRecord["QUANTITY ALLOWED"].ToString();
                    }
                }

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

                if (ipDocRequirements != null && !ipDocRequirements.Contains("NaN"))
                {
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
                }
            }
            //for ReleaseOrder = RO
            else if (documentClassification == "RO")
            {
                var roDocRequirements = new List<string>();
                var roDocRequirementsTrimmed = new List<string>();
                var roDocOptional = new List<string>();
                var roDocOptionalTrimmed = new List<string>();
                var ipReq = false;
                var psiReq = false;
                var psiRegReq = false;
                var docClassificCode = string.Empty;


                if (RequestDTO.AgencyId == "3")
                {
                    roDocRequirements = mongoRecord["RELEASE ORDER PROCESSING MANDATORY REQUIREMENTS"].ToString().Split('|').ToList();
                    roDocOptional = mongoRecord["RELEASE ORDER PROCESSING OPTIONAL REQUIREMENTS"].ToString().Split('|').ToList();
                    // ipReq = mongoRecord["ENLISTMENT OF SEED VARIETY REQUIRED (Yes/No)"].ToString().ToLower() == "yes";
                    //  docClassificCode = "PRD";

                    if (RequestDTO.IsFinancialRequirement)
                    {
                        AQDECFeeCalculateRequestDTO calculateECFeeRequest = new AQDECFeeCalculateRequestDTO();
                        calculateECFeeRequest.AgencyId = Convert.ToInt32(RequestDTO.AgencyId);
                        calculateECFeeRequest.HsCodeExt = RequestDTO.HsCode;
                        calculateECFeeRequest.Quantity = Convert.ToInt32(RequestDTO.Quantity);
                        calculateECFeeRequest.TradeTranTypeID = RequestDTO.TradeTranTypeID;
                        FactorData factorData = RequestDTO.FactorCodeValuePair["UNIT"];
                        if (factorData != null && !string.IsNullOrEmpty(factorData.FactorValueID))
                        {
                            calculateECFeeRequest.AgencyUOMId = Convert.ToInt32(factorData.FactorValueID);
                        }

                        AQDECFeeCalculation feeCalculation = new AQDECFeeCalculation(Command.UnitOfWork, calculateECFeeRequest);
                        var responseModel = feeCalculation.CalculateECFee();
                        if (!responseModel.IsError)
                        {

                            FinancialRequirement.PlainAmount = responseModel.Model.Amount;
                            FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(FinancialRequirement.PlainAmount);
                            FinancialRequirement.PlainAmmendmentFee = responseModel.Model.Amount;
                            FinancialRequirement.AmmendmentFee = Command.CryptoAlgorithm.Encrypt(FinancialRequirement.PlainAmmendmentFee);
                        }
                        else
                        {
                            Log.Information("Response {@message}", responseModel.Error.InternalError.Message);
                            // return InternalServerErrorReply(responseModel.Error.InternalError.Message);
                        }

                    }
                }
                else if (RequestDTO.AgencyId == "4")
                {
                    roDocRequirements = mongoRecord["RELEASE ORDER DOCUMENTARY REQUIRMENTS"].ToString().Split('|').ToList();
                    roDocOptional = mongoRecord["RELEASE ORDER DOCUMENTARY REQUIRMENTS (Optional)"].ToString().Split('|').ToList();
                    ipReq = mongoRecord["ENLISTMENT OF SEED VARIETY REQUIRED (Yes/No)"].ToString().ToLower() == "yes";
                    docClassificCode = "PRD";

                    //Financial Requirements
                    FinancialRequirement.PlainAmount = mongoRecord["RELEASE ORDER FEES"].ToString();
                    FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(mongoRecord["RELEASE ORDER FEES"].ToString());
                }
                else if (RequestDTO.AgencyId == "5")
                {
                    roDocRequirements = mongoRecord["RO  Mandatory DOCUMENTARY REQUIREMENTS"].ToString().Split('|').ToList();
                    roDocOptional = mongoRecord["RO  Optional DOCUMENTARY REQUIREMENTS"].ToString().Split('|').ToList();
                    ipReq = false;

                    //Financial Requirements
                    if (RequestDTO.IsFinancialRequirement)
                    {
                        var feeConfigurationList = Command.UnitOfWork.LPCOFeeConfigurationRepository.GetFeeConfig(
                            RequestDTO.HsCode,
                            RequestDTO.TradeTranTypeID,
                            Convert.ToInt32(RequestDTO.AgencyId)
                        ).FirstOrDefault();

                        var feeConfig = new LPCOFeeCleanResp();
                        feeConfig.AdditionalAmount = feeConfigurationList.AdditionalAmount;
                        feeConfig.AdditionalAmountOn = feeConfigurationList.AdditionalAmountOn;
                        feeConfig.Rate = feeConfigurationList.Rate;
                        feeConfig.CalculationBasis = feeConfigurationList.CalculationBasis;
                        feeConfig.CalculationSource = feeConfigurationList.CalculationSource;
                        feeConfig.MinAmount = feeConfigurationList.MinAmount;

                        var calculatedFee = new LPCOFeeCalculator(feeConfig, RequestDTO).Calculate();

                        FinancialRequirement.PlainAmount = calculatedFee.Fee.ToString();
                        FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(calculatedFee.Fee.ToString());
                        FinancialRequirement.AdditionalAmount = calculatedFee.AdditionalAmount;
                        FinancialRequirement.AdditionalAmountOn = calculatedFee.AdditionalAmountOn;
                    }
                }
                else
                {
                    roDocRequirements = mongoRecord["RO  DOCUMENTARY REQUIREMENTS"].ToString().Split('|').ToList();
                    roDocOptional = mongoRecord["RO  DOCUMENTARY REQUIREMENTS(Optional)"].ToString().Split('|').ToList();
                    ipReq = mongoRecord["IP REQUIRED"].ToString().ToLower() == "yes";
                    docClassificCode = "IMP";

                    // Check if HS Code is PSI related.  
                    var IsPSi = mongoRecord["Is PSI"].ToString().ToLower() == "yes";
                    if (IsPSi)
                    {
                        psiReq = mongoRecord["PSI REQUIRED (YES/NO)"].ToString().ToLower() == "yes";
                        psiRegReq = mongoRecord["REGISTRATION REQUIRED (YES/NO)"].ToString().ToLower() == "yes";
                    }


                    //Financial Requirements
                    FinancialRequirement.PlainAmount = mongoRecord["RO FEES"].ToString();
                    FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(mongoRecord["RO FEES"].ToString());
                }

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

                if (roDocRequirements != null && !roDocRequirements.Contains("NaN"))
                {
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
                }

                if (ipReq == true)
                {
                    var tempReq = new DocumentaryRequirement();
                    var ipDocRequired = Command.UnitOfWork.DocumentTypeRepository.Where(new { AgencyID = RequestDTO.AgencyId, documentClassificationCode = docClassificCode, AttachedObjectFormatID = 2, AltCode = "C" }).FirstOrDefault();

                    tempReq.Name = ipDocRequired.Name + " For " + "Release Order"; //replace DPP with collectionName 
                    tempReq.DocumentName = ipDocRequired.Name;
                    tempReq.IsMandatory = true;
                    tempReq.RequirementType = "Documentary";
                    tempReq.DocumentTypeCode = ipDocRequired.Code;
                    tempReq.AttachedObjectFormatID = ipDocRequired.AttachedObjectFormatID;

                    tarpDocumentRequirements.Add(tempReq);

                }

                if (psiReq)
                {
                    var tempReq = new DocumentaryRequirement();

                    var psiDocRequired = Command.UnitOfWork.DocumentTypeRepository.Where(new
                    {
                        // AgencyID = RequestDTO.AgencyId, 
                        // documentClassificationCode = docClassificCode, 
                        // AttachedObjectFormatID = 2, 
                        // AltCode = "C" 
                        Code = "D58" // TODO : Remove hardcoded values
                    }).FirstOrDefault();

                    tempReq.Name = psiDocRequired.Name + " For " + "Release Order"; //replace DPP with collectionName 
                    tempReq.DocumentName = psiDocRequired.Name;
                    tempReq.IsMandatory = false;
                    tempReq.RequirementType = "Documentary";
                    tempReq.DocumentTypeCode = psiDocRequired.Code;
                    tempReq.AttachedObjectFormatID = psiDocRequired.AttachedObjectFormatID;

                    tarpDocumentRequirements.Add(tempReq);

                }

                if (psiRegReq)
                {
                    var tempReq = new DocumentaryRequirement();

                    var psiRegRequired = Command.UnitOfWork.DocumentTypeRepository.Where(new
                    {
                        // AgencyID = RequestDTO.AgencyId, 
                        // documentClassificationCode = docClassificCode, 
                        // AttachedObjectFormatID = 2, 
                        // AltCode = "C" 
                        Code = "D60" // TODO : 
                    }).FirstOrDefault();

                    tempReq.Name = psiRegRequired.Name + " For " + "Release Order"; //replace DPP with collectionName //
                    tempReq.DocumentName = psiRegRequired.Name;
                    tempReq.IsMandatory = false;
                    tempReq.RequirementType = "Documentary";
                    tempReq.DocumentTypeCode = psiRegRequired.Code;
                    tempReq.AttachedObjectFormatID = psiRegRequired.AttachedObjectFormatID;

                    tarpDocumentRequirements.Add(tempReq);
                }
            }

            //for PythoCertificate = EC
            // NO EC in Agency 4 - FSCRD
            else if (documentClassification == "EC")
            {
                var ecDocRequirements = new List<string>();
                var ecDocRequirementsTrimmed = new List<string>();
                var ecDocOptional = new List<string>();
                var ecDocOptionalTrimmed = new List<string>();
                var premisesRegistrationRequired = false;
                var healthCertificateFeeRequired = false;
                var countries = new List<string>();

                if (RequestDTO.AgencyId == "2")
                {
                    ecDocRequirements = mongoRecord["Documentary requirements (Mandatory Phytosanitary certificate)"].ToString().Split('|').ToList();
                }
                else if (RequestDTO.AgencyId == "3")
                {
                    ecDocRequirements = mongoRecord["Health Certificate Processing Requirements"].ToString().Split('|').ToList();
                }
                else if (RequestDTO.AgencyId == "10")
                {
                    ecDocRequirements = mongoRecord["Certificate of Quality and Origin Processing Mandatory Documentary Requirements"].ToString().Split('|').ToList();
                    ecDocOptional = mongoRecord["Certificate of Quality and Origin Processing  Optional  Documentary Requirements"].ToString().Split('|').ToList();
                    premisesRegistrationRequired = mongoRecord["Is Premises Registration Required? (Yes/No)"].ToString().ToLower() == "yes";
                }


                if (RequestDTO.AgencyId == "2")
                {
                    ecDocOptional = mongoRecord["Documentary requirements (Optional Phytosanitary certificate)"].ToString().Split('|').ToList();
                }
                else if (RequestDTO.AgencyId == "3")
                {
                    ecDocOptional = mongoRecord["Optional Health Certificate"].ToString().Split('|').ToList();
                }

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
                        if (!string.IsNullOrEmpty(tempReq.DocumentTypeCode))
                        {
                            tarpDocumentRequirements.Add(tempReq);
                        }
                    }
                }

                if (ecDocRequirements != null && !ecDocRequirements.Contains("NaN"))
                {
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
                        if (!string.IsNullOrEmpty(tempReq.DocumentTypeCode))
                        {
                            tarpDocumentRequirements.Add(tempReq);
                        }
                    }

                }

                if (premisesRegistrationRequired == true)
                {
                    // TODO : Attach this Later
                    var tempReq = new DocumentaryRequirement();
                    var premisesRegistration = Command.UnitOfWork.DocumentTypeRepository.Where(new
                    {
                        AgencyID = RequestDTO.AgencyId,
                        Code = "A09"
                    }).FirstOrDefault();

                    if (premisesRegistration != null)
                    {
                        tempReq.Name = premisesRegistration.Name + " For " + "Certificate";
                        tempReq.DocumentName = premisesRegistration.Name;
                        tempReq.IsMandatory = true; // Change this later 
                        tempReq.RequirementType = "Documentary";
                        tempReq.DocumentTypeCode = premisesRegistration.Code;
                        tempReq.AttachedObjectFormatID = premisesRegistration.AttachedObjectFormatID;
                        tarpDocumentRequirements.Add(tempReq);

                    }

                }

                if (RequestDTO.IsFinancialRequirement)
                {
                    Log.Information("|{0}|{1}| RequestDTO.IsFinancialRequirement {@IsFinancialRequirement}", StrategyName, MethodID, RequestDTO.IsFinancialRequirement);

                    //Financial Requirements
                    if (RequestDTO.AgencyId == "2")
                    {
                        FinancialRequirement.PlainAmount = mongoRecord["Phytosanitary certification Fee"].ToString();
                        FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(mongoRecord["Phytosanitary certification Fee"].ToString());
                        FinancialRequirement.PlainAmmendmentFee = mongoRecord["Phytosanitary  certification Amendmend/Re-issue Fee "].ToString();
                        FinancialRequirement.AmmendmentFee = Command.CryptoAlgorithm.Encrypt(mongoRecord["Phytosanitary  certification Amendmend/Re-issue Fee "].ToString());
                    }
                    else if (RequestDTO.AgencyId == "3")
                    {
                        AQDECFeeCalculateRequestDTO calculateECFeeRequest = new AQDECFeeCalculateRequestDTO();
                        calculateECFeeRequest.AgencyId = Convert.ToInt32(RequestDTO.AgencyId);
                        calculateECFeeRequest.HsCodeExt = RequestDTO.HsCode;
                        calculateECFeeRequest.Quantity = Convert.ToInt32(RequestDTO.Quantity);
                        calculateECFeeRequest.TradeTranTypeID = RequestDTO.TradeTranTypeID;
                        FactorData factorData = RequestDTO.FactorCodeValuePair["UNIT"];
                        if (factorData != null && !string.IsNullOrEmpty(factorData.FactorValueID))
                        {
                            calculateECFeeRequest.AgencyUOMId = Convert.ToInt32(factorData.FactorValueID);
                        }

                        AQDECFeeCalculation feeCalculation = new AQDECFeeCalculation(Command.UnitOfWork, calculateECFeeRequest);
                        var responseModel = feeCalculation.CalculateECFee();
                        if (!responseModel.IsError)
                        {

                            FinancialRequirement.PlainAmount = responseModel.Model.Amount;
                            FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(FinancialRequirement.PlainAmount);
                            FinancialRequirement.PlainAmmendmentFee = responseModel.Model.Amount;
                            FinancialRequirement.AmmendmentFee = Command.CryptoAlgorithm.Encrypt(FinancialRequirement.PlainAmmendmentFee);
                        }
                        else
                        {
                            Log.Information("Response {@message}", responseModel.Error.InternalError.Message);
                            // return InternalServerErrorReply(responseModel.Error.InternalError.Message);
                        }
                    }
                    if (RequestDTO.AgencyId == "10")
                    {
                        Log.Information("|{0}|{1}| RequestDTO.AgencyId  10 ", StrategyName, MethodID);


                        // TODO Fee releated stuff later
                        // // get fee  
                        // FinancialRequirement.PlainAmount = mongoRecord["Certificate of Quality and Origin Processing Fee (PKR)"].ToString();
                        // FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(mongoRecord["Certificate of Quality and Origin Processing Fee (PKR)"].ToString());
                        // FinancialRequirement.PlainAmount = mongoRecord["Health Certificate Fee(PKR)"].ToString();
                        // FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(mongoRecord["Health Certificate Fee(PKR)"].ToString());


                        string ECFeeString = mongoRecord["Certificate of Quality and Origin Processing Fee (PKR)"].ToString();

                        decimal ECFeeDecimal = 0.0m;
                        if (!string.IsNullOrEmpty(ECFeeString))
                            decimal.TryParse(ECFeeString, out ECFeeDecimal);


                        // The column that tells if Health Certificate is Fee Required (Conditional)
                        // Condition: If the destination country is from one of the countries in the following column, then fee is applied.
                        // "Names of Countries Requiring Health Certificate on prescribed format"
                        countries = mongoRecord["Codes of Countries Requiring Health Certificate on prescribed format"].ToString().Split('|').ToList();
                        Log.Information("|{0}|{1}| countries {@countries}", StrategyName, MethodID, countries);
                        Log.Information("|{0}|{1}| RequestDTO.DestinationCountryCode {@DestinationCountryCode}", StrategyName, MethodID, RequestDTO.DestinationCountryCode);
                        string HealthCertFeeString = string.Empty;
                        if (countries.Contains(RequestDTO.DestinationCountryCode))
                        {
                            Log.Information("|{0}|{1}| contians {@RequestDTO.DestinationCountryCode}", StrategyName, MethodID, RequestDTO.DestinationCountryCode);
                            healthCertificateFeeRequired = true; // use later 

                            Log.Information("|{0}|{1}| ECFeeDecimal {@ECFeeDecimal}", StrategyName, MethodID, ECFeeDecimal);

                            HealthCertFeeString = mongoRecord["Health Certificate Fee (PKR)"].ToString();
                            Log.Information("|{0}|{1}| HealthCertFeeString {HealthCertFeeString}", StrategyName, MethodID, HealthCertFeeString);
                            decimal HealthCertFeeDecimal = 0.0m;
                            if (!string.IsNullOrEmpty(HealthCertFeeString))
                                decimal.TryParse(HealthCertFeeString, out HealthCertFeeDecimal);

                            Log.Information("|{0}|{1}| ECFeeDecimal {@HealthCertFeeDecimal}", StrategyName, MethodID, HealthCertFeeDecimal);
                            ECFeeDecimal = HealthCertFeeDecimal + ECFeeDecimal;
                            Log.Information("|{0}|{1}| HealthCertFeeDecimal + ECFeeDecimal {@ECFeeDecimal}", StrategyName, MethodID, ECFeeDecimal);

                        }

                        FinancialRequirement.PlainAmount = ECFeeDecimal.ToString();
                        FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(ECFeeDecimal.ToString());
                        if (!string.IsNullOrEmpty(HealthCertFeeString))
                        {
                            FinancialRequirement.PlainAmmendmentFee = HealthCertFeeString;
                            FinancialRequirement.AmmendmentFee = Command.CryptoAlgorithm.Encrypt(FinancialRequirement.PlainAmmendmentFee);
                        }
                        else
                        {
                            FinancialRequirement.PlainAmmendmentFee = "0";
                            FinancialRequirement.AmmendmentFee = Command.CryptoAlgorithm.Encrypt(FinancialRequirement.PlainAmmendmentFee);
                        }

                    }
                }
            }

            tarpRequirments.DocumentaryRequirementList = tarpDocumentRequirements;
            tarpRequirments.FinancialRequirement = FinancialRequirement;
            tarpRequirments.ValidityRequirement = ValidityRequirement;

            response.Model = tarpRequirments;
            Log.Information("Tarp Requirments Response: {@response}", response);
            Log.Information("[{0}.{1}] Ended", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return response;
        }
    }
}
