/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using PSW.ITMS.Data.Entities;

namespace PSW.ITMS.Data.Objects.Views
{
    public class UV_DocumentaryRequirement: Entity
    {
        #region Properties

		
		public long ID { get; set; }
		public string HSCode { get; set; }
		public string HSCodeExt { get; set; }
		public string ItemDescription { get; set; }
		public string ItemDescriptionExt { get; set; }
		public System.SByte UoMID { get; set; }
		public string TechnicalName { get; set; }
		public string RequestedDocument { get; set; }
		public System.SByte RequirementStageID { get; set; }
		public System.SByte RequestTypeID { get; set; }
		public System.SByte PurposeLogicOperatorID { get; set; }
		public string PurposeIDList { get; set; }
		public System.SByte CountryCodeLogicOperatorID { get; set; }
		public string CountryCodeList { get; set; }
		public System.SByte ChargePortcOperatorID { get; set; }
		public string ChargePortIDList { get; set; }
		public System.SByte DischargePortLogicOperatorID { get; set; }
		public string DischargePortIDList { get; set; }
		public short AgencyID { get; set; }
		public System.SByte RequirementCategoryID { get; set; }
		public string RequirementCategoryName { get; set; }
		public string RequiredDocumentTypeCode { get; set; }

        #endregion
    }
}