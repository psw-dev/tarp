using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;

using PSW.ITMS.Service.DTO;
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
            HttpClient client = new HttpClient();
            StringContent queryString = new StringContent(JsonData);

            HttpResponseMessage response = await client.PostAsync(new Uri(URL), queryString);

            response.EnsureSuccessStatusCode();
            //return responseBody;
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

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