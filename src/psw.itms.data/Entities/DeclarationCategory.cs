/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;
using System.Linq;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the DeclarationCategory table in the database 
    /// </summary>
	public class DeclarationCategory : Entity
	{
		#region Fields
		
		private short _iD;
		private string _name;
		private short _agencyID;
		private DateTime _createdOn;
		private int _createdBy;
		private System.Byte[] _lastChange;

		#endregion

		#region Properties
		
		public short ID { get { return _iD; } set { _iD = value; PrimaryKey = value; }}
		public string Name { get { return _name; } set { _name = value;  }}
		public short AgencyID { get { return _agencyID; } set { _agencyID = value;  }}
		public DateTime CreatedOn { get { return _createdOn; } set { _createdOn = value;  }}
		public int CreatedBy { get { return _createdBy; } set { _createdBy = value;  }}
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
				{"Name", Name},
				{"AgencyID", AgencyID},
				{"CreatedOn", CreatedOn},
				{"CreatedBy", CreatedBy},
				{"LastChange", LastChange}
			};
        }

		#endregion

		#region Constructors

		public DeclarationCategory()
		{
			TableName = "DeclarationCategory";
			PrimaryKeyName = "ID";
		}
		
		#endregion
	}
} 

