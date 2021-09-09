/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the DecisionMatrix table in the database 
    /// </summary>
	public class DecisionMatrix : Entity
    {
        #region Fields

        private long _iD;
        private long _ruleID;
        private string _factor1Value;
        private string _factor2Value;
        private string _factor3Value;
        private string _factor4Value;
        private string _factor5Value;
        private string _factor6Value;
        private string _factor7Value;
        private string _factor8Value;
        private string _factor9Value;
        private string _factor10Value;
        private string _factor11Value;
        private string _factor12Value;
        private string _factor13Value;
        private string _factor14Value;
        private string _factor15Value;
        private string _factor16Value;
        private string _factor17Value;
        private string _factor18Value;
        private string _factor19Value;
        private string _factor20Value;
        private long _requirementSetID;
        private DateTime _createdOn;
        private int _createdBy;
        private DateTime _updatedOn;
        private int _updatedBy;
        private DateTime? _authorizedOn;
        private int? _authorizedBy;
        private System.Byte[] _lastChange;

        #endregion

        #region Properties

        public long ID { get { return _iD; } set { _iD = value; PrimaryKey = value; } }
        public long RuleID { get { return _ruleID; } set { _ruleID = value; } }
        public string Factor1Value { get { return _factor1Value; } set { _factor1Value = value; } }
        public string Factor2Value { get { return _factor2Value; } set { _factor2Value = value; } }
        public string Factor3Value { get { return _factor3Value; } set { _factor3Value = value; } }
        public string Factor4Value { get { return _factor4Value; } set { _factor4Value = value; } }
        public string Factor5Value { get { return _factor5Value; } set { _factor5Value = value; } }
        public string Factor6Value { get { return _factor6Value; } set { _factor6Value = value; } }
        public string Factor7Value { get { return _factor7Value; } set { _factor7Value = value; } }
        public string Factor8Value { get { return _factor8Value; } set { _factor8Value = value; } }
        public string Factor9Value { get { return _factor9Value; } set { _factor9Value = value; } }
        public string Factor10Value { get { return _factor10Value; } set { _factor10Value = value; } }
        public string Factor11Value { get { return _factor11Value; } set { _factor11Value = value; } }
        public string Factor12Value { get { return _factor12Value; } set { _factor12Value = value; } }
        public string Factor13Value { get { return _factor13Value; } set { _factor13Value = value; } }
        public string Factor14Value { get { return _factor14Value; } set { _factor14Value = value; } }
        public string Factor15Value { get { return _factor15Value; } set { _factor15Value = value; } }
        public string Factor16Value { get { return _factor16Value; } set { _factor16Value = value; } }
        public string Factor17Value { get { return _factor17Value; } set { _factor17Value = value; } }
        public string Factor18Value { get { return _factor18Value; } set { _factor18Value = value; } }
        public string Factor19Value { get { return _factor19Value; } set { _factor19Value = value; } }
        public string Factor20Value { get { return _factor20Value; } set { _factor20Value = value; } }
        public long RequirementSetID { get { return _requirementSetID; } set { _requirementSetID = value; } }
        public DateTime CreatedOn { get { return _createdOn; } set { _createdOn = value; } }
        public int CreatedBy { get { return _createdBy; } set { _createdBy = value; } }
        public DateTime UpdatedOn { get { return _updatedOn; } set { _updatedOn = value; } }
        public int UpdatedBy { get { return _updatedBy; } set { _updatedBy = value; } }
        public DateTime? AuthorizedOn { get { return _authorizedOn; } set { _authorizedOn = value; } }
        public int? AuthorizedBy { get { return _authorizedBy; } set { _authorizedBy = value; } }
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
                {"RuleID", RuleID},
                {"Factor1Value", Factor1Value},
                {"Factor2Value", Factor2Value},
                {"Factor3Value", Factor3Value},
                {"Factor4Value", Factor4Value},
                {"Factor5Value", Factor5Value},
                {"Factor6Value", Factor6Value},
                {"Factor7Value", Factor7Value},
                {"Factor8Value", Factor8Value},
                {"Factor9Value", Factor9Value},
                {"Factor10Value", Factor10Value},
                {"Factor11Value", Factor11Value},
                {"Factor12Value", Factor12Value},
                {"Factor13Value", Factor13Value},
                {"Factor14Value", Factor14Value},
                {"Factor15Value", Factor15Value},
                {"Factor16Value", Factor16Value},
                {"Factor17Value", Factor17Value},
                {"Factor18Value", Factor18Value},
                {"Factor19Value", Factor19Value},
                {"Factor20Value", Factor20Value},
                {"RequirementSetID", RequirementSetID},
                {"CreatedOn", CreatedOn},
                {"CreatedBy", CreatedBy},
                {"UpdatedOn", UpdatedOn},
                {"UpdatedBy", UpdatedBy},
                {"AuthorizedOn", AuthorizedOn},
                {"AuthorizedBy", AuthorizedBy},
                {"LastChange", LastChange}
            };
        }

        #endregion

        #region Constructors

        public DecisionMatrix()
        {
            TableName = "DecisionMatrix";
            PrimaryKeyName = "ID";
        }

        #endregion
    }
}

