using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace PSW.ITMS.Service.DTO
{
    public class ValidateRegulatedHSCodesRequest
    {
        [JsonPropertyName("hsCodes")]
        public List<string> HSCodes { set; get; }
        
        [JsonPropertyName("agencyId")]
        public int AgencyID { set; get; }
        
        [JsonPropertyName("tradeTranTypeId")]
        public int TradeTranTypeId { set; get; }

        
    }
}