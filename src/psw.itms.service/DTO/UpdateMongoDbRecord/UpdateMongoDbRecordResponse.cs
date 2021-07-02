using System.Text.Json.Serialization;
using System.Collections.Generic;


namespace PSW.ITMS.Service.DTO
{
    public class UpdateMongoDbRecordResponsetDTO
    {
        [JsonPropertyName("recordUpdated")]
        public bool RecordUpdated { get; set; }
    }
}