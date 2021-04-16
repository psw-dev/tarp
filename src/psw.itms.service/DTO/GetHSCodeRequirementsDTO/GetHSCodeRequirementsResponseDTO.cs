using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace PSW.ITMS.Service.DTO
{
    public class GetHsCodeRequirementsResponseDto
    {
        public GetHsCodeRequirementsResponseDto()
        {
            PurposeWiseRequirements = new List<PurposeWiseRequirement>();
        }

        [JsonPropertyName("HSCodeExt")]
		public string HsCodeExt { get; set; }
        
		[JsonPropertyName("itemDescription")]
        public string ItemDescription { get; set; }

        [JsonPropertyName("UoM")]
        public UOMResponseDTO UoM { get; set; }

        [JsonPropertyName("technicalName")]
        public string TechnicalName { get; set; }

        [JsonPropertyName("requestedDocumentCode")]
        public string RequestedDocumentCode { get; set; }

        [JsonPropertyName("isFscrdEnlistmentRequired")]
        public bool IsFscrdEnlistmentRequired { get; set; }

        [JsonPropertyName("isNotPermitted")]
        public bool IsNotPermitted { get; set; }

        [JsonPropertyName("purposeWiseRequirements")]
        public List<PurposeWiseRequirement> PurposeWiseRequirements { get; set; }
    }

    public class PurposeWiseRequirement
    {
        public PurposeWiseRequirement()
        {
            TradePurposes = new List<GetPurposeOfImportByHSCodeResponseDTO>();
            RequiredDocument = new List<DocumentResponseDTO>();
        }

        [JsonPropertyName("purposesOfImport")]
        public IList<GetPurposeOfImportByHSCodeResponseDTO> TradePurposes { get; set; }

        [JsonPropertyName("billAmount")]
        public string BillAmount { get; set; }

        [JsonPropertyName("requiredDocument")]
        public List<DocumentResponseDTO> RequiredDocument { get; set; }
    }
}