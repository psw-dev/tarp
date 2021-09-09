using System.Text.Json.Serialization;


namespace PSW.ITMS.Service.DTO
{
    public class GetRegulatedHscodeListRequest
    {
        [JsonPropertyName("agencyId")]
        public int AgencyId { set; get; }

        [JsonPropertyName("documentTypeCode")]
        public string DocumentTypeCode { set; get; }
    }
}