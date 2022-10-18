using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace PSW.ITMS.Service.DTO
{
    public class ValidateRegulatedHSCodesRequest
    {
        [JsonPropertyName("hsCodes")]
        public List<string> HSCodes { set; get; }
        
    }
}