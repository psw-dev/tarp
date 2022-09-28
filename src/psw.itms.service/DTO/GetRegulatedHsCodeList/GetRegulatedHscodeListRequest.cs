using System.Text.Json.Serialization;


namespace PSW.ITMS.Service.DTO
{
    public class GetRegulatedHscodeListRequest
    {
        [JsonPropertyName("agencyId")]
        public int AgencyId { set; get; }

        [JsonPropertyName("chapter")]
        public string Chapter { set; get; }

        [JsonPropertyName("documentTypeCode")]
        public string DocumentTypeCode { set; get; }
    }
}