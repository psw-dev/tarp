using System.Collections.Generic;
using System.Linq;


using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Data.Entities;
using System;
using MongoDB.Bson;
using PSW.ITMS.Service.MongoDB;
using PSW.Lib.Logs;

namespace PSW.ITMS.Service.Strategies
{
    public class GetRequirementMongoStrategy : ApiStrategy<GetDocumentRequirementRequest, GetDocumentRequirementResponse>
    {
        #region Constructors 
        public GetRequirementMongoStrategy(CommandRequest request) : base(request)
        {
            this.Reply = new CommandReply();
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

                RegulatedHSCode TempHsCode = Command.UnitOfWork.RegulatedHSCodeRepository.Where(
                    new
                    {
                        HSCodeExt = RequestDTO.HsCode,
                        AgencyID = RequestDTO.AgencyId,
                        RequiredDocumentTypeCode = RequestDTO.documentTypeCode
                    }
                    ).FirstOrDefault();

                Log.Information("|{0}|{1}| RegulatedHSCode DbRecord {@TempHsCode}", StrategyName, MethodID, TempHsCode);

                if (TempHsCode == null)
                {
                    return BadRequestReply("Record for hscode does not exist");
                }

                Rule TempRule = Command.UnitOfWork.RuleRepository.Get(Convert.ToInt16(TempHsCode.RuleID));

                if (TempRule == null)
                {
                    return BadRequestReply("Record for rule against hscode does not exist");
                }

                Log.Information("|{0}|{1}| Rule DbRecord {@TempRule}", StrategyName, MethodID, TempRule);

                List<long> FactorsIDAppliedTORule = GetFactorAppliedInRule(TempRule);
                
                if(FactorsIDAppliedTORule.Count == 0)
                {
                    return BadRequestReply("No factor found in rule");
                }

                Log.Information("|{0}|{1}| FactorID's Applied To Rule DbRecord {@FactorsIDAppliedTORule}", StrategyName, MethodID, FactorsIDAppliedTORule);

                List<Factors> FactorDataList = LoadFactorData(FactorsIDAppliedTORule);

                Log.Information("|{0}|{1}| FactorData DbRecord {@FactorDataList}", StrategyName, MethodID, FactorDataList);

                MongoDbRecordFetcher MDbRecordFetcher = new MongoDbRecordFetcher("TARP",TempHsCode.CollectionName);

                BsonDocument doc = MDbRecordFetcher.GetFilteredRecord(RequestDTO.HsCode, RequestDTO.FactorCodeValuePair["PURPOSE"].FactorValue);

                Log.Information("|{0}|{1}| Mongo Record fetched {@doc}", StrategyName, MethodID, doc);

                if(doc == null)
                {
                    return BadRequestReply("No record found for Hscode : " + RequestDTO.HsCode);
                }

                string RecordChecker = CheckFactorInMongoRecord(FactorDataList, doc, RequestDTO.FactorCodeValuePair);

                List<DocumentaryRequirement> TempDocumentaryRequirementList = new List<DocumentaryRequirement>();

                if(RecordChecker == "Checked")
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

        public List<long> GetFactorAppliedInRule(Rule tempRule)
        {
            List<long> FactorsIDAppliedInRule = new List<long>();

            if (tempRule.Factor1ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor1ID);
            if (tempRule.Factor2ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor2ID);
            if (tempRule.Factor3ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor3ID);
            if (tempRule.Factor4ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor4ID);
            if (tempRule.Factor5ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor5ID);
            if (tempRule.Factor6ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor6ID);
            if (tempRule.Factor7ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor7ID);
            if (tempRule.Factor8ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor8ID);
            if (tempRule.Factor9ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor9ID);
            if (tempRule.Factor10ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor10ID);
            if (tempRule.Factor11ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor11ID);
            if (tempRule.Factor12ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor12ID);
            if (tempRule.Factor13ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor13ID);
            if (tempRule.Factor14ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor14ID);
            if (tempRule.Factor15ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor15ID);
            if (tempRule.Factor16ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor16ID);
            if (tempRule.Factor17ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor17ID);
            if (tempRule.Factor18ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor18ID);
            if (tempRule.Factor19ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor19ID);
            if (tempRule.Factor20ID != 0) FactorsIDAppliedInRule.Add(tempRule.Factor20ID);

            return FactorsIDAppliedInRule;
        }

        public List<Factors> LoadFactorData(List<long> FactorIDAppliedInRule)
        {
            List<Factors> FactorDataList = new List<Factors>();

            FactorDataList = this.Command.UnitOfWork.FactorRepository.GetFactorsData(FactorIDAppliedInRule);

            return FactorDataList;
        }

        public string CheckFactorInMongoRecord(List<Factors> FactorDataList, BsonDocument mongoDoc, Dictionary<string, FactorData> FactorCodeValuePair)
        {
            int count = 0;
            foreach (var factor in FactorDataList)
            {
                if (FactorCodeValuePair.ContainsKey(factor.FactorCode))
                {
                    var ItemList = mongoDoc[factor.FactorCode].ToString().Split('|').ToList();

                    if (ItemList.Contains(FactorCodeValuePair[factor.FactorCode].FactorValue) || ItemList.Contains("ALL"))
                    {
                        count+=1;
                    }
                    else
                    {
                        return "Factor: " + factor.Label + " Value : " + FactorCodeValuePair[factor.FactorCode] + " does not match in record";
                    }
                }
                else
                {
                    return "Factors provided does not contain FactorData that is required in Rule " + factor.FactorCode +" not provided";
                }
            }

            if(count == FactorDataList.Count)
            {
                return "Checked";
            }
            else
            {
                return " ";
            }
        }

        public List<DocumentaryRequirement> GetRequirements (BsonDocument mongoRecord, string RequiredDocumentTypeCode)
        {
            List<DocumentaryRequirement> tarpRequirements = new List<DocumentaryRequirement>();

            if(RequiredDocumentTypeCode == "D12")
            {
                var ipDocRequirements = mongoRecord["IP DOCUMENTARY REQUIREMENTS"].ToString().Split('|').ToList();
                List<string> ipDocRequirementsTrimmed = new List<string>();

                foreach(var lpco in ipDocRequirements)
                {
                    ipDocRequirementsTrimmed.Add(lpco.Trim());
                }

                // ipDocRequirementsTrimmed.Remove("Application on DPP form 14 [ Rule 19 (1) of PQR 2019]");
                // ipDocRequirementsTrimmed.Remove("Fee Challan");

                //DocumentaryRequirements
                foreach(var doc in ipDocRequirementsTrimmed)
                {
                    DocumentaryRequirement tempReq = new DocumentaryRequirement();

                    tempReq.Name = doc + " For " + " DPP Import Permit"; //replace DPP with collectionName 
                    tempReq.DocumentName = doc;
                    tempReq.IsMandatory = true;
                    tempReq.RequirementType = "Documentary";

                    tempReq.DocumentTypeCode = this.Command.UnitOfWork.DocumentTypeRepository.Where(new {Name = doc}).FirstOrDefault().Code;
                    tempReq.AttachedObjectFormatID = 1;

                    tarpRequirements.Add(tempReq);
                }

                //Financial Requirements
                DocumentaryRequirement tempReqFinancial = new DocumentaryRequirement();

                tempReqFinancial.Name = "Fee Challan For DPP Import Permit"; //replace DPP with collectionName 
                tempReqFinancial.IsMandatory = true;
                tempReqFinancial.RequirementType = "Financial";

                tempReqFinancial.PostingBillingAccountID = "123"; //change afterward with proper billing account
                tempReqFinancial.Amount = mongoRecord["IP FEES"].ToInt64();

                tarpRequirements.Add(tempReqFinancial);

                //ValidityTerm Requirements
                DocumentaryRequirement tempReqValidityTerm = new DocumentaryRequirement();

                tempReqValidityTerm.Name = "Validity Period For DPP Import Permit"; //replace DPP with collectionName 
                tempReqValidityTerm.IsMandatory = true;
                tempReqValidityTerm.RequirementType = "Validity Period";
                tempReqValidityTerm.UomName = "Month";
                string uomPeriod = mongoRecord["IP VALIDITY"].ToString();
                tempReqValidityTerm.Quantity = Convert.ToInt32(uomPeriod.Substring(0,2));

                tarpRequirements.Add(tempReqValidityTerm);

            }
            //For RO 
            else if(RequiredDocumentTypeCode == "D03")
            {
                var roDocRequirements = mongoRecord["RO  DOCUMENTARY REQUIREMENTS"].ToString().Split('|').ToList();

                List<string> roDocRequirementsTrimmed = new List<string>();

                foreach(var lpco in roDocRequirements)
                {
                    var removespaces = lpco.Trim();
                    roDocRequirementsTrimmed.Add(removespaces.TrimEnd('\n'));
                }

                // roDocRequirementsTrimmed.Remove("Application on DPP prescribed form 20 [Rule 44(1) of PQR 2019]");
                // roDocRequirementsTrimmed.Remove("Fee Challan");

                //DocumentaryRequirements
                foreach(var doc in roDocRequirementsTrimmed)
                {
                    DocumentaryRequirement tempReq = new DocumentaryRequirement();

                    tempReq.Name = doc + " For " + " DPP Release Order"; //replace DPP with collectionName 
                    tempReq.DocumentName = doc;
                    tempReq.IsMandatory = true;
                    tempReq.RequirementType = "Documentary";

                    tempReq.DocumentTypeCode = this.Command.UnitOfWork.DocumentTypeRepository.Where(new {Name = doc}).FirstOrDefault().Code;
                    tempReq.AttachedObjectFormatID = 1;

                    tarpRequirements.Add(tempReq);
                }

                //Financial Requirements
                DocumentaryRequirement tempReqFinancial = new DocumentaryRequirement();

                tempReqFinancial.Name = "Fee Challan For DPP Release Order"; //replace DPP with collectionName 
                tempReqFinancial.IsMandatory = true;
                tempReqFinancial.RequirementType = "Financial";

                tempReqFinancial.PostingBillingAccountID = "123"; //change afterward with proper billing account
                tempReqFinancial.Amount = mongoRecord["RO FEES"].ToInt64();

                tarpRequirements.Add(tempReqFinancial);

            }

            return tarpRequirements;
        }
    }
}
