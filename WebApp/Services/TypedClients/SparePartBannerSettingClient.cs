using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HelperClasses.DTOs.SparePartCMS;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class SparePartBannerSettingClient : ISparePartBannerSettingClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartBannerSettingClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SparePartBannerSettingClient(HttpClient client, ILogger<SparePartBannerSettingClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<SparePartBannerSettingDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/BannerSetting");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartBannerSettingDTO>>(HttpResponse);
        }

        public async Task<SparePartBannerSettingDTO> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/BannerSetting/{Id}");

            return await _clientHelper.ParseResponseAsync<SparePartBannerSettingDTO>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartBannerSettingDTO>> GetAllBySparePartDealerIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/{Id}/BannerSetting/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartBannerSettingDTO>>(HttpResponse);
        }


        public async Task<SparePartBannerSettingDTO> AddSparePartBannerSettingAsync(SparePartBannerSettingDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SpareParts/BannerSetting", content);

            return await _clientHelper.ParseResponseAsync<SparePartBannerSettingDTO>(HttpResponse);
        }

        public async Task<SparePartBannerSettingDTO> UpdateSparePartBannerSettingAsync(SparePartBannerSettingDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("SpareParts/BannerSetting", content);

            return await _clientHelper.ParseResponseAsync<SparePartBannerSettingDTO>(HttpResponse);
        }

        public async Task DeleteSparePartBannerSettingAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SpareParts/BannerSetting/{Id}");
        }
    }
}
