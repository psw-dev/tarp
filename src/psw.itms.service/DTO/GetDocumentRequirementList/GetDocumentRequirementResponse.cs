using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace PSW.ITMS.Service.DTO
{
    public class GetDocumentRequirementResponse
    {
        [JsonPropertyName("isLPCORequired")]
        public bool isLPCORequired { get; set; }

        [JsonPropertyName("formNumber")]
        public string FormNumber { get; set; }

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

        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("plainAmount")]
        public string PlainAmount { get; set; }

        [JsonPropertyName("ammendmentFee")]
        public string AmmendmentFee { get; set; }

        [JsonPropertyName("plainAmmendmentFee")]
        public string PlainAmmendmentFee { get; set; }
    }

    public class ValidityRequirement
    {
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("uomName")]
        public string UomName { get; set; }
    }
}