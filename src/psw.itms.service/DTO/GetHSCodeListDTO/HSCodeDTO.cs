using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class HSCodeDTO
    {
        [JsonPropertyName("hsCode")]
        public string HSCode { get; set; }

        [JsonPropertyName("pctCodes")]
        public List<string> HSCodeExt { get; set; }
    }

}