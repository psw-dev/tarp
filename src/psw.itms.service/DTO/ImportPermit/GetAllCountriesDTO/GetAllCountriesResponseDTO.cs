using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetAllCountriesResponseDTO
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("codeA3")]
        public string CodeA3 { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

}