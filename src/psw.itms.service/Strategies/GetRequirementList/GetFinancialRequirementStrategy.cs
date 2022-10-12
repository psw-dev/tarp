using MongoDB.Bson;
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
    public class GetFinancialRequirementStrategy : ApiStrategy<List<GetDocumentRequirementRequest>, GetFinancialRequirementResponse>
    {
        private Dictionary<LPCOFeeCleanResp, List<GetDocumentRequirementRequest>> psqcaFeeDict;

        #region Constructors 
        public GetFinancialRequirementStrategy(CommandRequest request) : base(request)
        {
            Reply = new CommandReply();
            psqcaFeeDict = new Dictionary<LPCOFeeCleanResp, List<GetDocumentRequirementRequest>>();
        }
        #endregion 

        #region Distructors 
        ~GetFinancialRequirementStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  

        public override CommandReply Execute()
        {
            try
            {
                foreach (var financialRequirementItem in RequestDTO)
                {
                    Log.Information("|{0}|{1}| Request DTO {@RequestDTO}", StrategyName, MethodID, RequestDTO);

                    RegulatedHSCode tempHsCode = Command.UnitOfWork.RegulatedHSCodeRepository.GetActiveHsCode(financialRequirementItem.HsCode, financialRequirementItem.AgencyId, financialRequirementItem.TradeTranTypeID, financialRequirementItem.documentTypeCode);

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
                        if (financialRequirementItem.AgencyId == "2")
                        {
                            mongoDoc = mongoDBRecordFetcher.GetFilteredRecord(financialRequirementItem.HsCode, financialRequirementItem.FactorCodeValuePair["PURPOSE"].FactorValue);
                            if (mongoDoc == null)
                            {
                                return BadRequestReply(String.Format("No record found for HsCode : {0}  Purpose : {1}", financialRequirementItem.HsCode, financialRequirementItem.FactorCodeValuePair["PURPOSE"].FactorValue));
                            }
                        }
                        else if (financialRequirementItem.AgencyId == "3")
                        {
                            mongoDoc = mongoDBRecordFetcher.GetFilteredRecordAQD(financialRequirementItem.HsCode, financialRequirementItem.FactorCodeValuePair["CATEGORY"].FactorValue);
                            if (mongoDoc == null)
                            {
                                return BadRequestReply(String.Format("No record found for HsCode : {0}  Category : {1}", financialRequirementItem.HsCode, financialRequirementItem.FactorCodeValuePair["CATEGORY"].FactorValue));
                            }
                        }
                        else if (financialRequirementItem.AgencyId == "4")
                        {
                            mongoDoc = mongoDBRecordFetcher.GetFilteredRecordFSCRD(financialRequirementItem.HsCode, financialRequirementItem.FactorCodeValuePair["PURPOSE"].FactorValue);
                            if (mongoDoc == null)
                            {
                                return BadRequestReply(String.Format("No record found for HsCode : {0}  Purpose : {1}", financialRequirementItem.HsCode, financialRequirementItem.FactorCodeValuePair["PURPOSE"].FactorValue));
                            }
                        }
                        else if (financialRequirementItem.AgencyId == "5")
                        {
                            mongoDoc = mongoDBRecordFetcher.GetFilteredRecordPSQCA(financialRequirementItem.HsCode);
                            if (mongoDoc == null)
                            {
                                return BadRequestReply(String.Format("No record found for HsCode : {0}", financialRequirementItem.HsCode));
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
                    { Code = financialRequirementItem.documentTypeCode }).FirstOrDefault();

                    Log.Information("|{0}|{1}| Required LPCO Parent Code {@documentClassification}", StrategyName, MethodID, docType.DocumentClassificationCode);

                    ResponseDTO = new GetFinancialRequirementResponse();

                    bool DocumentIsRequired = false;
                    bool IsParenCodeValid = false;

                    if (financialRequirementItem.AgencyId == "2")
                    {
                        DocumentIsRequired = mongoDBRecordFetcher.CheckIfLPCORequired(mongoDoc, docType.DocumentClassificationCode, out IsParenCodeValid);
                    }
                    else if (financialRequirementItem.AgencyId == "3")
                    {
                        DocumentIsRequired = mongoDBRecordFetcher.CheckIfLPCORequiredAQD(mongoDoc, docType.DocumentClassificationCode, out IsParenCodeValid);
                    }
                    else if (financialRequirementItem.AgencyId == "4")
                    {
                        DocumentIsRequired = mongoDBRecordFetcher.CheckIfLPCORequiredFSCRD(mongoDoc, docType.DocumentClassificationCode, out IsParenCodeValid);
                    }
                    else if (financialRequirementItem.AgencyId == "5")
                    {
                        DocumentIsRequired = mongoDBRecordFetcher.CheckIfLPCORequiredPSQCA(mongoDoc, docType.DocumentClassificationCode, out IsParenCodeValid);
                    }

                    if (!IsParenCodeValid)
                    {
                        Log.Information("|{0}|{1}| Parent Code is Not Valid", StrategyName, MethodID);

                        return BadRequestReply("Document does not belong to a supported document Classification");
                    }
                    else if (!DocumentIsRequired)
                    {
                        Log.Information("|{0}|{1}| LPCO required {2}", StrategyName, MethodID, "false");
                        Log.Information("{0} not required for HsCode : {1} and Purpose : {2}", docType.Name, financialRequirementItem.HsCode, financialRequirementItem.FactorCodeValuePair["PURPOSE"].FactorValue);
                        continue;
                    }

                    Log.Information("|{0}|{1}| LPCO required {2}", StrategyName, MethodID, "true");

                    var recordChecker = CheckFactorInMongoRecord(factorDataList, mongoDoc, financialRequirementItem.FactorCodeValuePair);

                    if (recordChecker == "Checked")
                    {
                        var response = GetRequirements(mongoDoc, docType.DocumentClassificationCode, financialRequirementItem);

                        if (!response.IsError)
                        {
                            ResponseDTO = response.Model;
                        }
                        else
                        {
                            Log.Error("|{0}|{1}| Error ", StrategyName, MethodID, response.Error.InternalError.Message);
                            return InternalServerErrorReply(response.Error.InternalError.Message);
                        }
                    }
                    else
                    {
                        return BadRequestReply(recordChecker);
                    }
                }

                if (psqcaFeeDict.Count() > 0)
                {
                    decimal plainAmount = 0m;
                    decimal additionalAmount = 0m;
                    string additionalAmountOn = string.Empty;
                    string docTypeCode = string.Empty;

                    foreach(var psqcaFeeElement in psqcaFeeDict)
                    {
                        var calculatedFee = new LPCOFeeCalculator(psqcaFeeElement.Key, psqcaFeeElement.Value).Calculate();
                        plainAmount += calculatedFee.Fee;
                        additionalAmount = calculatedFee.AdditionalAmount;
                        additionalAmountOn = calculatedFee.AdditionalAmountOn;
                        docTypeCode = psqcaFeeElement.Value[0].documentTypeCode;
                    }

                    plainAmount += additionalAmount;       // As fee is calculated document wise, assitional amount is also on document levels

                    var financialRequirement = new FinancialReq();
                    financialRequirement.PlainAmount = plainAmount.ToString();
                    financialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(plainAmount.ToString());
                    financialRequirement.AdditionalAmount = additionalAmount;
                    financialRequirement.AdditionalAmountOn = additionalAmountOn;
                    financialRequirement.documentTypeCode = docTypeCode;

                    ResponseDTO.DocumentFinancialRequirement = financialRequirement;
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

        public SingleResponseModel<GetFinancialRequirementResponse> GetRequirements(BsonDocument mongoRecord, string documentClassification, GetDocumentRequirementRequest request)
        {
            Log.Information("[{0}.{1}] Started", GetType().Name, MethodBase.GetCurrentMethod().Name);
            GetFinancialRequirementResponse tarpRequirments = new GetFinancialRequirementResponse();
            var response = new SingleResponseModel<GetFinancialRequirementResponse>();
            var FinancialRequirement = new FinancialReq();

            //for Import Permit = IMP
            if (documentClassification == "IMP" || documentClassification == "PRD")
            {
                if (request.AgencyId == "4")
                {
                    //Financial Requirements
                    FinancialRequirement.PlainAmount = mongoRecord["ENLISTMENT OF SEED VARIETY  FEES"].ToString();
                    FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(mongoRecord["ENLISTMENT OF SEED VARIETY  FEES"].ToString());
                }
                else
                {
                    //Financial Requirements
                    FinancialRequirement.PlainAmount = mongoRecord["IP FEES"].ToString();
                    FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(mongoRecord["IP FEES"].ToString());
                    FinancialRequirement.PlainAmmendmentFee = mongoRecord["IP Amendment Fees"].ToString();
                    FinancialRequirement.AmmendmentFee = Command.CryptoAlgorithm.Encrypt(mongoRecord["IP Amendment Fees"].ToString());
                    FinancialRequirement.PlainExtensionFee = mongoRecord["IP Extention Fees"].ToString();
                    FinancialRequirement.ExtensionFee = Command.CryptoAlgorithm.Encrypt(mongoRecord["IP Extention Fees"].ToString());
                }
            }
            //for ReleaseOrder = RO
            else if (documentClassification == "RO")
            {
                if (request.AgencyId == "3")
                {
                    if (request.IsFinancialRequirement)
                    {
                        AQDECFeeCalculateRequestDTO calculateECFeeRequest = new AQDECFeeCalculateRequestDTO();
                        calculateECFeeRequest.AgencyId = Convert.ToInt32(request.AgencyId);
                        calculateECFeeRequest.HsCodeExt = request.HsCode;
                        calculateECFeeRequest.Quantity = Convert.ToInt32(request.Quantity);
                        calculateECFeeRequest.TradeTranTypeID = request.TradeTranTypeID;
                        FactorData factorData = request.FactorCodeValuePair["UNIT"];
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
                        }
                    }
                }
                else if (request.AgencyId == "4")
                {
                    //Financial Requirements
                    FinancialRequirement.PlainAmount = mongoRecord["RELEASE ORDER FEES"].ToString();
                    FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(mongoRecord["RELEASE ORDER FEES"].ToString());
                }
                else if (request.AgencyId == "5")
                {
                    //Financial Requirements
                    if (request.IsFinancialRequirement)
                    {
                        var feeConfigurationList = Command.UnitOfWork.LPCOFeeConfigurationRepository.GetFeeConfig(
                            request.HsCode,
                            request.TradeTranTypeID,
                            Convert.ToInt32(request.AgencyId)
                        ).FirstOrDefault();

                        var feeConfig = new LPCOFeeCleanResp();
                        feeConfig.AdditionalAmount = feeConfigurationList.AdditionalAmount;
                        feeConfig.AdditionalAmountOn = feeConfigurationList.AdditionalAmountOn;
                        feeConfig.Rate = feeConfigurationList.Rate;
                        feeConfig.CalculationBasis = feeConfigurationList.CalculationBasis;
                        feeConfig.CalculationSource = feeConfigurationList.CalculationSource;
                        feeConfig.MinAmount = feeConfigurationList.MinAmount;
                        

                        if (psqcaFeeDict.ContainsKey(feeConfig))
                        {
                            psqcaFeeDict[feeConfig].Add(request);
                        }
                        else
                        {
                            var req = new List<GetDocumentRequirementRequest>();
                            req.Add(request);
                            psqcaFeeDict.Add(feeConfig, req);
                        }
                    }
                }
                else
                {
                    //Financial Requirements
                    FinancialRequirement.PlainAmount = mongoRecord["RO FEES"].ToString();
                    FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(mongoRecord["RO FEES"].ToString());
                }
            }

            //for PythoCertificate = EC
            // NO EC in Agency 4 - FSCRD
            else if (documentClassification == "EC")
            {
                if (request.IsFinancialRequirement)
                {
                    //Financial Requirements
                    if (request.AgencyId == "2")
                    {
                        FinancialRequirement.PlainAmount = mongoRecord["Phytosanitary certification Fee"].ToString();
                        FinancialRequirement.Amount = Command.CryptoAlgorithm.Encrypt(mongoRecord["Phytosanitary certification Fee"].ToString());
                        FinancialRequirement.PlainAmmendmentFee = mongoRecord["Phytosanitary  certification Amendmend/Re-issue Fee "].ToString();
                        FinancialRequirement.AmmendmentFee = Command.CryptoAlgorithm.Encrypt(mongoRecord["Phytosanitary  certification Amendmend/Re-issue Fee "].ToString());
                    }
                    else if (request.AgencyId == "3")
                    {
                        AQDECFeeCalculateRequestDTO calculateECFeeRequest = new AQDECFeeCalculateRequestDTO();
                        calculateECFeeRequest.AgencyId = Convert.ToInt32(request.AgencyId);
                        calculateECFeeRequest.HsCodeExt = request.HsCode;
                        calculateECFeeRequest.Quantity = Convert.ToInt32(request.Quantity);
                        calculateECFeeRequest.TradeTranTypeID = request.TradeTranTypeID;
                        FactorData factorData = request.FactorCodeValuePair["UNIT"];
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
                        }
                    }
                }
            }

            tarpRequirments.DocumentFinancialRequirement = FinancialRequirement;

            response.Model = tarpRequirments;
            Log.Information("Tarp Requirments Response: {@response}", response);
            Log.Information("[{0}.{1}] Ended", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return response;
        }
    }
}
