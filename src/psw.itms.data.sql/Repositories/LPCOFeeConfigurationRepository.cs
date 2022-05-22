
using Dapper;
using PSW.ITMS.Data.Entities;
using PSW.ITMS.Data.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PSW.ITMS.Data.Sql.Repositories
{
    public class LPCOFeeConfigurationRepository : Repository<LPCOFeeConfiguration>, ILPCOFeeConfigurationRepository
    {
        #region public constructors

        public LPCOFeeConfigurationRepository(IDbConnection context) : base(context)
        {
            TableName = "[dbo].[LPCOFeeConfiguration]";
            PrimaryKeyName = "ID";
        }

        #endregion

        #region Public methods
        #endregion
    }
}
