/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System.Data;

using PSW.ITMS.Data.Entities;
using PSW.ITMS.Data.Repositories;

namespace PSW.ITMS.Data.Sql.Repositories
{
    public class Ref_UnitsRepository : Repository<Ref_Units>, IRef_UnitsRepository
    {
		#region public constructors

        public Ref_UnitsRepository(IDbConnection context) : base(context)
        {
            TableName = "[SHRD].[dbo].[Ref_Units]";
			PrimaryKeyName = "Unit_ID";
        }

		#endregion

		#region Public methods

		#endregion
    }
}
