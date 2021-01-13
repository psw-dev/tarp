using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetCityByCountryRequestDTO
    {
        [JsonPropertyName("code")]
        public string CountryCode { get; set; }

    }
}