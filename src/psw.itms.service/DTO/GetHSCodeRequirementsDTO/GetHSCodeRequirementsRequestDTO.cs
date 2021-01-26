using System;
using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetHSCodeRequirementsRequestDTO
    {
        
        [JsonPropertyName("agencyId")]
        public int AgencyID { get; set; }

        [JsonPropertyName("hsCode")]
        public string HSCode { get; set; }

    }

}