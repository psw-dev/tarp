using System;
using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetHSCodeListRequestDTO
    {
        
        [JsonPropertyName("agencyId")]
        public int AgencyID { get; set; }

    }

}