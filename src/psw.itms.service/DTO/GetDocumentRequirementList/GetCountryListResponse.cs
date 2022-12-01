using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace PSW.ITMS.Service.DTO
{
    public class GetCountryListResponse
    {
        [JsonPropertyName("isPrintAllowed")]
        public bool isPrintAllowed { get; set; }

    }

}