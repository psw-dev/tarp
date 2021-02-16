using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetHsCodeRequirementsRequestDto
    {
        [JsonPropertyName("agencyId")]
        public int AgencyId { get; set; }

        [JsonPropertyName("hsCode")]
        public string HsCode { get; set; }

        [JsonPropertyName("documentTypeCode")]
        public string DocumentTypeCode { get; set; }

        [JsonPropertyName("tradeTranType")]
        public int TradeTranType { get; set; }
    }
}