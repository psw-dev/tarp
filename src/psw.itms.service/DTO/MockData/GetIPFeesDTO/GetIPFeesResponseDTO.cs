using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetIPFeesResponseDTO
    {
        [JsonPropertyName("HSCode")]
        public string HSCode { get; set; }

        [JsonPropertyName("IPFees")]
        public string IPFees { get; set; }
    }
}
