/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using PSW.ITMS.Data.Entities;
using System.Collections.Generic;

namespace PSW.ITMS.Data.Objects.Views
{
    public class UV_DocumentaryRequirement : Entity
    {
        #region Fields

        private long _iD;
        private string _hSCode;
        private string _hSCodeExt;
        private string _itemDescription;
        private string _itemDescriptionExt;
        private System.SByte _uoMID;
        private string _technicalName;
        private string _requestedDocument;
        private System.SByte _requirementStageID;
        private System.SByte _requestTypeID;
        private System.SByte _purposeLogicOperatorID;
        private string _purposeIDList;

        private System.SByte _countryCodeLogicOperatorID;
        private string _countryCodeList;
        private System.SByte _chargePortcOperatorID;
        private string _chargePortIDList;
        private System.SByte _dischargePortLogicOperatorID;
        private string _dischargePortIDList;
        private short _agencyID;
        private System.SByte _requirementCategoryID;
        private string _requirementCategoryName;
        private string _requiredDocumentTypeCode;
        private string _requiredDocumentTypeName;
        private decimal _billAmount;
        #endregion


        #region Properties


        public long ID { get { return _iD; } set { _iD = value; PrimaryKey = value; } }
        public string HSCode { get { return _hSCode; } set { _hSCode = value; } }
        public string HSCodeExt { get { return _hSCodeExt; } set { _hSCodeExt = value; } }
        public string ItemDescription { get { return _itemDescription; } set { _itemDescription = value; } }
        public string ItemDescriptionExt { get { return _itemDescriptionExt; } set { _itemDescriptionExt = value; } }
        public System.SByte UoMID { get { return _uoMID; } set { _uoMID = value; } }
        public string TechnicalName { get { return _technicalName; } set { _technicalName = value; } }
        public string RequestedDocument { get { return _requestedDocument; } set { _requestedDocument = value; } }
        public System.SByte RequirementStageID { get { return _requirementStageID; } set { _requirementStageID = value; } }
        public System.SByte RequestTypeID { get { return _requestTypeID; } set { _requestTypeID = value; } }
        public System.SByte PurposeLogicOperatorID { get { return _purposeLogicOperatorID; } set { _purposeLogicOperatorID = value; } }
        public string PurposeIDList { get { return _purposeIDList; } set { _purposeIDList = value; } }
        public System.SByte CountryCodeLogicOperatorID { get { return _countryCodeLogicOperatorID; } set { _countryCodeLogicOperatorID = value; } }
        public string CountryCodeList { get { return _countryCodeList; } set { _countryCodeList = value; } }
        public System.SByte ChargePortcOperatorID { get { return _chargePortcOperatorID; } set { _chargePortcOperatorID = value; } }
        public string ChargePortIDList { get { return _chargePortIDList; } set { _chargePortIDList = value; } }
        public System.SByte DischargePortLogicOperatorID { get { return _dischargePortLogicOperatorID; } set { _dischargePortLogicOperatorID = value; } }
        public string DischargePortIDList { get { return _dischargePortIDList; } set { _dischargePortIDList = value; } }
        public short AgencyID { get { return _agencyID; } set { _agencyID = value; } }
        public System.SByte RequirementCategoryID { get { return _requirementCategoryID; } set { _requirementCategoryID = value; } }
        public string RequirementCategoryName { get { return _requirementCategoryName; } set { _requirementCategoryName = value; } }
        public string RequiredDocumentTypeCode { get { return _requiredDocumentTypeCode; } set { _requiredDocumentTypeCode = value; } }
        public string RequiredDocumentTypeName { get { return _requiredDocumentTypeName; } set { _requiredDocumentTypeName = value; } }
        public decimal BillAmount { get { return _billAmount; } set { _billAmount = value; } }


        #endregion

        #region public Methods

        public override Dictionary<string, object> GetColumns()
        {
            return new Dictionary<string, object>
            {
                {"ID", ID},
                {"HSCode", HSCode},
                {"HSCodeExt", HSCodeExt},
                {"ItemDescription", ItemDescription},
                {"ItemDescriptionExt", ItemDescriptionExt},
                {"UoMID", UoMID},
                {"TechnicalName", TechnicalName},
                {"RequestedDocument", RequestedDocument},
                {"RequirementStageID", RequirementStageID},
                {"RequestTypeID", RequestTypeID},
                {"PurposeLogicOperatorID", PurposeLogicOperatorID},
                {"PurposeIDList", PurposeIDList},
                {"CountryCodeLogicOperatorID", CountryCodeLogicOperatorID},
                {"CountryCodeList", CountryCodeList},
                {"ChargePortcOperatorID", ChargePortcOperatorID},
                {"ChargePortIDList", ChargePortIDList},
                {"DischargePortLogicOperatorID", DischargePortLogicOperatorID},
                {"DischargePortIDList", DischargePortIDList},
                {"AgencyID", AgencyID},
                {"RequirementCategoryID", RequirementCategoryID},
                {"RequirementCategoryName", RequirementCategoryName},
                {"RequiredDocumentTypeCode", RequiredDocumentTypeCode},
                {"RequiredDocumentTypeName", RequiredDocumentTypeName},
                {"BillAmount", BillAmount}
            };
        }

        #endregion
    }
}