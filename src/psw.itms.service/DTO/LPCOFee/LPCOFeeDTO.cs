using System.Text.Json.Serialization;


namespace PSW.ITMS.Service.DTO
{
    public class LPCOFeeDTO
    { 
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }

        [JsonPropertyName("additionalAmount")]
        public decimal AdditionalAmount { get; set; }

        [JsonPropertyName("additionalAmountOn")]
        public string AdditionalAmountOn { get; set; }
    }
}