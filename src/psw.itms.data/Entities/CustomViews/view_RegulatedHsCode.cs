using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;


namespace PSW.ITMS.Data.Entities
{
    public class ViewRegulatedHsCode
    {
        [JsonPropertyName("hsCode")]
        public string HsCode { get; set; }

        [JsonPropertyName("productCode")]
        public List<string> ProductCode { get; set; }

        [JsonPropertyName("itemDescription")]
        public string ItemDescription { get; set; }

        [JsonPropertyName("itemDescriptionExt")]
        public string ItemDescriptionExt { get; set; }

        [JsonPropertyName("agencyId")]
        public int AgencyID { get; set; }

        [JsonPropertyName("technicalName")]
        public string TechnicalName { get; set; }
    }
}
