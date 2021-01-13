/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;
using System.Linq;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the PCTCode table in the database 
    /// </summary>
	public class PCTCode : Entity
	{
		#region Fields
		
		private long _iD;
		private string _hSCode1;
		private string _hSCode2;
		private string _name;
		private System.SByte _uoMID;
		private DateTime _createdOn;
		private int _createdBy;
		private DateTime _updatedOn;
		private int _updatedBy;

		#endregion

		#region Properties
		
		public long ID { get { return _iD; } set { _iD = value; PrimaryKey = value; }}
		public string HSCode1 { get { return _hSCode1; } set { _hSCode1 = value;  }}
		public string HSCode2 { get { return _hSCode2; } set { _hSCode2 = value;  }}
		public string Name { get { return _name; } set { _name = value;  }}
		public System.SByte UoMID { get { return _uoMID; } set { _uoMID = value;  }}
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
				{"HSCode1", HSCode1},
				{"HSCode2", HSCode2},
				{"Name", Name},
				{"UoMID", UoMID},
				{"CreatedOn", CreatedOn},
				{"CreatedBy", CreatedBy},
				{"UpdatedOn", UpdatedOn},
				{"UpdatedBy", UpdatedBy}
			};
        }

		#endregion

		#region Constructors

		public PCTCode()
		{
			TableName = "PCTCode";
			PrimaryKeyName = "ID";
		}
		
		#endregion
	}
} 

