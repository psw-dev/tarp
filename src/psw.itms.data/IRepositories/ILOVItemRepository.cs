/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using PSW.ITMS.Data.Entities;
using System.Collections.Generic;

namespace PSW.ITMS.Data.Repositories
{
    public interface ILOVItemRepository : IRepository<LOVItem>
    {
        List<FactorLOVItems> GetLOVItems(int? LOVID);
        List<FactorLOVItems> GetLOVItems(string lovTableName, string lovColumnName);
    }
}
