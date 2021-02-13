/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System.Data;
using PSW.ITMS.Data.Entities;
using PSW.ITMS.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Text;

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
        public List<string> GetHSCode(object propertyValues)
        {

            const string query = "SELECT DISTINCT HSCode FROM {0} WHERE {1}";

            var whereBulder = new StringBuilder();
            var objectType = propertyValues.GetType();
            var first = true;
            foreach (var property in objectType.GetProperties())
            {
                whereBulder.AppendFormat("{2} {0} = '{1}'", property.Name, property.GetValue(propertyValues).ToString().Replace("'", "''"), first ? "" : "AND");
                first = false;
            }
            return _connection.Query<string>(string.Format(query, TableName, whereBulder),
                                        transaction: _transaction).ToList();
            // return _connection.Query<string>(string.Format("SELECT DISTINCT HSCode FROM {0}", TableName),
            //                             transaction: _transaction).ToList();
        }

        public List<HSCodeTARP> SearchHSCodes(object searchFilters)
        {
            string query = "SELECT * FROM {0} WHERE {1}";

            var searchCriteria = new StringBuilder();
            var objectType = searchFilters.GetType();
            var first = true;
            foreach (var property in objectType.GetProperties())
            {
                searchCriteria.AppendFormat("{2} {0} like '%{1}%'", property.Name, property.GetValue(searchFilters).ToString().Replace("'", "''"), first ? "" : "OR");
                first = false;
            }

            query = string.Format(query, TableName, searchCriteria);
            return _connection.Query<HSCodeTARP>(query,
                                        transaction: _transaction).ToList();
        }

		#endregion
    }
}
