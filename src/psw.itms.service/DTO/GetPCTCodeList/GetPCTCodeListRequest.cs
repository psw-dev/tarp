using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetPCTCodeListRequest
    {
        [JsonPropertyName("hsCode")]
        public string HsCode { get; set; }
    }
}