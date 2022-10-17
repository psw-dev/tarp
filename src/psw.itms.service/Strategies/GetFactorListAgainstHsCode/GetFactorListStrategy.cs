using PSW.ITMS.Data.Entities;
using PSW.ITMS.Service.Command;
using PSW.ITMS.Service.DTO;
using PSW.Lib.Logs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSW.ITMS.Service.Strategies
{
    public class GetFactorListStrategy : ApiStrategy<GetFactorListAgainstHscodeRequest, GetFactorListAgainstHscodeResponse>
    {
        #region Constructors 
        public GetFactorListStrategy(CommandRequest request) : base(request)
        {
            Reply = new CommandReply();
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
                Log.Information("|{0}|{1}| Request DTO {@RequestDTO}", StrategyName, MethodID, RequestDTO);
                if (string.IsNullOrEmpty(RequestDTO.HsCode) || string.IsNullOrEmpty(RequestDTO.AgencyId) || string.IsNullOrEmpty(RequestDTO.documentTypeCode))
                {
                    return BadRequestReply("Please provide valid request parameters");
                }

                var tempHsCode = Command.UnitOfWork.RegulatedHSCodeRepository.GetActiveHsCode(
                    RequestDTO.HsCode,
                    RequestDTO.AgencyId.ToString(),
                    RequestDTO.TradeTranTypeID,
                    RequestDTO.documentTypeCode
                );

                if (tempHsCode == null)
                {
                    return BadRequestReply("Record for hscode does not exist");
                }

                Log.Information("|{0}|{1}| RegulatedHscode record fetched from db: {@tempHsCode}", StrategyName, MethodID, tempHsCode);

                var tempRule = Command.UnitOfWork.RuleRepository.Get(Convert.ToInt16(tempHsCode.RuleID));

                if (tempRule == null)
                {
                    return BadRequestReply("Record for rule against hscode does not exist");
                }

                Log.Information("|{0}|{1}| Rule record fetched from db: {@tempRule}", StrategyName, MethodID, tempRule);

                var factorsApplicable = FactorsPresentInRule(tempRule);
                var factorsData = new List<Factors>();

                Log.Information("|{0}|{1}| Applicable factor ID applied in rule : {@factorsApplicable}", StrategyName, MethodID, factorsApplicable);

                if (factorsApplicable.Count > 0)
                {
                    factorsData = Command.UnitOfWork.FactorRepository.GetFactorsData(factorsApplicable);

                    Log.Information("|{0}|{1}| Factor data fetched from rule : {@factorsData}", StrategyName, MethodID, factorsData);
    
                    if (factorsData == null || factorsData.Count == 0)
                    {
                        return BadRequestReply("Factors Details not available");
                    }
                }

                ResponseDTO = new GetFactorListAgainstHscodeResponse
                {
                    FactorList = factorsData
                };

                Log.Information("|{0}|{1}| Get factorList responseDTO : {@ResponseDTO}", StrategyName, MethodID, ResponseDTO);

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

        public List<long> FactorsPresentInRule(Rule rule)
        {
            var factorIdList = new List<long>();

            if (rule.Factor1ID != 0)
            {
                factorIdList.Add(rule.Factor1ID);
            }
            if (rule.Factor2ID != 0)
            {
                factorIdList.Add(rule.Factor2ID);
            }
            if (rule.Factor3ID != 0)
            {
                factorIdList.Add(rule.Factor3ID);
            }
            if (rule.Factor4ID != 0)
            {
                factorIdList.Add(rule.Factor4ID);
            }
            if (rule.Factor5ID != 0)
            {
                factorIdList.Add(rule.Factor5ID);
            }
            if (rule.Factor6ID != 0)
            {
                factorIdList.Add(rule.Factor6ID);
            }
            if (rule.Factor7ID != 0)
            {
                factorIdList.Add(rule.Factor7ID);
            }
            if (rule.Factor8ID != 0)
            {
                factorIdList.Add(rule.Factor8ID);
            }
            if (rule.Factor9ID != 0)
            {
                factorIdList.Add(rule.Factor9ID);
            }
            if (rule.Factor10ID != 0)
            {
                factorIdList.Add(rule.Factor10ID);
            }
            if (rule.Factor11ID != 0)
            {
                factorIdList.Add(rule.Factor11ID);
            }
            if (rule.Factor12ID != 0)
            {
                factorIdList.Add(rule.Factor12ID);
            }
            if (rule.Factor13ID != 0)
            {
                factorIdList.Add(rule.Factor13ID);
            }
            if (rule.Factor14ID != 0)
            {
                factorIdList.Add(rule.Factor14ID);
            }
            if (rule.Factor15ID != 0)
            {
                factorIdList.Add(rule.Factor15ID);
            }
            if (rule.Factor16ID != 0)
            {
                factorIdList.Add(rule.Factor16ID);
            }
            if (rule.Factor17ID != 0)
            {
                factorIdList.Add(rule.Factor17ID);
            }
            if (rule.Factor18ID != 0)
            {
                factorIdList.Add(rule.Factor18ID);
            }
            if (rule.Factor19ID != 0)
            {
                factorIdList.Add(rule.Factor19ID);
            }
            if (rule.Factor20ID != 0)
            {
                factorIdList.Add(rule.Factor20ID);
            }

            return factorIdList;
        }
    }
}
