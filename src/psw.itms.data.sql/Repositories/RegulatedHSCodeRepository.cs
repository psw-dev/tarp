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
    public class RegulatedHSCodeRepository : Repository<RegulatedHSCode>, IRegulatedHSCodeRepository
    {
		#region public constructors

        public RegulatedHSCodeRepository(IDbConnection context) : base(context)
        {
            TableName = "[dbo].[RegulatedHSCode]";
			PrimaryKeyName = "ID";
        }

		#endregion

		#region Public methods

        public List<AgencyList> GetAgencyListAgainstHscode (string hscode)
        {
            return _connection.Query<AgencyList>(string.Format("SELECT A.ID, A.NAME FROM REGULATEDHSCODE RHS INNER JOIN SHRD.DBO.AGENCY A ON RHS.AGENCYID = A.ID WHERE HSCODE = '{0}'",hscode)).ToList(); 
        }

		#endregion
    }
}
