using System.Text.Json.Serialization;

using System.Collections.Generic;

namespace PSW.ITMS.Data.Entities
{
    public class AgencyList
    {
        [JsonPropertyName("agencyID")]
        public int Id { get; set; }

        [JsonPropertyName("agencyName")]
        public string Name { get; set; }

        [JsonPropertyName("documentCodeList")]
        public List<string> DocumentCodeList { get; set; }
    }
}
