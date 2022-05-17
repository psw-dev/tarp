using System;
using System.Collections.Generic;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the OGAItemCategory table in the database 
    /// </summary>
	public class OGAItemCategory : Entity
    {
        #region Fields

        public byte _iD;
        public short _agencyID;
        public string _description;
        public bool? _isActive;
        public DateTime _createdOn;
        public int _createdBy;
        public DateTime _updatedOn;
        public int _updatedBy;

        #endregion

        #region Properties

        public byte ID { get { return _iD; } set { _iD = value; PrimaryKey = value; } }
        public short AgencyID { get { return _agencyID; } set { _agencyID = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        public bool? IsActive { get { return _isActive; } set { _isActive = value; } }
        public DateTime CreatedOn { get { return _createdOn; } set { _createdOn = value; } }
        public int CreatedBy { get { return _createdBy; } set { _createdBy = value; } }
        public DateTime UpdatedOn { get { return _updatedOn; } set { _updatedOn = value; } }
        public int UpdatedBy { get { return _updatedBy; } set { _updatedBy = value; } }

        #endregion

        #region Methods
        #endregion

        #region public Methods

        public override Dictionary<string, object> GetColumns()
        {
            return new Dictionary<string, object>
            {
                {"ID", ID},
                {"AgencyID", AgencyID},
                 {"Description", Description},
                {"IsActive", IsActive},
                {"CreatedOn", CreatedOn},
                {"CreatedBy", CreatedBy},
                {"UpdatedOn", UpdatedOn},
                {"UpdatedBy", UpdatedBy},
            };
        }

        #endregion
    
        #region Constructors
        public OGAItemCategory()
        {
            TableName = "OGAItemCategory";
            PrimaryKeyName = "ID";
        }
        #endregion
    }
}

