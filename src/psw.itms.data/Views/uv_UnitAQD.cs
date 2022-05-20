
using PSW.ITMS.Data.Entities;
using System.Collections.Generic;



namespace PSW.ITMS.Data.Objects.Views
{
    /// <summary>
    /// This class represents the uv_UnitAQD table in the database 
    /// </summary>
	public class uv_UnitAQD : Entity
    {
        #region Fields

        
		public int _iD ;
		public string _name ;
		public bool? _isActive ;
        #endregion

        #region Properties

        public int ID { get { return _iD; } set { _iD = value; PrimaryKey = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public bool? IsActive { get { return _isActive; } set { _isActive = value; } }
       
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
                {"IsActive", IsActive}
            };
        }

        #endregion

        #region Constructors

        public uv_UnitAQD()
        {
            TableName = "uv_UnitAQD";
            PrimaryKeyName = "ID";
        }

        #endregion
    }
}

