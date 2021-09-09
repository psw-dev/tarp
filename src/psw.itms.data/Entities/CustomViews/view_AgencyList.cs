using System.Text.Json.Serialization;


namespace PSW.ITMS.Data.Entities
{
    public class AgencyList
    {
        [JsonPropertyName("agencyID")]
        public int Id { get; set; }

        [JsonPropertyName("agencyName")]
        public string Name { get; set; }
    }
}
