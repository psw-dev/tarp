/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;
using System.Linq;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the Branch table in the database 
    /// </summary>
	public class Branch : Entity
	{
		#region Fields
		
		private int _iD;
		private short _bankID;
		private string _name;
		private string _code;

		#endregion

		#region Properties
		
		public int ID { get { return _iD; } set { _iD = value; PrimaryKey = value; }}
		public short BankID { get { return _bankID; } set { _bankID = value;  }}
		public string Name { get { return _name; } set { _name = value;  }}
		public string Code { get { return _code; } set { _code = value;  }}

		#endregion

		#region Methods

		#endregion

		#region public Methods

		public override Dictionary<string, object> GetColumns()
        {
            return new Dictionary<string, object> 
			{
				{"ID", ID},
				{"BankID", BankID},
				{"Name", Name},
				{"Code", Code}
			};
        }

		#endregion

		#region Constructors

		public Branch()
		{
			TableName = "Branch";
			PrimaryKeyName = "ID";
		}
		
		#endregion
	}
} 

