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
    public class GarageBranchBusinessSettingClient : IGarageBranchBusinessSettingClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageBranchBusinessSettingClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public GarageBranchBusinessSettingClient(HttpClient client, ILogger<GarageBranchBusinessSettingClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<GarageBranchBusinessSettingDTO> Create(GarageBranchBusinessSettingDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("Garage/BranchBusinessSetting", content);

            return await _clientHelper.ParseResponseAsync<GarageBranchBusinessSettingDTO>(HttpResponse);
        }

        public async Task<GarageBranchBusinessSettingDTO> Update(GarageBranchBusinessSettingDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("Garage/BranchBusinessSetting", content);

            return await _clientHelper.ParseResponseAsync<GarageBranchBusinessSettingDTO>(HttpResponse);
        }

        public async Task<IEnumerable<GarageBranchBusinessSettingDTO>> GetBusinessSettings(long businessId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/BranchBusinessSetting/GetAll/Business/{businessId}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageBranchBusinessSettingDTO>>(HttpResponse);
        }

        public async Task<GarageBranchBusinessSettingDTO> GetByIdAsync(long id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/BranchBusinessSetting/{id}");

            return await _clientHelper.ParseResponseAsync<GarageBranchBusinessSettingDTO>(HttpResponse);
        }

        public async Task ArchiveBranchBusinessSetting(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/BranchBusinessSetting/{Id}");
        }
    }
}
