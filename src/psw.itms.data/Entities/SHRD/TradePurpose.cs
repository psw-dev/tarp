/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;
using System.Linq;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the TradePurpose table in the database 
    /// </summary>
	public class TradePurpose : Entity
	{
		#region Fields
		
		private short _iD;
		private string _name;
		private System.SByte _tradeTranTypeID;
		private bool _isDPP;
		private bool _isAQD;
		private bool _isFSC;
		private bool _isPQC;
		private DateTime _createdOn;
		private int _createdBy;
		private DateTime _updatedOn;
		private int _updatedBy;

		#endregion

		#region Properties
		
		public short ID { get { return _iD; } set { _iD = value; PrimaryKey = value; }}
		public string Name { get { return _name; } set { _name = value;  }}
		public System.SByte TradeTranTypeID { get { return _tradeTranTypeID; } set { _tradeTranTypeID = value;  }}
		public bool IsDPP { get { return _isDPP; } set { _isDPP = value;  }}
		public bool IsAQD { get { return _isAQD; } set { _isAQD = value;  }}
		public bool IsFSC { get { return _isFSC; } set { _isFSC = value;  }}
		public bool IsPQC { get { return _isPQC; } set { _isPQC = value;  }}
		public DateTime CreatedOn { get { return _createdOn; } set { _createdOn = value;  }}
		public int CreatedBy { get { return _createdBy; } set { _createdBy = value;  }}
		public DateTime UpdatedOn { get { return _updatedOn; } set { _updatedOn = value;  }}
		public int UpdatedBy { get { return _updatedBy; } set { _updatedBy = value;  }}

		#endregion

		#region Methods

		#endregion

		#region public Methods

		public override Dictionary<string, object> GetColumns()
        {
            return new Dictionary<string, object> 
			{
				{"ID", ID},
				{"Name", Name},
				{"TradeTranTypeID", TradeTranTypeID},
				{"IsDPP", IsDPP},
				{"IsAQD", IsAQD},
				{"IsFSC", IsFSC},
				{"IsPQC", IsPQC},
				{"CreatedOn", CreatedOn},
				{"CreatedBy", CreatedBy},
				{"UpdatedOn", UpdatedOn},
				{"UpdatedBy", UpdatedBy}
			};
        }

		#endregion

		#region Constructors

		public TradePurpose()
		{
			TableName = "TradePurpose";
			PrimaryKeyName = "ID";
		}
		
		#endregion
	}
} 

