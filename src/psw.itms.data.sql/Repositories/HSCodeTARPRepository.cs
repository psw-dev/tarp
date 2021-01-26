/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System.Data;
using PSW.ITMS.Data.Entities;
using PSW.ITMS.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using Dapper;



namespace PSW.ITMS.Data.Sql.Repositories
{
    public class HSCodeTARPRepository : Repository<HSCodeTARP>, IHSCodeTARPRepository
    {
		#region public constructors

        public HSCodeTARPRepository(IDbConnection context) : base(context)
        {
            TableName = "[dbo].[HSCodeTARP]";
			PrimaryKeyName = "ID";
        }

		#endregion

		#region Public methods
        public List<string> GetHSCode(string hsCode)
        {
            string hsCodeParameter="%"+hsCode+"%";
            return _connection.Query<string>(string.Format("SELECT HSCode FROM {0} WHERE HSCode LIKE '{1}'", TableName, hsCodeParameter),
                                        transaction: _transaction).ToList();
        }

		#endregion
    }
}
