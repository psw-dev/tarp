/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;
using System.Linq;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the RegulatedHSCode table in the database 
    /// </summary>
	public class RegulatedHSCode : Entity
	{
		#region Fields
		
		private long _iD;
		private string _hSCode;
		private string _itemDescription;
		private string _hSCodeExt;
		private string _itemDescriptionExt;
		private short _agencyID;
		private string _requiredDocumentTypeCode;
		private long _ruleID;
		private DateTime _effectiveFromDt;
		private DateTime _effectiveThruDt;
		private DateTime _createdOn;
		private int _createdBy;
		private DateTime _updatedOn;
		private int _updatedBy;
		private DateTime? _authorizedOn;
		private int? _authorizedBy;
		private System.Byte[] _lastChange;

		#endregion

		#region Properties
		
		public long ID { get { return _iD; } set { _iD = value; PrimaryKey = value; }}
		public string HSCode { get { return _hSCode; } set { _hSCode = value;  }}
		public string ItemDescription { get { return _itemDescription; } set { _itemDescription = value;  }}
		public string HSCodeExt { get { return _hSCodeExt; } set { _hSCodeExt = value;  }}
		public string ItemDescriptionExt { get { return _itemDescriptionExt; } set { _itemDescriptionExt = value;  }}
		public short AgencyID { get { return _agencyID; } set { _agencyID = value;  }}
		public string RequiredDocumentTypeCode { get { return _requiredDocumentTypeCode; } set { _requiredDocumentTypeCode = value;  }}
		public long RuleID { get { return _ruleID; } set { _ruleID = value;  }}
		public DateTime EffectiveFromDt { get { return _effectiveFromDt; } set { _effectiveFromDt = value;  }}
		public DateTime EffectiveThruDt { get { return _effectiveThruDt; } set { _effectiveThruDt = value;  }}
		public DateTime CreatedOn { get { return _createdOn; } set { _createdOn = value;  }}
		public int CreatedBy { get { return _createdBy; } set { _createdBy = value;  }}
		public DateTime UpdatedOn { get { return _updatedOn; } set { _updatedOn = value;  }}
		public int UpdatedBy { get { return _updatedBy; } set { _updatedBy = value;  }}
		public DateTime? AuthorizedOn { get { return _authorizedOn; } set { _authorizedOn = value;  }}
		public int? AuthorizedBy { get { return _authorizedBy; } set { _authorizedBy = value;  }}
		public System.Byte[] LastChange { get { return _lastChange; } set { _lastChange = value;  }}

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
				{"AgencyID", AgencyID},
				{"RequiredDocumentTypeCode", RequiredDocumentTypeCode},
				{"RuleID", RuleID},
				{"EffectiveFromDt", EffectiveFromDt},
				{"EffectiveThruDt", EffectiveThruDt},
				{"CreatedOn", CreatedOn},
				{"CreatedBy", CreatedBy},
				{"UpdatedOn", UpdatedOn},
				{"UpdatedBy", UpdatedBy},
				{"AuthorizedOn", AuthorizedOn},
				{"AuthorizedBy", AuthorizedBy},
				{"LastChange", LastChange}
			};
        }

		#endregion

		#region Constructors

		public RegulatedHSCode()
		{
			TableName = "RegulatedHSCode";
			PrimaryKeyName = "ID";
		}
		
		#endregion
	}
} 

