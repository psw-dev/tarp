using System;
using System.Collections.Generic;
using System.Text;

namespace PSW.ITMS.Common.Constants
{
    public static class ErrorCategories
    {
        #region Public Fields

        public const string Authorization = "Authorization";
        public const string BadRequest = "BadRequest";
        public const string BusinessRule = "BusinessRule";
        public const string Identification = "Identification";
        public const string InternalServer = "InternalServer";
        public const string NotFound = "NotFound";
        public const string ServiceUnavailable = "ServiceUnavailable";
        public const string Validation = "Validation";

        #endregion Public Fields
    }

    public static class Ref_Units
    {
        public const string Piece = "APT";
        public const string Cage = "CGE";
        public const string Box = "BX";
        public const string MuttonCarcass = "MC";
        public const string BeefQuartrer = "BQ";
        public const string PackingUnits = "PU";
    }
}
