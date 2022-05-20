using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace PSW.ITMS.Service.DTO
{
    public class CalculateECFeeRequest
    { 
        [JsonPropertyName("hsCode")]
        public string HsCode { get; set; }

        [JsonPropertyName("agencyId")]
        public int AgencyId { get; set; }

        [JsonPropertyName("tradeTranTypeId")]
        public int TradeTranTypeID { get; set; }

        [JsonPropertyName("oGAItemCategoryID")]
        public int OGAItemCategoryID { get; set; }

        [JsonPropertyName("uomId")]
        public int UoMId { get; set; }

        [JsonPropertyName("quantity")]
        public int? Quantity { get; set; }



    }
}