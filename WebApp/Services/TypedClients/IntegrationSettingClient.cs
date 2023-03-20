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
    public class IntegrationSettingClient : IIntegrationSettingClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<IntegrationSettingClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public IntegrationSettingClient(HttpClient client, ILogger<IntegrationSettingClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<IntegrationSettingDTO> Create(IntegrationSettingDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("IntegrationSetting", content);

            return await _clientHelper.ParseResponseAsync<IntegrationSettingDTO>(HttpResponse);
        }

        public async Task<IntegrationSettingDTO> Update(IntegrationSettingDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("IntegrationSetting", content);

            return await _clientHelper.ParseResponseAsync<IntegrationSettingDTO>(HttpResponse);
        }

        public async Task<IEnumerable<IntegrationSettingDTO>> GetIntegrationSettings()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            /*HttpContent content = _clientHelper.CreateHttpContent();*/
            var HttpResponse = await _client.GetAsync("IntegrationSetting");

            return await _clientHelper.ParseResponseAsync<IEnumerable<IntegrationSettingDTO>>(HttpResponse);
        }
    }
}

