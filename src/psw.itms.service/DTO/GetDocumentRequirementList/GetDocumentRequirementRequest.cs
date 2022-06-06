using System.Collections.Generic;
using System.Text.Json.Serialization;


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

        [JsonPropertyName("tradeTranTypeId")]
        public int TradeTranTypeID { get; set; }

        [JsonPropertyName("Quantity")]
        public decimal Quantity { get; set; }

        [JsonPropertyName("AgencyQuantity")]
        public decimal AgencyQuantity { get; set; }

        [JsonPropertyName("isFinancialRequirement")]
        public bool IsFinancialRequirement { get; set; }

        [JsonPropertyName("importExportValue")]
        public decimal ImportExportValue { get; set; }
    }

    public class FactorData
    {
        [JsonPropertyName("factorID")]
        public int FactorID { get; set; }

        [JsonPropertyName("factorLabel")]
        public string FactorLabel { get; set; }

        [JsonPropertyName("factorValue")]
        public string FactorValue { get; set; }

        [JsonPropertyName("factorValueID")]
        public string FactorValueID { get; set; }
    }
}