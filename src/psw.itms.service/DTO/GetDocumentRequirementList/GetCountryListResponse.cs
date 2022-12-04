using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace PSW.ITMS.Service.DTO
{
    public class GetCountryListResponse
    {
        [JsonPropertyName("countryList")]
        public List<string> CountryList { get; set; }

    }

}