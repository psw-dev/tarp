/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;
using System.Linq;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the Bank table in the database 
    /// </summary>
	public class Bank : Entity
	{
		#region Fields
		
		private short _iD;
		private string _name;
		private string _code;
		private string _mnemonic;

		#endregion

		#region Properties
		
		public short ID { get { return _iD; } set { _iD = value; PrimaryKey = value; }}
		public string Name { get { return _name; } set { _name = value;  }}
		public string Code { get { return _code; } set { _code = value;  }}
		public string Mnemonic { get { return _mnemonic; } set { _mnemonic = value;  }}

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
				{"Code", Code},
				{"Mnemonic", Mnemonic}
			};
        }

		#endregion

		#region Constructors

		public Bank()
		{
			TableName = "Bank";
			PrimaryKeyName = "ID";
		}
		
		#endregion
	}
} 

