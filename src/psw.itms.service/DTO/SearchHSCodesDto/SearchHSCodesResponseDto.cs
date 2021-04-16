using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace PSW.ITMS.Service.DTO
{
    public class SearchHSCodesResponseDto
    {
        [JsonPropertyName("hsCodes")]
        public List<HSCodesData> HSCodes { get; set; }
    }

    public class HSCodesData
    {
        [JsonPropertyName("hsCode")]
        public string HSCode { get; set; }        

        [JsonPropertyName("pctCode")]
        public string HSCodeExt { get; set; }

        [JsonPropertyName("commodityName")]
        public string ItemDescription { get; set; }
        
        [JsonPropertyName("technicalName")]
        public string TechnicalName { get; set; }
    }
}
