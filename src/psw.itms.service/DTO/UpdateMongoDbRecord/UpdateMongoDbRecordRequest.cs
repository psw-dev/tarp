using System.Text.Json.Serialization;
using System.Collections.Generic;


namespace PSW.ITMS.Service.DTO
{
    public class UpdateMongoDbRecordRequestDTO
    {
        [JsonPropertyName("collection")]
        public string Collection { get; set; }

        [JsonPropertyName("hsCode")]
        public string HsCode { get; set; }

        [JsonPropertyName("purpose")]
        public string Purpose { get; set; }

        [JsonPropertyName("updateKey")]
        public string UpdateKey { get; set; }

        [JsonPropertyName("updateValue")]
        public string UpdateValue { get; set; }

    }
}