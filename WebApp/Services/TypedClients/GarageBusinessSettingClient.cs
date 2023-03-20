using HelperClasses.DTOs.GarageCMS;
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
    public class GarageBusinessSettingClient : IGarageBusinessSettingClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageBusinessSettingClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public GarageBusinessSettingClient(HttpClient client, ILogger<GarageBusinessSettingClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<GarageBusinessSettingDTO> Create(GarageBusinessSettingDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("Garage/BusinessSetting", content);

            return await _clientHelper.ParseResponseAsync<GarageBusinessSettingDTO>(HttpResponse);
        }

        public async Task<GarageBusinessSettingDTO> Update(GarageBusinessSettingDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("Garage/BusinessSetting", content);

            return await _clientHelper.ParseResponseAsync<GarageBusinessSettingDTO>(HttpResponse);
        }

        public async Task<IEnumerable<GarageBusinessSettingDTO>> GetBusinessSettings(long garageId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/BusinessSetting/GetAll/{garageId}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageBusinessSettingDTO>>(HttpResponse);
        }
        public async Task<GarageBusinessSettingDTO> GetBusinessSetting(long garageId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/BusinessSetting/Get/{garageId}");

            return await _clientHelper.ParseResponseAsync<GarageBusinessSettingDTO>(HttpResponse);
        }
    }
}

