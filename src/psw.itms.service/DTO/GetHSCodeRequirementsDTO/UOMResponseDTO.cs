using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class UOMResponseDTO
    {  
        [JsonPropertyName("id")]
        public int ID { get; set; }  
        
        [JsonPropertyName("name")]
        public string Name { get; set; }    
    }

}