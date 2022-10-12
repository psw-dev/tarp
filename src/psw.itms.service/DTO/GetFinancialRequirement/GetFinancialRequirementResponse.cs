using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetFinancialRequirementResponse
    {
        public FinancialReq DocumentFinancialRequirement { get; set; }
    }

    public class FinancialReq
    {

        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("plainAmount")]
        public string PlainAmount { get; set; }

        [JsonPropertyName("ammendmentFee")]
        public string AmmendmentFee { get; set; }

        [JsonPropertyName("plainAmmendmentFee")]
        public string PlainAmmendmentFee { get; set; }

        [JsonPropertyName("extensionFee")]
        public string ExtensionFee { get; set; }

        [JsonPropertyName("plainExtensionFee")]
        public string PlainExtensionFee { get; set; }

        [JsonPropertyName("additionalAmount")]
        public decimal AdditionalAmount { get; set; }

        [JsonPropertyName("additionalAmountOn")]
        public string AdditionalAmountOn { get; set; }

        [JsonPropertyName("documentTypeCode")]
        public string documentTypeCode { get; set; }
    }
}