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

                var TempHsCode = Command.UnitOfWork.RegulatedHSCodeRepository.Where(
                    new
                    {
                        HSCodeExt = RequestDTO.HsCode,
                        AgencyID = RequestDTO.AgencyId,
                        RequiredDocumentTypeCode = RequestDTO.documentTypeCode,
                        TradeTranTypeID = RequestDTO.TradeTranTypeID
                    }
                    ).FirstOrDefault();

                Log.Information("|{0}|{1}| RegulatedHSCode DbRecord {@TempHsCode}", StrategyName, MethodID, TempHsCode);

                if (TempHsCode == null)
                {
                    return BadRequestReply("Record for hscode does not exist");
                }

                var TempRule = Command.UnitOfWork.RuleRepository.Get(Convert.ToInt16(TempHsCode.RuleID));

                if (TempRule == null)
                {
                    return BadRequestReply("Record for rule against hscode does not exist");
                }

                Log.Information("|{0}|{1}| Rule DbRecord {@TempRule}", StrategyName, MethodID, TempRule);

                //var FactorsIDAppliedTORule = GetFactorAppliedInRule(TempRule);
                var FactorsIDAppliedTORule = TempRule.GetFactorAppliedInRule();

                if (FactorsIDAppliedTORule.Count == 0)
                {
                    return BadRequestReply("No factor found in rule");
                }

                Log.Information("|{0}|{1}| FactorID's Applied To Rule DbRecord {@FactorsIDAppliedTORule}", StrategyName, MethodID, FactorsIDAppliedTORule);

                var FactorDataList = LoadFactorData(FactorsIDAppliedTORule);

                Log.Information("|{0}|{1}| FactorData DbRecord {@FactorDataList}", StrategyName, MethodID, FactorDataList);

                var doc = new BsonDocument();

                MongoDbRecordFetcher MDbRecordFetcher;

                try
                {
                    MDbRecordFetcher = new MongoDbRecordFetcher("TARP", TempHsCode.CollectionName);
                }
                catch (SystemException ex)
                {
                    Log.Error("|{0}|{1}| Error occured in connecting to MongoDB {@ex}", StrategyName, MethodID, ex.ToString());

                    return BadRequestReply("Error occured in connecting to MongoDB");
                }

                try
                {
                    //MongoDbRecordFetcher MDbRecordFetcher = new MongoDbRecordFetcher("TARP", TempHsCode.CollectionName);
                    doc = MDbRecordFetcher.GetFilteredRecord(RequestDTO.HsCode, RequestDTO.FactorCodeValuePair["PURPOSE"].FactorValue);
                }
                catch (SystemException ex)
                {
                    Log.Error("|{0}|{1}| Error occured in fetching record from MongoDB {@ex}", StrategyName, MethodID, ex.ToString());

                    return BadRequestReply("Error occured in fetching record from MongoDB");
                }

                Log.Information("|{0}|{1}| Mongo Record fetched {@doc}", StrategyName, MethodID, doc);

                if (doc == null)
                {
                    return BadRequestReply("No record found for Hscode : " + RequestDTO.HsCode);
                }

                var RecordChecker = CheckFactorInMongoRecord(FactorDataList, doc, RequestDTO.FactorCodeValuePair);

                var TempDocumentaryRequirementList = new List<DocumentaryRequirement>();

                if (RecordChecker == "Checked")
                {
                    TempDocumentaryRequirementList = GetRequirements(doc, RequestDTO.documentTypeCode);

                    Log.Information("|{0}|{1}| Documentary Requirements {@TempDocumentaryRequirementList}", StrategyName, MethodID, TempDocumentaryRequirementList);
                }
                else
                {
                    return BadRequestReply(RecordChecker);
                }

                ResponseDTO = new GetDocumentRequirementResponse
                {
                    DocumentaryRequirementList = TempDocumentaryRequirementList
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

        

        private List<Factors> LoadFactorData(List<long> FactorIDAppliedInRule)
        {
            var FactorDataList = new List<Factors>();

            FactorDataList = Command.UnitOfWork.FactorRepository.GetFactorsData(FactorIDAppliedInRule);

            return FactorDataList;
        }

        public string CheckFactorInMongoRecord(List<Factors> FactorDataList, BsonDocument mongoDoc, Dictionary<string, FactorData> FactorCodeValuePair)
        {
            var count = 0;
            foreach (var factor in FactorDataList)
            {
                if (FactorCodeValuePair.ContainsKey(factor.FactorCode))
                {
                    var ItemList = mongoDoc[factor.FactorCode].ToString().Split('|').ToList();

                    if (ItemList.Contains(FactorCodeValuePair[factor.FactorCode].FactorValue) || ItemList.Contains("ALL"))
                    {
                        count += 1;
                    }
                    else
                    {
                        return $"Factor: {factor.Label} Value : {FactorCodeValuePair[factor.FactorCode]} does not match in record";
                    }
                }
                else
                {
                    return $"Factors provided does not contain FactorData that is required in Rule {factor.FactorCode} not provided";
                }
            }

            if (count == FactorDataList.Count)
            {
                return "Checked";
            }
            else
            {
                return " ";
            }
        }

        public List<DocumentaryRequirement> GetRequirements(BsonDocument mongoRecord, string RequiredDocumentTypeCode)
        {
            var tarpRequirements = new List<DocumentaryRequirement>();

            if (RequiredDocumentTypeCode == "D12")
            {
                var ipDocRequirements = mongoRecord["IP DOCUMENTARY REQUIREMENTS"].ToString().Split('|').ToList();
                var ipDocRequirementsTrimmed = new List<string>();

                foreach (var lpco in ipDocRequirements)
                {
                    ipDocRequirementsTrimmed.Add(lpco.Trim());
                }

                // ipDocRequirementsTrimmed.Remove("Application on DPP form 14 [ Rule 19 (1) of PQR 2019]");
                // ipDocRequirementsTrimmed.Remove("Fee Challan");

                //DocumentaryRequirements
                foreach (var doc in ipDocRequirementsTrimmed)
                {
                    var tempReq = new DocumentaryRequirement();

                    tempReq.Name = doc + " For " + " DPP Import Permit"; //replace DPP with collectionName 
                    tempReq.DocumentName = doc;
                    tempReq.IsMandatory = true;
                    tempReq.RequirementType = "Documentary";

                    tempReq.DocumentTypeCode = Command.UnitOfWork.DocumentTypeRepository.Where(new { Name = doc }).FirstOrDefault()?.Code;
                    tempReq.AttachedObjectFormatID = 1;

                    tarpRequirements.Add(tempReq);
                }

                //Financial Requirements
                var tempReqFinancial = new DocumentaryRequirement();

                tempReqFinancial.Name = "Fee Challan For DPP Import Permit"; //replace DPP with collectionName 
                tempReqFinancial.IsMandatory = true;
                tempReqFinancial.RequirementType = "Financial";

                tempReqFinancial.PostingBillingAccountID = "123"; //change afterward with proper billing account
                tempReqFinancial.Amount = PSWEncryption.encrypt(mongoRecord["IP FEES"].ToString());

                tarpRequirements.Add(tempReqFinancial);

                //ValidityTerm Requirements
                var tempReqValidityTerm = new DocumentaryRequirement();

                tempReqValidityTerm.Name = "Validity Period For DPP Import Permit"; //replace DPP with collectionName 
                tempReqValidityTerm.IsMandatory = true;
                tempReqValidityTerm.RequirementType = "Validity Period";
                tempReqValidityTerm.UomName = "Month";
                var uomPeriod = mongoRecord["IP VALIDITY"].ToString();
                tempReqValidityTerm.Quantity = Convert.ToInt32(uomPeriod.Substring(0, 2));

                tarpRequirements.Add(tempReqValidityTerm);

            }
            //For RO 
            else if (RequiredDocumentTypeCode == "D03")
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

                    tarpRequirements.Add(tempReq);
                }

                //Financial Requirements
                var tempReqFinancial = new DocumentaryRequirement();

                tempReqFinancial.Name = "Fee Challan For DPP Release Order"; //replace DPP with collectionName 
                tempReqFinancial.IsMandatory = true;
                tempReqFinancial.RequirementType = "Financial";

                tempReqFinancial.PostingBillingAccountID = "123"; //change afterward with proper billing account
                tempReqFinancial.Amount = PSWEncryption.encrypt(mongoRecord["RO FEES"].ToString());

                tarpRequirements.Add(tempReqFinancial);

            }
            //For Phythosanitary Certificate
            else if (RequiredDocumentTypeCode == "D15")
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

                    tempReq.Name = doc + " For " + " Phythosanitary Certificate";
                    tempReq.DocumentName = doc;
                    tempReq.IsMandatory = true;
                    tempReq.RequirementType = "Documentary";

                    tempReq.DocumentTypeCode = Command.UnitOfWork.DocumentTypeRepository.Where(new { Name = doc }).FirstOrDefault()?.Code;
                    tempReq.AttachedObjectFormatID = 1;

                    tarpRequirements.Add(tempReq);
                }

                //Financial Requirements
                var tempReqFinancial = new DocumentaryRequirement();

                tempReqFinancial.Name = "Fee Challan For Phythosanitary Certificate";
                tempReqFinancial.IsMandatory = true;
                tempReqFinancial.RequirementType = "Financial";

                tempReqFinancial.PostingBillingAccountID = "123"; //change afterward with proper billing account
                tempReqFinancial.Amount = PSWEncryption.encrypt(mongoRecord["PHYTOSANITARY  FEES"].ToString());

                tarpRequirements.Add(tempReqFinancial);

            }

            return tarpRequirements;
        }
    }
}
