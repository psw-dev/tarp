
using PSW.ITMS.Data.Entities;
using PSW.ITMS.Data.Repositories;
using System.Data;
using System.Linq;

namespace PSW.ITMS.Data.Sql.Repositories
{
    public class OGAItemCategoryRepository : Repository<OGAItemCategory>, IOGAItemCategoryRepository
    {
        #region public constructors

        public OGAItemCategoryRepository(IDbConnection context) : base(context)
        {
            TableName = "[dbo].[OGAItemCategory]";
            PrimaryKeyName = "ID";
        }

        #endregion

        #region Public methods
        #endregion
    }
}
