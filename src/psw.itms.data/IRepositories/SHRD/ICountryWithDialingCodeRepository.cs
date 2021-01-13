/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using PSW.ITMS.Data.Entities;

namespace PSW.ITMS.Data.Repositories
{
    public interface ICountryWithDialingCodeRepository : IRepository<CountryWithDialingCodes>
    {
        IEnumerable<CountryWithDialingCodes> GetCountryDialingCode();


    }
}
