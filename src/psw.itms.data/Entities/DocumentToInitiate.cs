/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;
using System.Linq;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the DocumentToInitiate table in the database 
    /// </summary>
	public class DocumentToInitiate : Entity
	{
		#region Fields
		
		private int _iD;
		private string _documentCode;
		private string _requiredDocumentCode;
		private DateTime _effectiveFrmDt;
		private DateTime _effectiveThruDt;
		private DateTime _createdOn;
		private int _createdBy;
		private DateTime _updatedOn;
		private int _updatedBy;
		private int? _agencyID;
		private string _hsCodeExt;
		private int? _tradeTranTypeID;

		#endregion

		#region Properties
		
		public int ID { get { return _iD; } set { _iD = value; PrimaryKey = value; }}
		public string DocumentCode { get { return _documentCode; } set { _documentCode = value;  }}
		public string RequiredDocumentCode { get { return _requiredDocumentCode; } set { _requiredDocumentCode = value;  }}
		public DateTime EffectiveFrmDt { get { return _effectiveFrmDt; } set { _effectiveFrmDt = value;  }}
		public DateTime EffectiveThruDt { get { return _effectiveThruDt; } set { _effectiveThruDt = value;  }}
		public DateTime CreatedOn { get { return _createdOn; } set { _createdOn = value;  }}
		public int CreatedBy { get { return _createdBy; } set { _createdBy = value;  }}
		public DateTime UpdatedOn { get { return _updatedOn; } set { _updatedOn = value;  }}
		public int UpdatedBy { get { return _updatedBy; } set { _updatedBy = value;  }}
		public int? AgencyID { get { return _agencyID; } set { _agencyID = value;  }}
		public string HsCodeExt { get { return _hsCodeExt; } set { _hsCodeExt = value;  }}
		public int? TradeTranTypeID { get { return _tradeTranTypeID; } set { _tradeTranTypeID = value;  }}

		#endregion

		#region Methods

		#endregion

		#region public Methods

		public override Dictionary<string, object> GetColumns()
        {
            return new Dictionary<string, object> 
			{
				{"ID", ID},
				{"DocumentCode", DocumentCode},
				{"RequiredDocumentCode", RequiredDocumentCode},
				{"EffectiveFrmDt", EffectiveFrmDt},
				{"EffectiveThruDt", EffectiveThruDt},
				{"CreatedOn", CreatedOn},
				{"CreatedBy", CreatedBy},
				{"UpdatedOn", UpdatedOn},
				{"UpdatedBy", UpdatedBy},
				{"AgencyID", AgencyID},
				{"HsCodeExt", HsCodeExt},
				{"TradeTranTypeID", TradeTranTypeID}
			};
        }

		#endregion

		#region Constructors

		public DocumentToInitiate()
		{
			TableName = "DocumentToInitiate";
			PrimaryKeyName = "ID";
		}
		
		#endregion
	}
} 

