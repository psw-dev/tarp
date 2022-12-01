/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System.Collections.Generic;
using System.Data;
using Dapper;
using PSW.ITMS.Data.Entities;
using PSW.ITMS.Data.Repositories;

namespace PSW.ITMS.Data.Sql.Repositories
{
    public class DocumentToInitiateRepository : Repository<DocumentToInitiate>, IDocumentToInitiateRepository
    {
		#region public constructors

        public DocumentToInitiateRepository(IDbConnection context) : base(context)
        {
            TableName = "[dbo].[DocumentToInitiate]";
			PrimaryKeyName = "ID";
        }

		#endregion

		#region Public methods

        public List<DocumentToInitiate> GetActiveList(string hsCodeExt, string agencyId, int tradeTranTypeId, string documentTypeCode)
        {
            return _connection.Query<DocumentToInitiate>(string.Format("select * from DocumentToInitiate where HsCodeExt = '{0}' AND AgencyID = '{1}' AND TradeTranTypeID = '{2}' AND DocumentCode = '{3}' AND GETDATE() BETWEEN EffectiveFrmDt AND EffectiveThruDt",hsCodeExt, agencyId, tradeTranTypeId, documentTypeCode)).AsList();
        }

		#endregion
    }
}
