/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;
using System.Linq;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the TestingRequirement table in the database 
    /// </summary>
	public class TestingRequirement : Entity
	{
		#region Fields
		
		private long _iD;
		private long _requirementID;
		private System.SByte _attachedObjectFormatID;
		private int _testID;
		private bool _isMandatory;
		private DateTime _createdOn;
		private int _createdBy;

		#endregion

		#region Properties
		
		public long ID { get { return _iD; } set { _iD = value; PrimaryKey = value; }}
		public long RequirementID { get { return _requirementID; } set { _requirementID = value;  }}
		public System.SByte AttachedObjectFormatID { get { return _attachedObjectFormatID; } set { _attachedObjectFormatID = value;  }}
		public int TestID { get { return _testID; } set { _testID = value;  }}
		public bool IsMandatory { get { return _isMandatory; } set { _isMandatory = value;  }}
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
				{"RequirementID", RequirementID},
				{"AttachedObjectFormatID", AttachedObjectFormatID},
				{"TestID", TestID},
				{"IsMandatory", IsMandatory},
				{"CreatedOn", CreatedOn},
				{"CreatedBy", CreatedBy}
			};
        }

		#endregion

		#region Constructors
		
		#endregion
	}
} 

