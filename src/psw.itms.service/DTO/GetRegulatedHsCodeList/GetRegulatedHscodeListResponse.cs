using PSW.ITMS.Data.Entities;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetRegulatedHscodeListResponse
    {
        [JsonPropertyName("regulatedHsCodeList")]
        public List<ViewRegulatedHsCode> RegulatedHsCodeList { set; get; }
    }
}