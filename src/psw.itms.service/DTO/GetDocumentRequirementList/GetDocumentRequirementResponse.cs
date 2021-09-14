using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace PSW.ITMS.Service.DTO
{
    public class GetDocumentRequirementResponse
    {
        [JsonPropertyName("documentaryRequirementList")]
        public List<DocumentaryRequirement> DocumentaryRequirementList { get; set; }
        public FinancialRequirement FinancialRequirement { get; set; }
        public ValidityRequirement ValidityRequirement { get; set; }
    }

    public class DocumentaryRequirement
    {
        //Common for all Requirements

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("isMandatory")]
        public bool IsMandatory { get; set; }

        [JsonPropertyName("requirementType")]
        public string RequirementType { get; set; }

        [JsonPropertyName("documentTypeCode")]
        public string DocumentTypeCode { get; set; }

        [JsonPropertyName("documentName")]
        public string DocumentName { get; set; }

        [JsonPropertyName("attachedObjectFormatID")]
        public int AttachedObjectFormatID { get; set; }
    }

    public class FinancialRequirement
    {
        [JsonPropertyName("postingBillingAccountID")]
        public string PostingBillingAccountID { get; set; }

        [JsonPropertyName("amount")]
        public string Amount { get; set; }
    }

    public class ValidityRequirement
    {
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("uomName")]
        public string UomName { get; set; }
    }
}