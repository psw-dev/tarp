/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;
using System.Linq;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the RequirementSet table in the database 
    /// </summary>
	public class RequirementSet : Entity
	{
		#region Fields
		
		private long _iD;
		private string _name;
		private short _agencyID;
		private DateTime _createdOn;
		private int _createdBy;

		#endregion

		#region Properties
		
		public long ID { get { return _iD; } set { _iD = value; PrimaryKey = value; }}
		public string Name { get { return _name; } set { _name = value;  }}
		public short AgencyID { get { return _agencyID; } set { _agencyID = value;  }}
		public DateTime CreatedOn { get { return _createdOn; } set { _createdOn = value;  }}
		public int CreatedBy { get { return _createdBy; } set { _createdBy = value;  }}

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
				{"CreatedBy", CreatedBy}
			};
        }

		#endregion

		#region Constructors
		
		#endregion
	}
} 

