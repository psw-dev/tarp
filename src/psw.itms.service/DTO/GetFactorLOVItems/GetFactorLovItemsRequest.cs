using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace PSW.ITMS.Service.DTO
{
    public class GetFactorLovItemsRequest
    {
        [JsonPropertyName("factorList")]
        public List<int> FactorList { get; set; }
    }
}