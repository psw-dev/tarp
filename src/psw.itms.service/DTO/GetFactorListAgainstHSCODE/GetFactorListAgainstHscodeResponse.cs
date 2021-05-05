using System.Text.Json.Serialization;
using System.Collections.Generic;

using PSW.ITMS.Data.Repositories;

namespace PSW.ITMS.Service.DTO
{
    public class GetFactorListAgainstHscodeResponse
    {
        [JsonPropertyName("factorList")]
        public List<Factors> FactorList { get; set; }
    }
}