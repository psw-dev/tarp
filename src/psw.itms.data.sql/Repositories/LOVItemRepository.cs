/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using Dapper;
using PSW.ITMS.Data.Entities;
using PSW.ITMS.Data.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PSW.ITMS.Data.Sql.Repositories
{
    public class LOVItemRepository : Repository<LOVItem>, ILOVItemRepository
    {
        #region public constructors

        public LOVItemRepository(IDbConnection context) : base(context)
        {
            TableName = "[dbo].[LOVItem]";
            PrimaryKeyName = "ID";
        }

        #endregion

        #region Public methods

        public List<FactorLOVItems> GetLOVItems(int? LOVID)
        {
            return _connection.Query<FactorLOVItems>(string.Format("SELECT ItemKey, ItemValue, AltItemKey FROM LOVITEM WHERE LOVID = '{0}'", LOVID)).ToList();
        }

        public List<FactorLOVItems> GetLOVItems(string lovTableName, string lovColumnName)
        {
            return _connection.Query<FactorLOVItems>(string.Format("SELECT ID as ItemKey, {0} as ItemValue, * FROM [SHRD].[dbo].[{1}]", lovColumnName, lovTableName)).ToList();
        }

        #endregion
    }
}
