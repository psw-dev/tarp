/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using PSW.ITMS.Data.Entities;
using PSW.ITMS.Data.Repositories;
using System.Data;

namespace PSW.ITMS.Data.Sql.Repositories
{
    public class LogicOperatorRepository : Repository<LogicOperator>, ILogicOperatorRepository
    {
        #region public constructors

        public LogicOperatorRepository(IDbConnection context) : base(context)
        {
            TableName = "[dbo].[LogicOperator]";
            PrimaryKeyName = "ID";
        }

        #endregion

        #region Public methods

        #endregion
    }
}
