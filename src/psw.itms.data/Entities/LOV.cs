/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System.Collections.Generic;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the LOV table in the database 
    /// </summary>
	public class LOV : Entity
    {
        #region Fields

        private int _iD;
        private string _name;
        private string _description;
        private bool _isActive;
        private System.SByte _lOVScopeID;
        private short? _agencyID;
        private short? _parentLOVID;
        private long? _sourcedLOVID;

        #endregion

        #region Properties

        public int ID { get { return _iD; } set { _iD = value; PrimaryKey = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }
        public System.SByte LOVScopeID { get { return _lOVScopeID; } set { _lOVScopeID = value; } }
        public short? AgencyID { get { return _agencyID; } set { _agencyID = value; } }
        public short? ParentLOVID { get { return _parentLOVID; } set { _parentLOVID = value; } }
        public long? SourcedLOVID { get { return _sourcedLOVID; } set { _sourcedLOVID = value; } }

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
                {"Description", Description},
                {"IsActive", IsActive},
                {"LOVScopeID", LOVScopeID},
                {"AgencyID", AgencyID},
                {"ParentLOVID", ParentLOVID},
                {"SourcedLOVID", SourcedLOVID}
            };
        }

        #endregion

        #region Constructors

        public LOV()
        {
            TableName = "LOV";
            PrimaryKeyName = "ID";
        }

        #endregion
    }
}

