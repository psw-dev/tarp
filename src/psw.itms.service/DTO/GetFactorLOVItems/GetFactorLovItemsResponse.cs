using System.Text.Json.Serialization;
using System.Collections.Generic;

using PSW.ITMS.Data.Entities;

namespace PSW.ITMS.Service.DTO
{
    public class GetFactorLovItemsResponse
    {
        [JsonPropertyName("factorLOVItemsList")]
        public List<FactorLOVItemsData> FactorLOVItemsList { get; set; }
    }

    public class FactorLOVItemsData 
    {
        [JsonPropertyName("factorLabel")]
        public string FactorLabel { get; set; }

        [JsonPropertyName("factorLOVItems")]
        public List<FactorLOVItems> FactorLOVItems { get; set; }
    }
}