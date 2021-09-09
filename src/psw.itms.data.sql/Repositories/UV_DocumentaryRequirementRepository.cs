/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using PSW.ITMS.Data.Objects.Views;
using PSW.ITMS.Data.Repositories;
using System.Data;

namespace PSW.ITMS.Data.Sql.Repositories
{
    public class UV_DocumentaryRequirementRepository : Repository<UV_DocumentaryRequirement>, IUV_DocumentaryRequirementRepository
    {
        #region public constructors

        public UV_DocumentaryRequirementRepository(IDbConnection connection) : base(connection)
        {
            TableName = "[uv_DocumentaryRequirement]";
            PrimaryKeyName = "ID";
        }

        #endregion

        #region Public methods

        #endregion
    }
}