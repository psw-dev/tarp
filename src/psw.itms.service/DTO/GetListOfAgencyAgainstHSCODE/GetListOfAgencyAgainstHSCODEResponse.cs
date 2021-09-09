using PSW.ITMS.Data.Entities;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetListOfAgencyAgainstHscodeResponse
    {
        [JsonPropertyName("agencyList")]
        public List<AgencyList> AgencyList { get; set; }
    }

}