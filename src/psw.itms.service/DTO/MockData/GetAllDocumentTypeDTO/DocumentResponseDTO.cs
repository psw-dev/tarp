using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class DocumentResponseDTO
    {  
        [JsonPropertyName("code")]
        public string DocumentCode { get; set; }  
        
        [JsonPropertyName("name")]
        public string DocumentName { get; set; }    
    }

}