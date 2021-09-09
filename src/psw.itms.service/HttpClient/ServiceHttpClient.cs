using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PSW.ITMS.Service
{
    public class ServiceHttpClient
    {
        public string URL;

        public string JsonData;

        public ServiceHttpClient(string _URL, string _JsonData)
        {
            URL = _URL;
            JsonData = _JsonData;
        }

        public async Task<string> GetHttpResponse()
        {
            var client = new HttpClient();
            var queryString = new StringContent(JsonData);

            var response = await client.PostAsync(new Uri(URL), queryString);

            response.EnsureSuccessStatusCode();
            //return responseBody;
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                //close HttpClient
                client.Dispose();
                //Return the response got
                return responseBody;
            }
            else
            {
                client.Dispose();
                return "Got Error: " + (int)response.StatusCode + "|" + response.ReasonPhrase;
            }
        }

    }
}