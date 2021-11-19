/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using PSW.ITMS.Data.Entities;
using System.Collections.Generic;

namespace PSW.ITMS.Data.Repositories
{
    public interface IRepository<T> : IRepositoryTransaction
    {
        #region Methods

        int Add(T _entity);
        void Delete(int id);
        void Delete(Entity _entity);
        IEnumerable<T> Get();
        T Get(string id);
        T Get(int id);
        List<T> GetPage(int pageNumber, int pageSize);
        List<T> Where(object propertyValues);
        int Update(Entity _entity);

        T Find(int id);
        T Find(string id);
        int Count(string ColumnValue, string ColumnName);


        #endregion
    }
}