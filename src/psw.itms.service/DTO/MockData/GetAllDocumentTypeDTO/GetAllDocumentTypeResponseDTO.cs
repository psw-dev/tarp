using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetAllDocumentTypeResponseDTO
    {
        [JsonPropertyName("HSCode")]
        public string HSCode { get; set; }

        [JsonPropertyName("purpose")]
        public string Purpose { get; set; }

        [JsonPropertyName("documents")]
        public IList<DocumentResponseDTO> Documents { get; set; }


    }
}
