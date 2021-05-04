using System.Text.Json.Serialization;
using System.Collections.Generic;
using PSW.ITMS.Data.Repositories;

namespace PSW.ITMS.Service.DTO
{
    public class GetListOfAgencyAgainstHscodeResponse
    {
        [JsonPropertyName("agencyList")]
        public List<AgencyList> AgencyList { get; set; }
    }

    // public class AgencyList
    // {
    //     [JsonPropertyName("agencyName")]
    //     public string AgencyName { get; set; }

    //     [JsonPropertyName("agencyId")]
    //     public string AgencyId { get; set; }
    // }
}