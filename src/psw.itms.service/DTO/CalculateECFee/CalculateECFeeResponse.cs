using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace PSW.ITMS.Service.DTO
{
    public class CalculateECFeeResponse
    {
       [JsonPropertyName("amount")]
        public string Amount { get; set; }

    }
}