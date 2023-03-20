using HelperClasses.DTOs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class RestaurantBannerSettingClient : IRestaurantBannerSettingClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<RestaurantBannerSettingClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public RestaurantBannerSettingClient(HttpClient client, ILogger<RestaurantBannerSettingClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<RestaurantBannerSettingDTO> AddRestaurantBannerSettingAsync(RestaurantBannerSettingDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("RestaurantBannerSetting", content);

            return await _clientHelper.ParseResponseAsync<RestaurantBannerSettingDTO>(HttpResponse);
        }

        public async Task DeleteRestaurantBannerSettingAsync(long RestaurantBannerSettingId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"RestaurantBannerSetting/{RestaurantBannerSettingId}");
        }

        public async Task<IEnumerable<RestaurantBannerSettingDTO>> GetAllRestaurantBannerSettingsAsync(long RestaurantId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("RestaurantBannerSetting/GetAll/Restaurants/" + RestaurantId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantBannerSettingDTO>>(HttpResponse);
        }



        public async Task<RestaurantBannerSettingDTO> GetRestaurantBannerSettingByIdAsync(long RestaurantBannerSettingId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("RestaurantBannerSetting/" + RestaurantBannerSettingId);

            return await _clientHelper.ParseResponseAsync<RestaurantBannerSettingDTO>(HttpResponse);
        }

        public async Task<IEnumerable<RestaurantBannerSettingDTO>> GetBannerByType(long RestaurantId , string Type)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("RestaurantBannerSetting/Restaurants/" + RestaurantId + "/Type/" + Type);

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantBannerSettingDTO>>(HttpResponse);
        }

        public async Task<RestaurantBannerSettingDTO> UpdateRestaurantBannerSettingAsync(RestaurantBannerSettingDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("RestaurantBannerSetting", content);

            return await _clientHelper.ParseResponseAsync<RestaurantBannerSettingDTO>(HttpResponse);
        }
        public async Task<RestaurantBannerSettingDTO> UpdateRestaurantBannerSettingMenuImage(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.PutAsync($"RestaurantBannerSetting/MenuImage/{Id}", null);

            return await _clientHelper.ParseResponseAsync<RestaurantBannerSettingDTO>(HttpResponse);
        }
        public async Task<RestaurantBannerSettingDTO> ToggleActiveStatus(long RestaurantBannerSettingId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"RestaurantBannerSetting/ToggleStatus/{RestaurantBannerSettingId}");

            return await _clientHelper.ParseResponseAsync<RestaurantBannerSettingDTO>(HttpResponse);
        }
    }
}
