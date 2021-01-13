using System;
using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetImportPermitsResponseDTO
    {

        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("traderOrAgentName")]
        public string TraderOrAgentName { get; set; }
    
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        
        [JsonPropertyName("HSCode")]
        public string HSCode { get; set; }
    
        [JsonPropertyName("commodityName")]
        public string CommodityName { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    
    }

}

    // Sno 
    // Trader Name/ Agent Name 
    // Import Permit 
    // Date 
    // HS Code 
    // Commodity Name 
    // Status  
