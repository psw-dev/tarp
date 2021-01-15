using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class MockDataRequestDTO
    {
        [JsonPropertyName("agencyId")]
        public int AgencyId { get; set; }

        [JsonPropertyName("HSCode")]
        public string HSCode { get; set; }

        [JsonPropertyName("importPurpose")]
        public string ImportPurpose { get; set; }

        [JsonPropertyName("request")]
        public string Request { get; set; }
    }
}
