/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the NilRequirement table in the database 
    /// </summary>
	public class NilRequirement : Entity
    {
        #region Fields

        private int _iD;
        private long _requirementID;
        private string _displayText;
        private DateTime _createdOn;
        private int _createdBy;
        private System.Byte[] _lastChange;

        #endregion

        #region Properties

        public int ID { get { return _iD; } set { _iD = value; PrimaryKey = value; } }
        public long RequirementID { get { return _requirementID; } set { _requirementID = value; } }
        public string DisplayText { get { return _displayText; } set { _displayText = value; } }
        public DateTime CreatedOn { get { return _createdOn; } set { _createdOn = value; } }
        public int CreatedBy { get { return _createdBy; } set { _createdBy = value; } }
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
                {"RequirementID", RequirementID},
                {"DisplayText", DisplayText},
                {"CreatedOn", CreatedOn},
                {"CreatedBy", CreatedBy},
                {"LastChange", LastChange}
            };
        }

        #endregion

        #region Constructors

        public NilRequirement()
        {
            TableName = "NilRequirement";
            PrimaryKeyName = "ID";
        }

        #endregion
    }
}

