
using PSW.ITMS.Data.Entities;

using System.Collections.Generic;

namespace PSW.ITMS.Data.Repositories
{
    public interface ILPCOFeeConfigurationRepository : IRepository<LPCOFeeConfiguration>
    {
        List<LPCOFeeConfiguration> GetFeeConfig(string HSCodeExt, int TradeTranTypeID, int AgencyID);
    }
}
