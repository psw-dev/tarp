/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System.Data;
using System.Collections.Generic;
using Dapper;
using System.Threading.Tasks;
using System.Linq;

using PSW.ITMS.Data.Entities;
using PSW.ITMS.Data.Repositories;

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

        public  List<FactorLOVItems> GetLOVItems(int FactorID)
        {
            return _connection.Query<FactorLOVItems>(string.Format("SELECT ItemKey, ItemValue, AltItemKey FROM LOVITEM LI INNER JOIN LOV L on LI.LOVID = L.ID  WHERE L.ID = (SELECT LOVID FROM FACTOR WHERE ID = '{0}' AND ISLOV = 1)",FactorID)).ToList(); 
        }

		#endregion
    }
}
