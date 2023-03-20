using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HelperClasses.DTOs.GarageCMS;

namespace WebApp.Services.TypedClients
{
    public class GarageBannerSettingClient : IGarageBannerSettingClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageBannerSettingClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public GarageBannerSettingClient(HttpClient client, ILogger<GarageBannerSettingClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<GarageBannerSettingDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/BannerSetting");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageBannerSettingDTO>>(HttpResponse);
        }

        public async Task<GarageBannerSettingDTO> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/BannerSetting/{Id}");

            return await _clientHelper.ParseResponseAsync<GarageBannerSettingDTO>(HttpResponse);
        }

        public async Task<IEnumerable<GarageBannerSettingDTO>> GetAllByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{Id}/BannerSetting/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageBannerSettingDTO>>(HttpResponse);
        }


        public async Task<GarageBannerSettingDTO> AddGarageBannerSettingAsync(GarageBannerSettingDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Garage/BannerSetting", content);

            return await _clientHelper.ParseResponseAsync<GarageBannerSettingDTO>(HttpResponse);
        }

        public async Task<GarageBannerSettingDTO> UpdateGarageBannerSettingAsync(GarageBannerSettingDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Garage/BannerSetting", content);

            return await _clientHelper.ParseResponseAsync<GarageBannerSettingDTO>(HttpResponse);
        }

        public async Task DeleteGarageBannerSettingAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/BannerSetting/{Id}");
        }

    }
}
