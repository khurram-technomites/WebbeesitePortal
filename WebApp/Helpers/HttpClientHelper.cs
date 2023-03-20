using HelperClasses.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApp.Interfaces.Helpers;
using WebApp.Services.TypedClients;

namespace WebApp.Helpers
{
    public class HttpClientHelper : IHttpClientHelper
    {
        public Uri CreateRequestUri(string relativePath, string queryString = "")
        {
            var uriBuilder = new UriBuilder(relativePath);
            uriBuilder.Query = queryString;
            return uriBuilder.Uri;
        }

        public HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, IsoDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public async Task<T> ParseResponseAsync<T>(HttpResponseMessage response)
        {
            var data = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(data);
        }

        public async Task<T> ParseResponseJsonAsync<T>(HttpResponseMessage response)
        {
            var data = await ParseResponseAsync<ResponseMessageDTO>(response);

            return JsonConvert.DeserializeObject<T>(data.Result.ToString());
        }

        public async Task<string> ParseResponseStringAsync(HttpResponseMessage response)
        {
            var data = await ParseResponseAsync<ResponseMessageDTO>(response);

            return data.Result.ToString();
        }

        private JsonSerializerSettings IsoDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat
                };
            }
        }

        private JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }
    }
}
