using System;
using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetHSCodeListResponseDTO
    {
        [JsonPropertyName("id")]
        public long ID { get; set; }

        [JsonPropertyName("HSCode1")]
        public string HSCode1 { get; set; }

        [JsonPropertyName("HSCode2")]
        public string HSCode2 { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("uomId")]
        public System.SByte UOMId { get; set; }

        // [JsonPropertyName("createdOn")]
        // public DateTime CreatedOn { get; set; }

        // [JsonPropertyName("createdBy")]
        // public int CreatedBy { get; set; }
        
        // [JsonPropertyName("updatedOn")]
        // public DateTime UpdatedOn { get; set; }
        
        // [JsonPropertyName("updatedBy")]
        // public int UpdatedBy { get; set; }

    }

}