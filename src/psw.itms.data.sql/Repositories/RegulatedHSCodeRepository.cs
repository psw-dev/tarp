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

        public List<AgencyList> GetAgencyListAgainstHscode(string hscode, string documentCode)
        {
            return _connection.Query<AgencyList>(string.Format("SELECT A.ID, A.NAME FROM REGULATEDHSCODE RHS INNER JOIN SHRD.DBO.AGENCY A ON RHS.AGENCYID = A.ID WHERE HSCODEEXT = '{0}' AND RequiredDocumentTypeCode = '{1}'",hscode, documentCode)).ToList(); 
        }

        public List<ViewRegulatedHsCode> GetRegulatedHsCodeList()
        {
            return _connection.Query<ViewRegulatedHsCode>(string.Format("SELECT DISTINCT HsCode, ItemDescription, ItemDescriptionExt, AgencyID, TechnicalName FROM REGULATEDHSCODE")).ToList();
        }

        public List<ViewRegulatedHsCode> GetRegulatedHsCodeList(string agencyId)
        {
            return _connection.Query<ViewRegulatedHsCode>(string.Format("SELECT DISTINCT HsCode, ItemDescription, ItemDescriptionExt, AgencyID, TechnicalName FROM REGULATEDHSCODE WHERE AGENCYID = '{0}'",agencyId)).ToList();
        }

        public List<string> GetPCTCodeList(string hscode)
        {
            return _connection.Query<string>(string.Format("SELECT DISTINCT ProductCode FROM REGULATEDHSCODE WHERE HSCODE = '{0}'",hscode)).ToList(); 
        }

		#endregion
    }
}
