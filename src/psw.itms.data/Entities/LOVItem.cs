/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;
using System.Linq;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the LOVItem table in the database 
    /// </summary>
	public class LOVItem : Entity
	{
		#region Fields
		
		private long _iD;
		private int _lOVID;
		private string _itemKey;
		private string _itemValue;
		private string _altItemKey;
		private long? _parentLOVItemID;
		private string _parentLOVItemKey;

		#endregion

		#region Properties
		
		public long ID { get { return _iD; } set { _iD = value; PrimaryKey = value; }}
		public int LOVID { get { return _lOVID; } set { _lOVID = value;  }}
		public string ItemKey { get { return _itemKey; } set { _itemKey = value;  }}
		public string ItemValue { get { return _itemValue; } set { _itemValue = value;  }}
		public string AltItemKey { get { return _altItemKey; } set { _altItemKey = value;  }}
		public long? ParentLOVItemID { get { return _parentLOVItemID; } set { _parentLOVItemID = value;  }}
		public string ParentLOVItemKey { get { return _parentLOVItemKey; } set { _parentLOVItemKey = value;  }}

		#endregion

		#region Methods

		#endregion

		#region public Methods

		public override Dictionary<string, object> GetColumns()
        {
            return new Dictionary<string, object> 
			{
				{"ID", ID},
				{"LOVID", LOVID},
				{"ItemKey", ItemKey},
				{"ItemValue", ItemValue},
				{"AltItemKey", AltItemKey},
				{"ParentLOVItemID", ParentLOVItemID},
				{"ParentLOVItemKey", ParentLOVItemKey}
			};
        }

		#endregion

		#region Constructors

		public LOVItem()
		{
			TableName = "LOVItem";
			PrimaryKeyName = "ID";
		}
		
		#endregion
	}
} 

