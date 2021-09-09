/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the Rule table in the database 
    /// </summary>
	public class Rule : Entity
    {
        #region Fields

        private long _iD;
        private long _factor1ID;
        private string _binaryOperator1;
        private long _factor2ID;
        private string _binaryOperator2;
        private long _factor3ID;
        private string _binaryOperator3;
        private long _factor4ID;
        private string _binaryOperator4;
        private long _factor5ID;
        private string _binaryOperator5;
        private long _factor6ID;
        private string _binaryOperator6;
        private long _factor7ID;
        private string _binaryOperator7;
        private long _factor8ID;
        private string _binaryOperator8;
        private long _factor9ID;
        private string _binaryOperator9;
        private long _factor10ID;
        private string _binaryOperator10;
        private long _factor11ID;
        private string _binaryOperator11;
        private long _factor12ID;
        private string _binaryOperator12;
        private long _factor13ID;
        private string _binaryOperator13;
        private long _factor14ID;
        private string _binaryOperator14;
        private long _factor15ID;
        private string _binaryOperator15;
        private long _factor16ID;
        private string _binaryOperator16;
        private long _factor17ID;
        private string _binaryOperator17;
        private long _factor18ID;
        private string _binaryOperator18;
        private long _factor19ID;
        private string _binaryOperator19;
        private long _factor20ID;
        private string _binaryOperator20;
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
        public long Factor1ID { get { return _factor1ID; } set { _factor1ID = value; } }
        public string BinaryOperator1 { get { return _binaryOperator1; } set { _binaryOperator1 = value; } }
        public long Factor2ID { get { return _factor2ID; } set { _factor2ID = value; } }
        public string BinaryOperator2 { get { return _binaryOperator2; } set { _binaryOperator2 = value; } }
        public long Factor3ID { get { return _factor3ID; } set { _factor3ID = value; } }
        public string BinaryOperator3 { get { return _binaryOperator3; } set { _binaryOperator3 = value; } }
        public long Factor4ID { get { return _factor4ID; } set { _factor4ID = value; } }
        public string BinaryOperator4 { get { return _binaryOperator4; } set { _binaryOperator4 = value; } }
        public long Factor5ID { get { return _factor5ID; } set { _factor5ID = value; } }
        public string BinaryOperator5 { get { return _binaryOperator5; } set { _binaryOperator5 = value; } }
        public long Factor6ID { get { return _factor6ID; } set { _factor6ID = value; } }
        public string BinaryOperator6 { get { return _binaryOperator6; } set { _binaryOperator6 = value; } }
        public long Factor7ID { get { return _factor7ID; } set { _factor7ID = value; } }
        public string BinaryOperator7 { get { return _binaryOperator7; } set { _binaryOperator7 = value; } }
        public long Factor8ID { get { return _factor8ID; } set { _factor8ID = value; } }
        public string BinaryOperator8 { get { return _binaryOperator8; } set { _binaryOperator8 = value; } }
        public long Factor9ID { get { return _factor9ID; } set { _factor9ID = value; } }
        public string BinaryOperator9 { get { return _binaryOperator9; } set { _binaryOperator9 = value; } }
        public long Factor10ID { get { return _factor10ID; } set { _factor10ID = value; } }
        public string BinaryOperator10 { get { return _binaryOperator10; } set { _binaryOperator10 = value; } }
        public long Factor11ID { get { return _factor11ID; } set { _factor11ID = value; } }
        public string BinaryOperator11 { get { return _binaryOperator11; } set { _binaryOperator11 = value; } }
        public long Factor12ID { get { return _factor12ID; } set { _factor12ID = value; } }
        public string BinaryOperator12 { get { return _binaryOperator12; } set { _binaryOperator12 = value; } }
        public long Factor13ID { get { return _factor13ID; } set { _factor13ID = value; } }
        public string BinaryOperator13 { get { return _binaryOperator13; } set { _binaryOperator13 = value; } }
        public long Factor14ID { get { return _factor14ID; } set { _factor14ID = value; } }
        public string BinaryOperator14 { get { return _binaryOperator14; } set { _binaryOperator14 = value; } }
        public long Factor15ID { get { return _factor15ID; } set { _factor15ID = value; } }
        public string BinaryOperator15 { get { return _binaryOperator15; } set { _binaryOperator15 = value; } }
        public long Factor16ID { get { return _factor16ID; } set { _factor16ID = value; } }
        public string BinaryOperator16 { get { return _binaryOperator16; } set { _binaryOperator16 = value; } }
        public long Factor17ID { get { return _factor17ID; } set { _factor17ID = value; } }
        public string BinaryOperator17 { get { return _binaryOperator17; } set { _binaryOperator17 = value; } }
        public long Factor18ID { get { return _factor18ID; } set { _factor18ID = value; } }
        public string BinaryOperator18 { get { return _binaryOperator18; } set { _binaryOperator18 = value; } }
        public long Factor19ID { get { return _factor19ID; } set { _factor19ID = value; } }
        public string BinaryOperator19 { get { return _binaryOperator19; } set { _binaryOperator19 = value; } }
        public long Factor20ID { get { return _factor20ID; } set { _factor20ID = value; } }
        public string BinaryOperator20 { get { return _binaryOperator20; } set { _binaryOperator20 = value; } }
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
                {"Factor1ID", Factor1ID},
                {"BinaryOperator1", BinaryOperator1},
                {"Factor2ID", Factor2ID},
                {"BinaryOperator2", BinaryOperator2},
                {"Factor3ID", Factor3ID},
                {"BinaryOperator3", BinaryOperator3},
                {"Factor4ID", Factor4ID},
                {"BinaryOperator4", BinaryOperator4},
                {"Factor5ID", Factor5ID},
                {"BinaryOperator5", BinaryOperator5},
                {"Factor6ID", Factor6ID},
                {"BinaryOperator6", BinaryOperator6},
                {"Factor7ID", Factor7ID},
                {"BinaryOperator7", BinaryOperator7},
                {"Factor8ID", Factor8ID},
                {"BinaryOperator8", BinaryOperator8},
                {"Factor9ID", Factor9ID},
                {"BinaryOperator9", BinaryOperator9},
                {"Factor10ID", Factor10ID},
                {"BinaryOperator10", BinaryOperator10},
                {"Factor11ID", Factor11ID},
                {"BinaryOperator11", BinaryOperator11},
                {"Factor12ID", Factor12ID},
                {"BinaryOperator12", BinaryOperator12},
                {"Factor13ID", Factor13ID},
                {"BinaryOperator13", BinaryOperator13},
                {"Factor14ID", Factor14ID},
                {"BinaryOperator14", BinaryOperator14},
                {"Factor15ID", Factor15ID},
                {"BinaryOperator15", BinaryOperator15},
                {"Factor16ID", Factor16ID},
                {"BinaryOperator16", BinaryOperator16},
                {"Factor17ID", Factor17ID},
                {"BinaryOperator17", BinaryOperator17},
                {"Factor18ID", Factor18ID},
                {"BinaryOperator18", BinaryOperator18},
                {"Factor19ID", Factor19ID},
                {"BinaryOperator19", BinaryOperator19},
                {"Factor20ID", Factor20ID},
                {"BinaryOperator20", BinaryOperator20},
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

        public Rule()
        {
            TableName = "Rule";
            PrimaryKeyName = "ID";
        }

        #endregion
    }
}

