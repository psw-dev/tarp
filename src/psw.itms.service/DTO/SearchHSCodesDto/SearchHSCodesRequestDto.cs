using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class SearchHSCodesRequestDto
    {
        [JsonPropertyName("agencyId")]
        public int AgencyId { get; set; }

        [JsonPropertyName("hsCode")]
        public string HSCode { get; set; }        

        [JsonPropertyName("pctCode")]
        public string PCTCode { get; set; }

        [JsonPropertyName("commodityName")]
        public string CommodityName { get; set; }
        
        [JsonPropertyName("technicalName")]
        public string TechnicalName { get; set; }
    }
}
