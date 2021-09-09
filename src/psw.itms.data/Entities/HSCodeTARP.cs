/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the HSCodeTARP table in the database 
    /// </summary>
	public class HSCodeTARP : Entity
    {
        #region Fields

        private long _iD;
        private string _hSCode;
        private string _itemDescription;
        private string _hSCodeExt;
        private string _itemDescriptionExt;
        private System.SByte _uoMID;
        private string _documentTypeCode;
        private System.SByte _requestTypeID;
        private string _technicalName;
        private System.SByte _purposeLogicOperatorID;
        private string _purposeIDList;
        private System.SByte _countryCodeLogicOperatorID;
        private string _countryCodeList;
        private System.SByte _chargePortcOperatorID;
        private string _chargePortIDList;
        private System.SByte _dischargePortLogicOperatorID;
        private string _dischargePortIDList;
        private long _requirementSetID;
        private short _agencyID;
        private DateTime _effectiveFromDt;
        private DateTime _effectiveThruDt;
        private DateTime _createdOn;
        private int _createdBy;
        private DateTime _updatedOn;
        private int _updatedBy;
        private DateTime? _authorizedOn;
        private int? _authorizedBy;

        #endregion

        #region Properties

        public long ID { get { return _iD; } set { _iD = value; PrimaryKey = value; } }
        public string HSCode { get { return _hSCode; } set { _hSCode = value; } }
        public string ItemDescription { get { return _itemDescription; } set { _itemDescription = value; } }
        public string HSCodeExt { get { return _hSCodeExt; } set { _hSCodeExt = value; } }
        public string ItemDescriptionExt { get { return _itemDescriptionExt; } set { _itemDescriptionExt = value; } }
        public System.SByte UoMID { get { return _uoMID; } set { _uoMID = value; } }
        public string DocumentTypeCode { get { return _documentTypeCode; } set { _documentTypeCode = value; } }
        public System.SByte RequestTypeID { get { return _requestTypeID; } set { _requestTypeID = value; } }
        public string TechnicalName { get { return _technicalName; } set { _technicalName = value; } }
        public System.SByte PurposeLogicOperatorID { get { return _purposeLogicOperatorID; } set { _purposeLogicOperatorID = value; } }
        public string PurposeIDList { get { return _purposeIDList; } set { _purposeIDList = value; } }
        public System.SByte CountryCodeLogicOperatorID { get { return _countryCodeLogicOperatorID; } set { _countryCodeLogicOperatorID = value; } }
        public string CountryCodeList { get { return _countryCodeList; } set { _countryCodeList = value; } }
        public System.SByte ChargePortcOperatorID { get { return _chargePortcOperatorID; } set { _chargePortcOperatorID = value; } }
        public string ChargePortIDList { get { return _chargePortIDList; } set { _chargePortIDList = value; } }
        public System.SByte DischargePortLogicOperatorID { get { return _dischargePortLogicOperatorID; } set { _dischargePortLogicOperatorID = value; } }
        public string DischargePortIDList { get { return _dischargePortIDList; } set { _dischargePortIDList = value; } }
        public long RequirementSetID { get { return _requirementSetID; } set { _requirementSetID = value; } }
        public short AgencyID { get { return _agencyID; } set { _agencyID = value; } }
        public DateTime EffectiveFromDt { get { return _effectiveFromDt; } set { _effectiveFromDt = value; } }
        public DateTime EffectiveThruDt { get { return _effectiveThruDt; } set { _effectiveThruDt = value; } }
        public DateTime CreatedOn { get { return _createdOn; } set { _createdOn = value; } }
        public int CreatedBy { get { return _createdBy; } set { _createdBy = value; } }
        public DateTime UpdatedOn { get { return _updatedOn; } set { _updatedOn = value; } }
        public int UpdatedBy { get { return _updatedBy; } set { _updatedBy = value; } }
        public DateTime? AuthorizedOn { get { return _authorizedOn; } set { _authorizedOn = value; } }
        public int? AuthorizedBy { get { return _authorizedBy; } set { _authorizedBy = value; } }

        #endregion

        #region Methods

        #endregion

        #region public Methods

        public override Dictionary<string, object> GetColumns()
        {
            return new Dictionary<string, object>
            {
                {"ID", ID},
                {"HSCode", HSCode},
                {"ItemDescription", ItemDescription},
                {"HSCodeExt", HSCodeExt},
                {"ItemDescriptionExt", ItemDescriptionExt},
                {"UoMID", UoMID},
                {"DocumentTypeCode", DocumentTypeCode},
                {"RequestTypeID", RequestTypeID},
                {"TechnicalName", TechnicalName},
                {"PurposeLogicOperatorID", PurposeLogicOperatorID},
                {"PurposeIDList", PurposeIDList},
                {"CountryCodeLogicOperatorID", CountryCodeLogicOperatorID},
                {"CountryCodeList", CountryCodeList},
                {"ChargePortcOperatorID", ChargePortcOperatorID},
                {"ChargePortIDList", ChargePortIDList},
                {"DischargePortLogicOperatorID", DischargePortLogicOperatorID},
                {"DischargePortIDList", DischargePortIDList},
                {"RequirementSetID", RequirementSetID},
                {"AgencyID", AgencyID},
                {"EffectiveFromDt", EffectiveFromDt},
                {"EffectiveThruDt", EffectiveThruDt},
                {"CreatedOn", CreatedOn},
                {"CreatedBy", CreatedBy},
                {"UpdatedOn", UpdatedOn},
                {"UpdatedBy", UpdatedBy},
                {"AuthorizedOn", AuthorizedOn},
                {"AuthorizedBy", AuthorizedBy}
            };
        }

        #endregion

        #region Constructors

        #endregion
    }
}

