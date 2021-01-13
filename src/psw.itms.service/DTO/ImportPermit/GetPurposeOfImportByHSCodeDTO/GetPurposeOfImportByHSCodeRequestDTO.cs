using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetPurposeOfImportByHSCodeRequestDTO
    {
        [JsonPropertyName("agencyId")]
        public int AgencyId { get; set; }

        [JsonPropertyName("HSCode")]
        public string HSCode { get; set; }
       
    }

}