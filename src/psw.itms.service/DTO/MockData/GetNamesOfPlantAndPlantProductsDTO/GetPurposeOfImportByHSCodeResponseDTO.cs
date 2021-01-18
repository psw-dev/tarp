using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetNamesOfPlantAndPlantProductsResponseDTO
    {
        [JsonPropertyName("HSCode")]
        public string HSCode { get; set; }

        [JsonPropertyName("purpose")]
        public string Purpose { get; set; }

        [JsonPropertyName("tecnicalName")]
        public string Name { get; set; }
    }
}
