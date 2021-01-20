using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetPurposeOfImportByHSCodeResponseDTO
    {
        [JsonPropertyName("Id")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
