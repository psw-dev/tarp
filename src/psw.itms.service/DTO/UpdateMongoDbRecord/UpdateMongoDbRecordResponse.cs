using System.Text.Json.Serialization;


namespace PSW.ITMS.Service.DTO
{
    public class UpdateMongoDbRecordResponsetDTO
    {
        [JsonPropertyName("recordUpdated")]
        public bool RecordUpdated { get; set; }
    }
}