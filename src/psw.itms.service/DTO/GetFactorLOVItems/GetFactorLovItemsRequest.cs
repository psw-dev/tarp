using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace PSW.ITMS.Service.DTO
{
    public class GetFactorLovItemsRequest
    {
        [JsonPropertyName("factorList")]
        public List<string> FactorList { get; set; }
    }
}