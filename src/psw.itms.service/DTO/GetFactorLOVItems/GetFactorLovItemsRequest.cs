using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetFactorLovItemsRequest
    {
        [JsonPropertyName("factorList")]
        public List<int> FactorList { get; set; }
    }
}