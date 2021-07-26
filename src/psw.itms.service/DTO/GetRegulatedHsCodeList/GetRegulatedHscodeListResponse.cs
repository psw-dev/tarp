using System.Text.Json.Serialization;
using System.Collections.Generic;
using PSW.ITMS.Data.Entities;

namespace PSW.ITMS.Service.DTO
{
    public class GetRegulatedHscodeListResponse
    {
         [JsonPropertyName("regulatedHsCodeList")]
        public List<ViewRegulatedHsCode> RegulatedHsCodeList { set; get; }
    }
}