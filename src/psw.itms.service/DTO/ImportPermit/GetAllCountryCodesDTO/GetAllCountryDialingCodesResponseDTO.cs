using System.Text.Json.Serialization;

namespace PSW.ITMS.Service.DTO
{
    public class GetAllCountryDialingCodesResponseDTO
    {
        [JsonPropertyName("code")]
        public string CountryCode { get; set; }

        [JsonPropertyName("dialCode")]
        public string CountryDialingCode { get; set; }
    }

}