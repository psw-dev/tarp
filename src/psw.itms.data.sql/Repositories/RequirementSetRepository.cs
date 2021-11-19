/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using PSW.ITMS.Data.Entities;
using PSW.ITMS.Data.Repositories;
using System.Data;

namespace PSW.ITMS.Data.Sql.Repositories
{
    public class RequirementSetRepository : Repository<RequirementSet>, IRequirementSetRepository
    {
        #region public constructors

        public RequirementSetRepository(IDbConnection context) : base(context)
        {
            TableName = "[dbo].[RequirementSet]";
            PrimaryKeyName = "ID";
        }

        #endregion

        #region Public methods

        #endregion
    }
}
