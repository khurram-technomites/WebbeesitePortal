using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApp.Services.TypedClients
{
    public class RestaurantTaxSettingClient: IRestaurantTaxSettingClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<RestaurantTaxSettingClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;

        public RestaurantTaxSettingClient(HttpClient client, ILogger<RestaurantTaxSettingClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<IEnumerable<RestaurantTaxSettingDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Restaurant/TaxSettings");

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantTaxSettingDTO>>(HttpResponse);
        }

        public async Task <RestaurantTaxSettingDTO> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Restaurant/TaxSettings/" + Id);

            return await _clientHelper.ParseResponseAsync <RestaurantTaxSettingDTO>(HttpResponse);
        }

        public async Task<IEnumerable<RestaurantTaxSettingDTO>> GetAllByRestaurantIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Restaurant/{Id}/TaxSettings/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantTaxSettingDTO>>(HttpResponse);
        }

        public async Task<RestaurantTaxSettingDTO> GetByRestaurantBranchIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Restaurant/TaxSettings/ByBranch/" + Id);

            return await _clientHelper.ParseResponseAsync<RestaurantTaxSettingDTO>(HttpResponse);
        }

        public async Task<RestaurantTaxSettingDTO> AddRestaurantTaxSettingAsync(RestaurantTaxSettingDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Restaurant/TaxSettings", content);

            return await _clientHelper.ParseResponseAsync<RestaurantTaxSettingDTO>(HttpResponse);
        }

        public async Task<RestaurantTaxSettingDTO> UpdateRestaurantTaxSettingAsync(RestaurantTaxSettingDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Restaurant/TaxSettings", content);

            return await _clientHelper.ParseResponseAsync<RestaurantTaxSettingDTO>(HttpResponse);
        }

        public async Task DeleteRestaurantTaxSettingAsync(long TaxSettingId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Restaurant/TaxSettings/{TaxSettingId}");
        }
    }
}
