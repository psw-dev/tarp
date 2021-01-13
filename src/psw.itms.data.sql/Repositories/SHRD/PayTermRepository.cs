/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System.Data;

using PSW.ITMS.Data.Entities;
using PSW.ITMS.Data.Repositories;

namespace PSW.ITMS.Data.Sql.Repositories
{
    public class PayTermRepository : Repository<PayTerm>, IPayTermRepository
    {
		#region public constructors

        public PayTermRepository(IDbConnection context) : base(context)
        {
            TableName = "[dbo].[PayTerm]";
			PrimaryKeyName = "ID";
        }

		#endregion

		#region Public methods

		#endregion
    }
}
