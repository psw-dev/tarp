/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;
using System.Linq;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the AddDeclarationRequirement table in the database 
    /// </summary>
	public class AddDeclarationRequirement : Entity
	{
		#region Fields
		
		private long _iD;
		private long _documentRequirementID;
		private short _declarationCategoryID;
		private string _declarationText;
		private DateTime _createdOn;
		private int _createdBy;
		private System.Byte[] _lastChange;

		#endregion

		#region Properties
		
		public long ID { get { return _iD; } set { _iD = value; PrimaryKey = value; }}
		public long DocumentRequirementID { get { return _documentRequirementID; } set { _documentRequirementID = value;  }}
		public short DeclarationCategoryID { get { return _declarationCategoryID; } set { _declarationCategoryID = value;  }}
		public string DeclarationText { get { return _declarationText; } set { _declarationText = value;  }}
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
				{"DocumentRequirementID", DocumentRequirementID},
				{"DeclarationCategoryID", DeclarationCategoryID},
				{"DeclarationText", DeclarationText},
				{"CreatedOn", CreatedOn},
				{"CreatedBy", CreatedBy},
				{"LastChange", LastChange}
			};
        }

		#endregion

		#region Constructors

		public AddDeclarationRequirement()
		{
			TableName = "AddDeclarationRequirement";
			PrimaryKeyName = "ID";
		}
		
		#endregion
	}
} 

