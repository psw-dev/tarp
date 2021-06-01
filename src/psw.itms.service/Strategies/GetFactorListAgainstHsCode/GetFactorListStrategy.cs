using System.Collections.Generic;
using System.Linq;


using PSW.ITMS.Service.DTO;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Data.Entities;
using System;

namespace PSW.ITMS.Service.Strategies
{
    public class GetFactorListStrategy : ApiStrategy<GetFactorListAgainstHscodeRequest, GetFactorListAgainstHscodeResponse>
    {
        #region Constructors 
        public GetFactorListStrategy(CommandRequest request) : base(request)
        {
            this.Reply = new CommandReply();
        }
        #endregion 

        #region Distructors 
        ~GetFactorListStrategy()
        {

        }
        #endregion 

        #region Strategy Excecution  

        public override CommandReply Execute()
        {
            try
            {
                if (string.IsNullOrEmpty(RequestDTO.HsCode) || string.IsNullOrEmpty(RequestDTO.AgencyId) || string.IsNullOrEmpty(RequestDTO.documentTypeCode))
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

                if(TempHsCode == null)
                {
                    return BadRequestReply("Record for hscode does not exist");
                }

                Rule TempRule = Command.UnitOfWork.RuleRepository.Get(Convert.ToInt16(TempHsCode.RuleID));

                if(TempRule == null)
                {
                    return BadRequestReply("Record for rule against hscode does not exist");
                }

                List<long> FactorsApplicable = FactorsPresentInRule(TempRule);

                if(FactorsApplicable.Count == 0)
                {
                    return BadRequestReply("Factors data not available");
                }

                List<Factors> FactorsData = new List<Factors>();

                FactorsData = Command.UnitOfWork.FactorRepository.GetFactorsData(FactorsApplicable);

                if(FactorsData == null || FactorsData.Count == 0)
                {
                    return BadRequestReply("Factors Details not available");
                }

                ResponseDTO = new GetFactorListAgainstHscodeResponse
                {
                    FactorList = FactorsData
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

        public List<long> FactorsPresentInRule (Rule rule)
        {
            List<long> FactorIdList = new List<long>();

            if(rule.Factor1ID != 0)
            {
                FactorIdList.Add(rule.Factor1ID);
            }
            if(rule.Factor2ID != 0)
            {
                FactorIdList.Add(rule.Factor2ID);
            }
            if(rule.Factor3ID != 0)
            {
                FactorIdList.Add(rule.Factor3ID);
            }
            if(rule.Factor4ID != 0)
            {
                FactorIdList.Add(rule.Factor4ID);
            }
            if(rule.Factor5ID != 0)
            {
                FactorIdList.Add(rule.Factor5ID);
            }
            if(rule.Factor6ID != 0)
            {
                FactorIdList.Add(rule.Factor6ID);
            }
            if(rule.Factor7ID != 0)
            {
                FactorIdList.Add(rule.Factor7ID);
            }
            if(rule.Factor8ID != 0)
            {
                FactorIdList.Add(rule.Factor8ID);
            }
            if(rule.Factor9ID != 0)
            {
                FactorIdList.Add(rule.Factor9ID);
            }
            if(rule.Factor10ID != 0)
            {
                FactorIdList.Add(rule.Factor10ID);
            }
            if(rule.Factor11ID != 0)
            {
                FactorIdList.Add(rule.Factor11ID);
            }
            if(rule.Factor12ID != 0)
            {
                FactorIdList.Add(rule.Factor12ID);
            }
            if(rule.Factor13ID != 0)
            {
                FactorIdList.Add(rule.Factor13ID);
            }
            if(rule.Factor14ID != 0)
            {
                FactorIdList.Add(rule.Factor14ID);
            }
            if(rule.Factor15ID != 0)
            {
                FactorIdList.Add(rule.Factor15ID);
            }
            if(rule.Factor16ID != 0)
            {
                FactorIdList.Add(rule.Factor16ID);
            }
            if(rule.Factor17ID != 0)
            {
                FactorIdList.Add(rule.Factor17ID);
            }
            if(rule.Factor18ID != 0)
            {
                FactorIdList.Add(rule.Factor18ID);
            }
            if(rule.Factor19ID != 0)
            {
                FactorIdList.Add(rule.Factor19ID);
            }
            if(rule.Factor20ID != 0)
            {
                FactorIdList.Add(rule.Factor20ID);
            }

            return FactorIdList;
        }
    }
}
