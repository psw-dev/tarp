using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace PSW.ITMS.Service.DTO
{
    public class GetHSCodeRequirementsResponseDTO
    {
        
        // [JsonPropertyName("purposeIDList")]
        // public string PurposeIDList { get; set; }

        // [JsonPropertyName("technicalName")]
        // public string TechnicalName { get; set; }

        // [JsonPropertyName("itemDescription")]
        // public string ItemDescription { get; set; }

        // [JsonPropertyName("UoMId")]
        // public int UoMID { get; set; }
        
        public IList<GetPurposeOfImportByHSCodeResponseDTO> purposesOfImport { get; set; }
        public IList<DocumentResponseDTO> documents { get; set; }

        public IList<UOMResponseDTO> UoMs { get; set; }
        public IList<ListOfRules> rules { get; set; }
    }

}