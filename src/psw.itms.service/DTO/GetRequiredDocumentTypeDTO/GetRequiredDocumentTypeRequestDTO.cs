using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetRequiredDocumentTypeRequestDTO
    {
        [JsonPropertyName("agencyId")]
        public int AgencyId { get; set; }

        [JsonPropertyName("HSCode")]
        public string HSCode { get; set; }        

        [JsonPropertyName("importPurposeId")]
        public int ImportPurposeID { get; set; }

    }
}
