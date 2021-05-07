using System.Text.Json.Serialization;
using System.Collections.Generic;


namespace PSW.ITMS.Service.DTO
{
    public class GetDocumentRequirementResponse
    {
        [JsonPropertyName("documentaryRequirementList")]
        public List<DocumentaryRequirement> DocumentaryRequirementList { get; set; }
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

        //Properties for Documentary Requirement

        [JsonPropertyName("documentTypeCode")]
        public string DocumentTypeCode { get; set; }

        [JsonPropertyName("documentName")]
        public string DocumentName { get; set; }

        //Properties for Financial Requirement

        [JsonPropertyName("postingBillingAccountID")]
        public string PostingBillingAccountID { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        //Properties for Nil Requirements

        [JsonPropertyName("displayText")]
        public string DisplayText { get; set; }

        //Properties for Testing Requirements

        [JsonPropertyName("testID")]
        public string TestID { get; set; }

        //Properties for ValidityTerm Requirement

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("uomName")]
        public string UomName { get; set; }
    }
}