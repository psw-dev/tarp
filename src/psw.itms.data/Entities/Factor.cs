/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;
using System.Linq;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the Factor table in the database 
    /// </summary>
	public class Factor : Entity
	{
		#region Fields
		
		private long _iD;
		private short _agencyID;
		private string _label;
		private string _description;
		private System.SByte _dataTypeID;
		private bool _isLOV;
		private int? _lOVID;
		private bool _isActive;
		private DateTime _createdOn;
		private int _createdBy;
		private DateTime _updatedOn;
		private int _updatedBy;
		private System.Byte[] _lastChange;
		private string _factorCode;

		#endregion

		#region Properties
		
		public long ID { get { return _iD; } set { _iD = value; PrimaryKey = value; }}
		public short AgencyID { get { return _agencyID; } set { _agencyID = value;  }}
		public string Label { get { return _label; } set { _label = value;  }}
		public string Description { get { return _description; } set { _description = value;  }}
		public System.SByte DataTypeID { get { return _dataTypeID; } set { _dataTypeID = value;  }}
		public bool IsLOV { get { return _isLOV; } set { _isLOV = value;  }}
		public int? LOVID { get { return _lOVID; } set { _lOVID = value;  }}
		public bool IsActive { get { return _isActive; } set { _isActive = value;  }}
		public DateTime CreatedOn { get { return _createdOn; } set { _createdOn = value;  }}
		public int CreatedBy { get { return _createdBy; } set { _createdBy = value;  }}
		public DateTime UpdatedOn { get { return _updatedOn; } set { _updatedOn = value;  }}
		public int UpdatedBy { get { return _updatedBy; } set { _updatedBy = value;  }}
		public System.Byte[] LastChange { get { return _lastChange; } set { _lastChange = value;  }}
		public string FactorCode { get { return _factorCode; } set { _factorCode = value; }}

		#endregion

		#region Methods

		#endregion

		#region public Methods

		public override Dictionary<string, object> GetColumns()
        {
            return new Dictionary<string, object> 
			{
				{"ID", ID},
				{"AgencyID", AgencyID},
				{"Label", Label},
				{"Description", Description},
				{"DataTypeID", DataTypeID},
				{"IsLOV", IsLOV},
				{"LOVID", LOVID},
				{"IsActive", IsActive},
				{"CreatedOn", CreatedOn},
				{"CreatedBy", CreatedBy},
				{"UpdatedOn", UpdatedOn},
				{"UpdatedBy", UpdatedBy},
				{"LastChange", LastChange},
				{"FactorCode", FactorCode}
			};
        }

		#endregion

		#region Constructors

		public Factor()
		{
			TableName = "Factor";
			PrimaryKeyName = "ID";
		}
		
		#endregion
	}
} 

