using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetPurposeOfImportByHSCodeResponseDTO
    {
        [JsonPropertyName("HSCode")]
        public string HSCode { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
