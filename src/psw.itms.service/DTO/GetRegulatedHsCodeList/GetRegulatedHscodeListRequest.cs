using System.Text.Json.Serialization;
using System.Collections.Generic;


namespace PSW.ITMS.Service.DTO
{
    public class GetRegulatedHscodeListRequest
    {
        [JsonPropertyName("agencyId")]
        public string AgencyId { set; get; }
    }
}