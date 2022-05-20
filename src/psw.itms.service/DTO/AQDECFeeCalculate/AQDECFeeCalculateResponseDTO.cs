using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace PSW.ITMS.Service.DTO
{
    public class AQDECFeeCalculateResponseDTO
    {
        [JsonPropertyName("amount")]
        public string Amount { get; set; }

    }
}