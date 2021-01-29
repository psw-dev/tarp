using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace PSW.ITMS.Service.DTO
{
    public class GetHSCodeRequirementsResponseDTO
    {
        [JsonPropertyName("id")]
        public long ID{ get; set; }

        [JsonPropertyName("HSCode")]
		public string HSCode{ get; set; }

        [JsonPropertyName("HSCodeExt")]
		public string HSCodeExt{ get; set; }
        
		[JsonPropertyName("itemDescription")]
        public string ItemDescription { get; set; }

        [JsonPropertyName("itemDescriptionExt")]
        public string ItemDescriptionExt { get; set; }
        
        [JsonPropertyName("UoM")]
        public UOMResponseDTO UoM { get; set; }
		
		
		[JsonPropertyName("technicalName")]
        public string TechnicalName { get; set; }
		
        [JsonPropertyName("requestedDocument")]
        public DocumentResponseDTO RequestedDocument{ get; set; }
        
        [JsonPropertyName("requirementStageID")]
		public int RequirementStageID{ get; set; }
		
        [JsonPropertyName("requestTypeID")]
        public int RequestTypeID{ get; set; }

		[JsonPropertyName("purposeLogicOperatorID")]
        public int PurposeLogicOperatorID { get; set; }
		
        [JsonPropertyName("purposesOfImport")]
        public IList<GetPurposeOfImportByHSCodeResponseDTO> purposesOfImport { get; set; }
		
		[JsonPropertyName("agencyID")]
		public short AgencyID { get; set; }

        [JsonPropertyName("requirementCategoryID")]
		public int RequirementCategoryID{ get; set; }

        [JsonPropertyName("requirementCategoryName")]
		public string RequirementCategoryName { get; set; }
        
        [JsonPropertyName("requiredDocument")]
		public DocumentResponseDTO RequiredDocument { get; set; }
    }

}