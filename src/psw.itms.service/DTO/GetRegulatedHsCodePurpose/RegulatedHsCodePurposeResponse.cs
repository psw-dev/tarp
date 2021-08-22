using System.Text.Json.Serialization;
using System.Collections.Generic;


namespace PSW.ITMS.Service.DTO
{
    public class RegulatedHsCodePurposeResponseDTO
    {
        [JsonPropertyName("regulatedHscodePurposeList")]
        public List<RegulatedHsCodePurpose> RegulatedHsCodePurposeList { get; set; }
    }

    public class RegulatedHsCodePurpose
    {
        [JsonPropertyName("hsCode")]
        public string HsCode { get; set; }

        [JsonPropertyName("purposeList")]
        public List<string> PurposeList { get; set; }
    }
}