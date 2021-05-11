using System.Text.Json.Serialization;
using System.Collections.Generic;
using PSW.ITMS.Data.Entities;

namespace PSW.ITMS.Service.DTO
{
    public class GetListOfAgencyAgainstHscodeResponse
    {
        [JsonPropertyName("agencyList")]
        public List<AgencyList> AgencyList { get; set; }
    }

}