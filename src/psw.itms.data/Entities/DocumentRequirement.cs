/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;
using System.Linq;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the DocumentRequirement table in the database 
    /// </summary>
	public class DocumentRequirement : Entity
	{
		#region Fields
		
		private long _iD;
		private long _requirementID;
		private System.SByte _attachedObjectFormatID;
		private string _documentTypeCode;
		private bool _isMandatory;
		private System.SByte _requirementStageID;
		private DateTime _createdOn;
		private int _createdBy;

		#endregion

		#region Properties
		
		public long ID { get { return _iD; } set { _iD = value; PrimaryKey = value; }}
		public long RequirementID { get { return _requirementID; } set { _requirementID = value;  }}
		public System.SByte AttachedObjectFormatID { get { return _attachedObjectFormatID; } set { _attachedObjectFormatID = value;  }}
		public string DocumentTypeCode { get { return _documentTypeCode; } set { _documentTypeCode = value;  }}
		public bool IsMandatory { get { return _isMandatory; } set { _isMandatory = value;  }}
		public System.SByte RequirementStageID { get { return _requirementStageID; } set { _requirementStageID = value;  }}
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
				{"DocumentTypeCode", DocumentTypeCode},
				{"IsMandatory", IsMandatory},
				{"RequirementStageID", RequirementStageID},
				{"CreatedOn", CreatedOn},
				{"CreatedBy", CreatedBy}
			};
        }

		#endregion

		#region Constructors
		
		#endregion
	}
} 

