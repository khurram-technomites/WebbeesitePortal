using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApp.Interfaces.Helpers
{
    public interface IHttpClientHelper
    {
        Uri CreateRequestUri(string relativePath, string queryString = "");
        HttpContent CreateHttpContent<T>(T content);
        Task<T> ParseResponseAsync<T>(HttpResponseMessage response);
        Task<T> ParseResponseJsonAsync<T>(HttpResponseMessage response);
        Task<string> ParseResponseStringAsync(HttpResponseMessage response);
    }
}
