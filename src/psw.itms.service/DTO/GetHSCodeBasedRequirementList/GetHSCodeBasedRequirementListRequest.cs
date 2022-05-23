using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace PSW.ITMS.Service.DTO
{
    public class GetHSCodeBasedRequirementListRequest
    {
        // [JsonPropertyName("hsCodeList")]
        // public List<string> HsCodeList { get; set; }
        [JsonPropertyName("hsCodeFactorList")]
        public List<HSCodeFactor> HsCodeFactorList { get; set; }

        [JsonPropertyName("agencyId")]
        public string AgencyId { get; set; }

        [JsonPropertyName("documentTypeCode")]
        public string documentTypeCode { get; set; }

        // [JsonPropertyName("factorLabelValuePairList")]
        // public Dictionary<string, FactorData> FactorCodeValuePair { get; set; }

        [JsonPropertyName("tradeTranTypeId")]
        public int TradeTranTypeID { get; set; }
    }

    public class HSCodeFactor
    {
        [JsonPropertyName("hsCode")]
        public string HsCode { get; set; }

        [JsonPropertyName("factorLabelValuePairList")]
        public Dictionary<string, FactorData> FactorCodeValuePair { get; set; }

    }
    // public class FactorData
    // {
    //     [JsonPropertyName("factorID")]
    //     public int FactorID { get; set; }

    //     [JsonPropertyName("factorLabel")]
    //     public string FactorLabel { get; set; }

    //     [JsonPropertyName("factorValue")]
    //     public string FactorValue { get; set; }
    // }
}