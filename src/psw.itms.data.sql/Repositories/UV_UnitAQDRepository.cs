/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using PSW.ITMS.Data.Objects.Views;
using PSW.ITMS.Data.Repositories;
using System.Data;

namespace PSW.ITMS.Data.Sql.Repositories
{
    public class UV_UnitAQDRepository : Repository<uv_UnitAQD>, IUV_UnitAQDRepository
    {
        #region public constructors

        public UV_UnitAQDRepository(IDbConnection connection) : base(connection)
        {
            TableName = "[SHRD].[dbo].[uv_UnitAQD]";
            PrimaryKeyName = "ID";
        }

        #endregion

        #region Public methods

        #endregion
    }
}