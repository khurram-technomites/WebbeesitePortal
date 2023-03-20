using HelperClasses.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class SubscriberClient : ISubscriberClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SubscriberClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public SubscriberClient(HttpClient client, ILogger<SubscriberClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<SubscriberDTO> Create(SubscriberDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("Subscriber", content);

            return await _clientHelper.ParseResponseAsync<SubscriberDTO>(HttpResponse);
        }

        public async Task<SubscriberDTO> Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"Subscriber/{Id}");

            return await _clientHelper.ParseResponseAsync<SubscriberDTO>(response);
        }

        public async Task<SubscriberDTO> Edit(SubscriberDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("Subscriber", content);

            return await _clientHelper.ParseResponseAsync<SubscriberDTO>(HttpResponse);
        }

        public async Task<IEnumerable<SubscriberDTO>> GetSubscribers(PagingParameters Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PostAsync("Subscriber/GetAll", content);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SubscriberDTO>>(HttpResponse);
        }

        public async Task<SubscriberDTO> GetEmailByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Subscriber/{Id}");

            return await _clientHelper.ParseResponseAsync<SubscriberDTO>(HttpResponse);
        }
        public async Task<IEnumerable<SubscriberDTO>> GetsubscribersDateWise(DateTime FromDate, DateTime ToDate)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Subscriber/DateWise/{FromDate}/{ToDate}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SubscriberDTO>>(HttpResponse);
        }
        public async Task<SubscriberDTO> SendMessage(string Email , string Subject , String Message)
        {
            SubscriberDTO Model = new SubscriberDTO();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PostAsync($"Subscriber/sendEmail/{Email}/{Subject}/{Message}",content);

            return await _clientHelper.ParseResponseAsync<SubscriberDTO>(HttpResponse);
        }
    }
}
