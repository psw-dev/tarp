/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System.Collections.Generic;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the TermUoM table in the database 
    /// </summary>
	public class TermUoM : Entity
    {
        #region Fields

        private System.SByte _iD;
        private string _name;
        private System.Byte[] _lastChange;

        #endregion

        #region Properties

        public System.SByte ID { get { return _iD; } set { _iD = value; PrimaryKey = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public System.Byte[] LastChange { get { return _lastChange; } set { _lastChange = value; } }

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
                {"LastChange", LastChange}
            };
        }

        #endregion

        #region Constructors

        public TermUoM()
        {
            TableName = "TermUoM";
            PrimaryKeyName = "ID";
        }

        #endregion
    }
}

