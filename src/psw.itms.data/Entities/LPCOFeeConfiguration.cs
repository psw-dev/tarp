using System;
using System.Collections.Generic;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the LPCOFeeConfiguration table in the database 
    /// </summary>
	public class LPCOFeeConfiguration : Entity
    {
        #region Fields

        public long _iD;
        public short _agencyID;
        public string _hSCode;
        public string _productCode;
        public byte? _oGAItemCategoryID;
        public int? _unit_ID;
        public string _calculationBasis;
        public string _calculationSource;
        public int? _qtyRangeTo;
        public int? _qtyRangeFrom;
        public string _currencyCode;
        public decimal? _rate;
        public DateTime _effectiveFromDt;
        public DateTime? _effectiveThruDt;
        public short? _tradeTranTypeID;
        public decimal? _minAmount;
        public decimal? _additionalAmount;
        #endregion

        #region Properties

        public long ID { get { return _iD; } set { _iD = value; PrimaryKey = value; } }
        public short AgencyID { get { return _agencyID; } set { _agencyID = value; } }
        public string HSCode { get { return _hSCode; } set { _hSCode = value; } }
        public string ProductCode { get { return _productCode; } set { _productCode = value; } }
        public byte? OGAItemCategoryID { get { return _oGAItemCategoryID; } set { _oGAItemCategoryID = value; } }
        public int? Unit_ID { get { return _unit_ID; } set { _unit_ID = value; } }
        public string CalculationBasis { get { return _calculationBasis; } set { _calculationBasis = value; } }
        public string CalculationSource { get { return _calculationSource; } set { _calculationSource = value; } }
        public int? QtyRangeTo { get { return _qtyRangeTo; } set { _qtyRangeTo = value; } }
        public int? QtyRangeFrom { get { return _qtyRangeFrom; } set { _qtyRangeFrom = value; } }
        public string CurrencyCode { get { return _currencyCode; } set { _currencyCode = value; } }
        public decimal? Rate { get { return _rate; } set { _rate = value; } }
        public DateTime EffectiveFromDt { get { return _effectiveFromDt; } set { _effectiveFromDt = value; } }
        public DateTime? EffectiveThruDt { get { return _effectiveThruDt; } set { _effectiveThruDt = value; } }
        public short? TradeTranTypeID { get { return _tradeTranTypeID; } set { _tradeTranTypeID = value; } }
        public decimal? MinAmount { get { return _rate; } set { _rate = value; } }
        public decimal? AdditionalAmount { get { return _rate; } set { _rate = value; } }


        #endregion

        #region Methods

        #endregion

        #region public Methods

        public override Dictionary<string, object> GetColumns()
        {
            return new Dictionary<string, object>
            {
                {"ID", ID},
                {"HSCode", HSCode},
                {"ProductCode", ProductCode},
                {"OGAItemCategoryID", OGAItemCategoryID},
                {"Unit_ID", Unit_ID},
                {"CalculationBasis", CalculationBasis},
                {"CalculationSource", CalculationSource},
                {"QtyRangeTo", QtyRangeTo},
                {"QtyRangeFrom", QtyRangeFrom},
                {"CurrencyCode", CurrencyCode},
                {"Rate", Rate},
                {"EffectiveFromDt", EffectiveFromDt},
                {"EffectiveThruDt", EffectiveThruDt},
                {"TradeTranTypeID", TradeTranTypeID},
                {"MinAmount", MinAmount},
                {"AdditionalAmount", AdditionalAmount}
            };
        }

        #endregion

        #region Constructors

        public LPCOFeeConfiguration()
        {
            TableName = "LPCOFeeConfiguration";
            PrimaryKeyName = "ID";
        }

        #endregion
    }
}

