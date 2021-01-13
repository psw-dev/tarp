/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System.Collections.Generic;
using System.Data;
using Dapper;
using PSW.ITMS.Data.Entities;
using PSW.ITMS.Data.Repositories;

namespace PSW.ITMS.Data.Sql.Repositories
{
    public class CountryWithDialingCodeRepository : Repository<CountryWithDialingCodes>, ICountryWithDialingCodeRepository
    {
        #region public constructors

        public CountryWithDialingCodeRepository(IDbConnection context) : base(context)
        {
            TableName = "[DialingCode] dialingCode join Country country on  dialingCode.CountryCode =country.code ";
            PrimaryKeyName = "country.code";
        }

        public IEnumerable<CountryWithDialingCodes> GetCountryDialingCode()
        {
            return (IEnumerable<CountryWithDialingCodes>)_connection.Query<CountryWithDialingCodes>(string.Format("SELECT dialingCode.[CountryCode],dialingCode.[DialCode] , country.Name FROM {0}", TableName),
                                                        transaction: _transaction);
        }

        public override void Delete(int id)
        {
        }
        public override CountryWithDialingCodes Get(int id)
        {
            return null;
        }
        public override CountryWithDialingCodes Get(string id)
        {
            return null;
        }
        public override int Add(CountryWithDialingCodes _entity)
        {
            return 0;
        }
        public override int Count(string ColumnValue, string ColumnName)
        {
            return base.Count(ColumnValue, ColumnName);
        }
        public override void Delete(Entity _entity)
        {
            base.Delete(_entity);
        }
        public override CountryWithDialingCodes Find(int id)
        {
            return null;
        }
        public override bool Equals(object obj)
        {
            return false;
        }
        public override IEnumerable<CountryWithDialingCodes> Get()
        {
            return null;
        }
        public override List<CountryWithDialingCodes> GetPage(int pageNumber, int pageSize)
        {
            return null;
        }
        #endregion

        #region Public methods

        #endregion
    }
}
