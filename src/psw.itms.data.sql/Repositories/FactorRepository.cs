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
    public class FactorRepository : Repository<Factor>, IFactorRepository
    {
		#region public constructors

        public FactorRepository(IDbConnection context) : base(context)
        {
            TableName = "[dbo].[Factor]";
			PrimaryKeyName = "ID";
        }

		#endregion

		#region Public methods

        public List<Factors> GetFactorsData(List<long> FactorIdList)
        {
            string Factorstring = "";

            foreach(var item in FactorIdList)
            {
                Factorstring = Factorstring + item + ",";
            }

            Factorstring = Factorstring.Substring(0,Factorstring.Length - 1);

            return _connection.Query<Factors>(string.Format("SELECT ID, LABEL, DESCRIPTION, ISLOV, FACTORCODE FROM FACTOR WHERE ID IN ({0})",Factorstring)).ToList();
        }

		#endregion
    }
}
