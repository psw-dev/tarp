using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetNamesOfPlantAndPlantProductsResponseDTO
    {
        
        [JsonPropertyName("tecnicalName")]
        public string Name { get; set; }
    }
}
