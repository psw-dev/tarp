/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;
using System.Linq;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the DocumentType table in the database 
    /// </summary>
	public class DocumentType : Entity
	{
		#region Fields
		
		private string _code;
		private string _name;
		private short? _agencyID;
		private bool _sysOwn;
		private string _tableName;
		private DateTime _createdOn;
		private int _createdBy;
		private DateTime _updatedOn;
		private int _updatedBy;

		#endregion

		#region Properties
		
		public string Code { get { return _code; } set { _code = value; PrimaryKey = value; }}
		public string Name { get { return _name; } set { _name = value;  }}
		public short? AgencyID { get { return _agencyID; } set { _agencyID = value;  }}
		public bool SysOwn { get { return _sysOwn; } set { _sysOwn = value;  }}
		public string TableName { get { return _tableName; } set { _tableName = value;  }}
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
				{"Code", Code},
				{"Name", Name},
				{"AgencyID", AgencyID},
				{"SysOwn", SysOwn},
				{"TableName", TableName},
				{"CreatedOn", CreatedOn},
				{"CreatedBy", CreatedBy},
				{"UpdatedOn", UpdatedOn},
				{"UpdatedBy", UpdatedBy}
			};
        }

		#endregion

		#region Constructors

		public DocumentType()
		{
			TableName = "DocumentType";
			PrimaryKeyName = "Code";
		}
		
		#endregion
	}
} 

