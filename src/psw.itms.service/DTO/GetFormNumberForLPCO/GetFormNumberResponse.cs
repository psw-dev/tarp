using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace PSW.ITMS.Service.DTO
{
    public class GetFormNumberResponseDTO
    {
        [JsonPropertyName("formNumber")]
        public string FormNumber { get; set; }
    }
}