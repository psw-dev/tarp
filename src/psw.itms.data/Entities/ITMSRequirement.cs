/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;
using System.Linq;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the ITMSRequirement table in the database 
    /// </summary>
	public class ITMSRequirement : Entity
	{
		#region Fields
		
		private long _iD;
		private int _pCTCodeID;
		private string _hSCode1;
		private string _hSCode2;
		private string _documentTypeCode;
		private System.SByte _requestTypeID;
		private string _name;
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
		
		public long ID { get { return _iD; } set { _iD = value; PrimaryKey = value; }}
		public int PCTCodeID { get { return _pCTCodeID; } set { _pCTCodeID = value;  }}
		public string HSCode1 { get { return _hSCode1; } set { _hSCode1 = value;  }}
		public string HSCode2 { get { return _hSCode2; } set { _hSCode2 = value;  }}
		public string DocumentTypeCode { get { return _documentTypeCode; } set { _documentTypeCode = value;  }}
		public System.SByte RequestTypeID { get { return _requestTypeID; } set { _requestTypeID = value;  }}
		public string Name { get { return _name; } set { _name = value;  }}
		public System.SByte PurposeLogicOperatorID { get { return _purposeLogicOperatorID; } set { _purposeLogicOperatorID = value;  }}
		public string PurposeIDList { get { return _purposeIDList; } set { _purposeIDList = value;  }}
		public System.SByte CountryCodeLogicOperatorID { get { return _countryCodeLogicOperatorID; } set { _countryCodeLogicOperatorID = value;  }}
		public string CountryCodeList { get { return _countryCodeList; } set { _countryCodeList = value;  }}
		public System.SByte ChargePortcOperatorID { get { return _chargePortcOperatorID; } set { _chargePortcOperatorID = value;  }}
		public string ChargePortIDList { get { return _chargePortIDList; } set { _chargePortIDList = value;  }}
		public System.SByte DischargePortLogicOperatorID { get { return _dischargePortLogicOperatorID; } set { _dischargePortLogicOperatorID = value;  }}
		public string DischargePortIDList { get { return _dischargePortIDList; } set { _dischargePortIDList = value;  }}
		public long RequirementSetID { get { return _requirementSetID; } set { _requirementSetID = value;  }}
		public short AgencyID { get { return _agencyID; } set { _agencyID = value;  }}
		public DateTime EffectiveFromDt { get { return _effectiveFromDt; } set { _effectiveFromDt = value;  }}
		public DateTime EffectiveThruDt { get { return _effectiveThruDt; } set { _effectiveThruDt = value;  }}
		public DateTime CreatedOn { get { return _createdOn; } set { _createdOn = value;  }}
		public int CreatedBy { get { return _createdBy; } set { _createdBy = value;  }}
		public DateTime UpdatedOn { get { return _updatedOn; } set { _updatedOn = value;  }}
		public int UpdatedBy { get { return _updatedBy; } set { _updatedBy = value;  }}
		public DateTime? AuthorizedOn { get { return _authorizedOn; } set { _authorizedOn = value;  }}
		public int? AuthorizedBy { get { return _authorizedBy; } set { _authorizedBy = value;  }}

		#endregion

		#region Methods

		#endregion

		#region public Methods

		public override Dictionary<string, object> GetColumns()
        {
            return new Dictionary<string, object> 
			{
				{"ID", ID},
				{"PCTCodeID", PCTCodeID},
				{"HSCode1", HSCode1},
				{"HSCode2", HSCode2},
				{"DocumentTypeCode", DocumentTypeCode},
				{"RequestTypeID", RequestTypeID},
				{"Name", Name},
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

