using System.Collections.Generic;
using System.Text.Json.Serialization;
using PSW.ITMS.Data.Entities;
namespace PSW.ITMS.Service.DTO
{
    public class GetPCTCodeListResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("pctCodeList")]
        public List<ProductDetail> PctCodeList { get; set; }
    }
}