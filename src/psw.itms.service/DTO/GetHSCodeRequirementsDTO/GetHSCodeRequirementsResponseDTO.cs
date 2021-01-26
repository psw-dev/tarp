using System;
using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetHSCodeRequirementsResponseDTO
    {
        
        [JsonPropertyName("agencyId")]
        public string PurposeIDList { get; set; }

        [JsonPropertyName("technicalName")]
        public string TechnicalName { get; set; }

        [JsonPropertyName("itemDescription")]
        public string ItemDescription { get; set; }

        [JsonPropertyName("UoMId")]
        public int UoMID { get; set; }
    }

}