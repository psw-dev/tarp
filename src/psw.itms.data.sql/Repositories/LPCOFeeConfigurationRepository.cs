
using Dapper;
using PSW.ITMS.Data.Entities;
using PSW.ITMS.Data.Repositories;
using PSW.Lib.Logs;
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

        public List<LPCOFeeConfiguration> GetFeeConfig(string HSCodeExt, int TradeTranTypeID, int AgencyID)
        {
            var query = @"SELECT * FROM [TARP].[dbo].[LPCOFeeConfiguration]
                            WHERE AgencyID = @AGENCYID
                            AND TradeTranTypeID = @TRADETRANTYPEID
                            AND HSCodeExt = @HSCODEEXT
                            AND EffectiveFromDt <= GETDATE()
                            AND EffectiveThruDt > GETDATE()";

            return _connection.Query<LPCOFeeConfiguration>(
                    query,
                    param: new {
                        AGENCYID = AgencyID,
                        TRADETRANTYPEID = TradeTranTypeID,
                        HSCODEEXT = HSCodeExt
                        },
                    transaction: _transaction
                   ).ToList();
        }

        #endregion
    }
}
