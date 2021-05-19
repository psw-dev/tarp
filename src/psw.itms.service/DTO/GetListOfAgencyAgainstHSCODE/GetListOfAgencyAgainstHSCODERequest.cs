using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetListOfAgencyAgainstHscodeRequest
    {
        [JsonPropertyName("hsCode")]
        public string HsCode { get; set; }

        [JsonPropertyName("documentCode")]
        public string DocumentCode { get; set; }
    }
}