/*This code is a generated one , Change the source code of the generator if you want some change in this code
You can find the source code of the code generator from here -> https://git.psw.gov.pk/unais.vayani/DalGenerator*/

using System;
using System.Collections.Generic;
using System.Linq;


namespace PSW.ITMS.Data.Entities
{
    /// <summary>
    /// This class represents the Country table in the database 
    /// </summary>
	public class CountryWithDialingCodes : Entity
    {
        #region Fields

        private string _countryCode;
        private string _dialCode;
        private string _name;

        #endregion

        #region Properties

        public string CountryCode { get { return _countryCode; } set { _countryCode = value; PrimaryKey = value; } }
        public string DialCode { get { return _dialCode; } set { _dialCode = value; } }
        public string Name { get { return _name; } set { _name = value; } }

        #endregion

        #region Methods

        #endregion

        #region public Methods

        public override Dictionary<string, object> GetColumns()
        {
            return new Dictionary<string, object>
            {
                {"CountryCode", CountryCode},
                {"DialCode", DialCode},
                {"Name", Name}
            };
        }

        #endregion

        #region Constructors

        public CountryWithDialingCodes()
        {
            TableName = "CountryWithDialingCodes";
            PrimaryKeyName = "CountryCode";
        }

        #endregion
    }
}

