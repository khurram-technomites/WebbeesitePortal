using HelperClasses.DTOs.SparePartCMS;
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
    public class SparePartBusinessSettingClient : ISparePartBusinessSettingClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartBusinessSettingClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public SparePartBusinessSettingClient(HttpClient client, ILogger<SparePartBusinessSettingClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<SparePartBusinessSettingDTO> Create(SparePartBusinessSettingDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("SparePart/BusinessSetting", content);

            return await _clientHelper.ParseResponseAsync<SparePartBusinessSettingDTO>(HttpResponse);
        }

        public async Task<SparePartBusinessSettingDTO> Update(SparePartBusinessSettingDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("SparePart/BusinessSetting", content);

            return await _clientHelper.ParseResponseAsync<SparePartBusinessSettingDTO>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartBusinessSettingDTO>> GetBusinessSettings(long SparePartId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SparePart/BusinessSetting/GetAll/{SparePartId}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartBusinessSettingDTO>>(HttpResponse);
        }
    }
}

