using PSW.ITMS.Data.Entities;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetFactorListAgainstHscodeResponse
    {
        [JsonPropertyName("factorList")]
        public List<Factors> FactorList { get; set; }
    }
}