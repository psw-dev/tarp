using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetCityByCountryResponseDTO
    {
        // ID, CountryCode, Name

        [JsonPropertyName("cityId")]
        public string ID { get; set; }

        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }

        [JsonPropertyName("cityName")]
        public string CityName { get; set; }   // Property Name Changed For Auto Mapper Developer Reference Purposes
    
    }

}