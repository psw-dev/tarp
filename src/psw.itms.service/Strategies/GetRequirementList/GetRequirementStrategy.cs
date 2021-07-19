using System.Collections.Generic;
using System.Linq;


using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Data.Entities;
using System;
using MongoDB.Bson;
using PSW.ITMS.Service.MongoDB;

namespace PSW.ITMS.Service.Strategies
{
    public class GetRequirementStrategy : ApiStrategy<GetDocumentRequirementRequest, GetDocumentRequirementResponse>
    {
        #region Constructors 
        public GetRequirementStrategy(CommandRequest request) : base(request)
        {
            this.Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~GetRequirementStrategy()
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

                RegulatedHSCode TempHsCode = Command.UnitOfWork.RegulatedHSCodeRepository.Where(
                    new
                    {
                        HSCodeExt = RequestDTO.HsCode,
                        AgencyID = RequestDTO.AgencyId,
                        RequiredDocumentTypeCode = RequestDTO.documentTypeCode
                    }
                    ).FirstOrDefault();

                if (TempHsCode == null)
                {
                    return BadRequestReply("Record for hscode does not exist");
                }

                Rule TempRule = Command.UnitOfWork.RuleRepository.Get(Convert.ToInt16(TempHsCode.RuleID));

                if (TempRule == null)
                {
                    return BadRequestReply("Record for rule against hscode does not exist");
                }


                DecisionMatrix TempDecisionMatrix = GetDecisionMatrixBaseOnFactors(RequestDTO.FactorCodeValuePair, TempRule);

                if (TempDecisionMatrix == null)
                {
                    return BadRequestReply("Record for decsionMatrix against hscode does not exist");
                }

                List<DocumentaryRequirement> TempDocumentaryRequirementList = GetRequirements(TempDecisionMatrix.RequirementSetID.ToString());

                if (TempDocumentaryRequirementList == null)
                {
                    return BadRequestReply("Record for documentary requirements against hscode does not exist");
                }

                ResponseDTO = new GetDocumentRequirementResponse
                {
                    DocumentaryRequirementList = TempDocumentaryRequirementList
                };


                // Send Command Reply 
                return OKReply();
            }
            catch (Exception ex)
            {
                return InternalServerErrorReply(ex);
            }
        }
        #endregion 

        public DecisionMatrix GetDecisionMatrixBaseOnFactors(Dictionary<string, FactorData> factorCodeValuePairs, Rule tempRule)
        {
            string F1Value = tempRule.Factor1ID != 0 ? GetFactorValue(tempRule.Factor1ID.ToString(), factorCodeValuePairs) : null;
            string F2Value = tempRule.Factor2ID != 0 ? GetFactorValue(tempRule.Factor2ID.ToString(), factorCodeValuePairs) : null;
            string F3Value = tempRule.Factor3ID != 0 ? GetFactorValue(tempRule.Factor3ID.ToString(), factorCodeValuePairs) : null;
            string F4Value = tempRule.Factor4ID != 0 ? GetFactorValue(tempRule.Factor4ID.ToString(), factorCodeValuePairs) : null;
            string F5Value = tempRule.Factor5ID != 0 ? GetFactorValue(tempRule.Factor5ID.ToString(), factorCodeValuePairs) : null;
            string F6Value = tempRule.Factor6ID != 0 ? GetFactorValue(tempRule.Factor6ID.ToString(), factorCodeValuePairs) : null;
            string F7Value = tempRule.Factor7ID != 0 ? GetFactorValue(tempRule.Factor7ID.ToString(), factorCodeValuePairs) : null;
            string F8Value = tempRule.Factor8ID != 0 ? GetFactorValue(tempRule.Factor8ID.ToString(), factorCodeValuePairs) : null;
            string F9Value = tempRule.Factor9ID != 0 ? GetFactorValue(tempRule.Factor9ID.ToString(), factorCodeValuePairs) : null;
            string F10Value = tempRule.Factor10ID != 0 ? GetFactorValue(tempRule.Factor10ID.ToString(), factorCodeValuePairs) : null;
            string F11Value = tempRule.Factor11ID != 0 ? GetFactorValue(tempRule.Factor11ID.ToString(), factorCodeValuePairs) : null;
            string F12Value = tempRule.Factor12ID != 0 ? GetFactorValue(tempRule.Factor12ID.ToString(), factorCodeValuePairs) : null;
            string F13Value = tempRule.Factor13ID != 0 ? GetFactorValue(tempRule.Factor13ID.ToString(), factorCodeValuePairs) : null;
            string F14Value = tempRule.Factor14ID != 0 ? GetFactorValue(tempRule.Factor14ID.ToString(), factorCodeValuePairs) : null;
            string F15Value = tempRule.Factor15ID != 0 ? GetFactorValue(tempRule.Factor15ID.ToString(), factorCodeValuePairs) : null;
            string F16Value = tempRule.Factor16ID != 0 ? GetFactorValue(tempRule.Factor16ID.ToString(), factorCodeValuePairs) : null;
            string F17Value = tempRule.Factor17ID != 0 ? GetFactorValue(tempRule.Factor17ID.ToString(), factorCodeValuePairs) : null;
            string F18Value = tempRule.Factor18ID != 0 ? GetFactorValue(tempRule.Factor18ID.ToString(), factorCodeValuePairs) : null;
            string F19Value = tempRule.Factor19ID != 0 ? GetFactorValue(tempRule.Factor19ID.ToString(), factorCodeValuePairs) : null;
            string F20Value = tempRule.Factor20ID != 0 ? GetFactorValue(tempRule.Factor20ID.ToString(), factorCodeValuePairs) : null;

            var DecisionMatrix = this.Command.UnitOfWork.DecisionMatrixRepository.Where(
                new
                {
                    RuleID = tempRule.ID,
                    Factor1Value = F1Value,
                    Factor2Value = F2Value,
                    Factor3Value = F3Value,
                    Factor4Value = F4Value,
                    Factor5Value = F5Value,
                    Factor6Value = F6Value,
                    Factor7Value = F7Value,
                    Factor8Value = F8Value,
                    Factor9Value = F9Value,
                    Factor10Value = F10Value,
                    Factor11Value = F11Value,
                    Factor12Value = F12Value,
                    Factor13Value = F13Value,
                    Factor14Value = F14Value,
                    Factor15Value = F15Value,
                    Factor16Value = F16Value,
                    Factor17Value = F17Value,
                    Factor18Value = F18Value,
                    Factor19Value = F19Value,
                    Factor20Value = F20Value
                }
            ).FirstOrDefault();

            return DecisionMatrix;
        }

        public string GetFactorValue(string FactorID, Dictionary<string, FactorData> factorCodeValuePairs)
        {
            string FactorValue = "";

            var factor = this.Command.UnitOfWork.FactorRepository.Get(FactorID);

            if (factorCodeValuePairs.ContainsKey(factor.FactorCode))
            {
                FactorValue = factorCodeValuePairs[factor.FactorCode].FactorValue;
            }
            else
            {
                FactorValue = null;
            }

            return FactorValue;
        }

        public List<DocumentaryRequirement> GetRequirements(string requirementSetID)
        {
            List<Requirement> RequirementList = this.Command.UnitOfWork.RequirementRepository.Where(
                new
                {
                    RequirementSetID = requirementSetID
                }
            );

            List<DocumentaryRequirement> tempDocumentaryRequirementList = new List<DocumentaryRequirement>();

            foreach (var requirement in RequirementList)
            {
                DocumentaryRequirement tempRequirement = new DocumentaryRequirement();

                tempRequirement.Name = requirement.Name;
                tempRequirement.IsMandatory = requirement.IsMandatory;
                tempRequirement.RequirementType = this.Command.UnitOfWork.RequirementCategoryRepository.Get(requirement.RequirementCategoryID).Name;

                if (tempRequirement.RequirementType == "Financial")
                {
                    var FinancialRequirement = this.Command.UnitOfWork.FinancialRequirementRepository.Where
                    (
                        new
                        {
                            RequirementID = requirement.ID
                        }
                    ).FirstOrDefault();

                    tempRequirement.PostingBillingAccountID = FinancialRequirement.PostingBillingAccountID.ToString();
                    tempRequirement.Amount = FinancialRequirement.Amount;
                }

                else if (tempRequirement.RequirementType == "Documentary")
                {
                    var DocumentaryRequirement = this.Command.UnitOfWork.DocumentRequirementRepository.Where
                    (
                        new
                        {
                            RequirementID = requirement.ID
                        }
                    ).FirstOrDefault();

                    tempRequirement.DocumentTypeCode = DocumentaryRequirement.DocumentTypeCode;
                    tempRequirement.DocumentName = this.Command.UnitOfWork.DocumentTypeRepository.Get(tempRequirement.DocumentTypeCode).Name;
                    tempRequirement.AttachedObjectFormatID = DocumentaryRequirement.AttachedObjectFormatID;
                }

                else if (tempRequirement.RequirementType == "Validity Period")
                {
                    var ValidityRequirement = this.Command.UnitOfWork.ValidityTermRequirementRepository.Where
                    (
                        new
                        {
                            RequirementID = requirement.ID
                        }
                    ).FirstOrDefault();

                    tempRequirement.UomName = this.Command.UnitOfWork.TermUoMRepository.Get(ValidityRequirement.TermUoMID).Name;
                    tempRequirement.Quantity = ValidityRequirement.Quantity;
                }

                else if (tempRequirement.RequirementType == "Testing")
                {
                    var TestingRequirement = this.Command.UnitOfWork.TestingRequirementRepository.Where
                    (
                        new
                        {
                            RequirementID = requirement.ID
                        }
                    ).FirstOrDefault();

                    tempRequirement.TestID = TestingRequirement.TestID.ToString();
                }

                else if (tempRequirement.RequirementType == "Nil Requirement")
                {
                    var NilRequirement = this.Command.UnitOfWork.NilRequirementRepository.Where
                    (
                        new
                        {
                            RequirementID = requirement.ID
                        }
                    ).FirstOrDefault();

                    tempRequirement.DisplayText = NilRequirement.DisplayText;
                }

                else if (tempRequirement.RequirementType == "Refusal Info")
                {
                    //Wadeed: to verify db table from Rehan
                }

                tempDocumentaryRequirementList.Add(tempRequirement);
            }

            return tempDocumentaryRequirementList;
        }
    }
}
