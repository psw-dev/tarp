using System.Text.Json.Serialization;
using System.Collections.Generic;


namespace PSW.ITMS.Service.DTO
{
    public class GetDocumentRequirementRequest
    {
        [JsonPropertyName("hsCode")]
        public string HsCode { get; set; }

        [JsonPropertyName("agencyId")]
        public string AgencyId { get; set; }

        [JsonPropertyName("documentTypeCode")]
        public string documentTypeCode { get; set; }

        [JsonPropertyName("factorLabelValuePairList")]
        public Dictionary<string, FactorData> FactorCodeValuePair { get; set; }
    }

    public class FactorData
    {
        [JsonPropertyName("factorID")]
        public int FactorID { get; set; }

        [JsonPropertyName("factorLabel")]
        public string FactorLabel { get; set; }

        [JsonPropertyName("factorValue")]
        public string FactorValue { get; set; }
    }
}