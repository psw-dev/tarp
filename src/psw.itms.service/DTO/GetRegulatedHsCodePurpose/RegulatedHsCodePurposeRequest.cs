using System.Text.Json.Serialization;
using System.Collections.Generic;


namespace PSW.ITMS.Service.DTO
{
    public class RegulatedHsCodePurposeRequestDTO
    {
        [JsonPropertyName("agencyId")]
        public int AgencyId { set; get; }

        [JsonPropertyName("documentTypeCode")]
        public string DocumentTypeCode { set; get; }

        [JsonPropertyName("tradeTranTypeId")]
        public int TradeTranTypeID { set; get; }
    }
}