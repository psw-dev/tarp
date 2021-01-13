/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using PSW.ITMS.Data.Entities;

namespace PSW.ITMS.Data.Repositories
{
	public interface IRepository<T>
    {
		#region Methods

        void Add(T _entity);
        void Delete(int id);
        void Delete(Entity _entity);
        Task<IEnumerable<T>> Get();
        T Get(string id);
        T Get(int id);
        List<T> GetPage(int pageNumber, int pageSize);
        List<T> Where(object propertyValues);
		int Update(Entity _entity);


		#endregion
    }
}
