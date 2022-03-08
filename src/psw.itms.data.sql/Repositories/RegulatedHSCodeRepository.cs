/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using Dapper;
using PSW.ITMS.Data.Entities;
using PSW.ITMS.Data.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PSW.ITMS.Data.Sql.Repositories
{
    public class RegulatedHSCodeRepository : Repository<RegulatedHSCode>, IRegulatedHSCodeRepository
    {
        #region public constructors

        public RegulatedHSCodeRepository(IDbConnection context) : base(context)
        {
            TableName = "[dbo].[RegulatedHSCode]";
            PrimaryKeyName = "ID";
        }

        #endregion

        #region Public methods

        public List<AgencyList> GetAgencyListAgainstHscode(string hscode, int tradeTranTypeID)
        {
            return _connection.Query<AgencyList>(string.Format("SELECT A.ID, A.NAME, RHS.ITEMDESCRIPTION FROM REGULATEDHSCODE RHS INNER JOIN SHRD.DBO.AGENCY A ON RHS.AGENCYID = A.ID WHERE HSCODEEXT = '{0}' AND TRADETRANTYPEID = '{1}' AND GETDATE() BETWEEN EFFECTIVEFROMDT AND EFFECTIVETHRUDT", hscode, tradeTranTypeID)).ToList();
        }

        public List<ViewRegulatedHsCode> GetRegulatedHsCodeList()
        {
            return _connection.Query<ViewRegulatedHsCode>(string.Format("SELECT DISTINCT HsCode, AgencyID FROM REGULATEDHSCODE WHERE GETDATE() BETWEEN EFFECTIVEFROMDT AND EFFECTIVETHRUDT")).ToList();
        }

        public List<ViewRegulatedHsCode> GetRegulatedHsCodeList(int agencyId)
        {
            return _connection.Query<ViewRegulatedHsCode>(string.Format("SELECT DISTINCT HsCode, AgencyID FROM REGULATEDHSCODE WHERE AGENCYID = '{0}' AND GETDATE() BETWEEN EFFECTIVEFROMDT AND EFFECTIVETHRUDT", agencyId)).ToList();
        }

        public List<ViewRegulatedHsCode> GetRegulatedHsCodeList(int agencyId, string documentTypeCode)
        {
            return _connection.Query<ViewRegulatedHsCode>(string.Format("SELECT DISTINCT HsCode, AgencyID FROM REGULATEDHSCODE WHERE AGENCYID = '{0}' AND REQUIREDDOCUMENTTYPECODE = '{1}' AND GETDATE() BETWEEN EFFECTIVEFROMDT AND EFFECTIVETHRUDT", agencyId, documentTypeCode)).ToList();
        }

        public List<HscodeDetails> GetHsCodeDetailList(string hscode, string documentTypeCode, int agencyId)
        {
            return _connection.Query<HscodeDetails>(string.Format("SELECT DISTINCT ITEMDESCRIPTION, PRODUCTCODE, ITEMDESCRIPTIONEXT, TECHNICALNAME FROM REGULATEDHSCODE WHERE HSCODE = '{0}' AND REQUIREDDOCUMENTTYPECODE = '{1}' AND AGENCYID = '{2}' AND GETDATE() BETWEEN EFFECTIVEFROMDT AND EFFECTIVETHRUDT", hscode, documentTypeCode, agencyId)).ToList();
        }

        public List<ProductDetail> GetPCTCodeList(string hscode)
        {
            return _connection.Query<ProductDetail>(string.Format("SELECT DISTINCT PRODUCTCODE, Concat(ProductCode, ItemDescription) as  ItemDescription  FROM REGULATEDHSCODE WHERE HSCODE = '{0}' AND GETDATE() BETWEEN EFFECTIVEFROMDT AND EFFECTIVETHRUDT", hscode)).ToList();
        }

        public List<string> GetExtHsCodeList(int agencyId, string requiredDocumentTypeCode, int tradeTransitTypeId)
        {
            return _connection.Query<string>(string.Format("SELECT HSCODEEXT FROM REGULATEDHSCODE WHERE AGENCYID = '{0}' AND TRADETRANTYPEID = '{1}' AND REQUIREDDOCUMENTTYPECODE = '{2}' AND GETDATE() BETWEEN EFFECTIVEFROMDT AND EFFECTIVETHRUDT", agencyId, tradeTransitTypeId, requiredDocumentTypeCode)).ToList();
        }
        public RegulatedHSCode GetActiveHsCode(string hsCodeExt, string agencyId, int tradeTranTypeId, string requiredDocumentTypeCode)
        {
            return _connection.Query<RegulatedHSCode>(string.Format("select * from RegulatedHSCode where HSCodeExt = '{0}' AND AgencyId = '{1}' AND TradeTranTypeID = '{2}' AND RequiredDocumentTypeCode = '{3}' AND GETDATE() BETWEEN EFFECTIVEFROMDT AND EFFECTIVETHRUDT",hsCodeExt, agencyId, tradeTranTypeId, requiredDocumentTypeCode)).FirstOrDefault();
        }

        #endregion
    }
}
