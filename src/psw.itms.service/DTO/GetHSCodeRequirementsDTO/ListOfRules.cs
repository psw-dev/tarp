using System;
using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class ListOfRules
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
        public string ItemDescriptionExt;
        
        [JsonPropertyName("UoMId")]
        public int UoMID { get; set; }
		
		
		[JsonPropertyName("technicalName")]
        public string TechnicalName { get; set; }
		
        [JsonPropertyName("requestedDocument")]
        public string RequestedDocument{ get; set; }
        
        [JsonPropertyName("requirementStageID")]
		public int RequirementStageID{ get; set; }
		
        [JsonPropertyName("requestTypeID")]
        public int RequestTypeID{ get; set; }

		[JsonPropertyName("purposeLogicOperatorID")]
        public int PurposeLogicOperatorID { get; set; }
		
        [JsonPropertyName("purposeIDList")]
        public string PurposeIDList { get; set; }
		
		[JsonPropertyName("agencyID")]
		public short AgencyID { get; set; }

        [JsonPropertyName("requirementCategoryID")]
		public int RequirementCategoryID{ get; set; }

        [JsonPropertyName("requirementCategoryName")]
		public string RequirementCategoryName { get; set; }
        
        [JsonPropertyName("requiredDocumentTypeCode")]
		public string RequiredDocumentTypeCode { get; set; }
    }

}