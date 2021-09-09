/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using PSW.ITMS.Data.Entities;
using System.Collections.Generic;

namespace PSW.ITMS.Data.Repositories
{
    public interface IRegulatedHSCodeRepository : IRepository<RegulatedHSCode>
    {
        List<AgencyList> GetAgencyListAgainstHscode(string hscode, string documentCode);
        List<ViewRegulatedHsCode> GetRegulatedHsCodeList();
        List<ViewRegulatedHsCode> GetRegulatedHsCodeList(int agencyId);
        List<ViewRegulatedHsCode> GetRegulatedHsCodeList(int agencyId, string documentTypeCode);
        List<HscodeDetails> GetHsCodeDetailList(string hscode, string documentTypeCode, int agencyId);
        List<string> GetPCTCodeList(string hscode);
        List<string> GetExtHsCodeList(int agencyId, string requiredDocumentTypeCode, int tradeTransitTypeId);
    }
}
