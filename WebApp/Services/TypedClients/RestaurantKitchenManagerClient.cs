using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces.TypedClients;
using HelperClasses.DTOs.RestaurantKitchenManager;
using HelperClasses.DTOs.RestaurantCashierStaff;

namespace WebApp.Services.TypedClients
{
    public class RestaurantKitchenManagerClient: IRestaurantKitchenManagerClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<RestaurantKitchenManagerClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public RestaurantKitchenManagerClient(HttpClient client, ILogger<RestaurantKitchenManagerClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;   
        }
        public async Task<IEnumerable<RestaurantKitchenManagerDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Restaurant/KitchenManager");

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantKitchenManagerDTO>>(HttpResponse);
        }

        public async Task<RestaurantKitchenManagerDTO> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Restaurant/KitchenManager/{Id}");

            return await _clientHelper.ParseResponseAsync<RestaurantKitchenManagerDTO>(HttpResponse);
        }

        public async Task<IEnumerable<RestaurantKitchenManagerDTO>> GetAllByRestaurantIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Restaurant/{Id}/KitchenManager/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantKitchenManagerDTO>>(HttpResponse);
        }

        public async Task <RestaurantKitchenManagerDTO> GetByRestaurantBranchIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Restaurant/KitchenManager/ByBranch/" + Id);

            return await _clientHelper.ParseResponseAsync<RestaurantKitchenManagerDTO>(HttpResponse);
        }

        public async Task<RestaurantKitchenManagerDTO> AddRestaurantKitchenManagerAsync(RestaurantKitchenManagerDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Restaurant/KitchenManager", content);

            return await _clientHelper.ParseResponseAsync<RestaurantKitchenManagerDTO>(HttpResponse);
        }

        public async Task<RestaurantKitchenManagerDTO> UpdateRestaurantKitchenManagerAsync(RestaurantKitchenManagerDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Restaurant/KitchenManager", content);

            return await _clientHelper.ParseResponseAsync<RestaurantKitchenManagerDTO>(HttpResponse);
        }

        public async Task<RestaurantKitchenManagerDTO> ToggleActiveStatus(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Restaurant/KitchenManager/{Id}/ToggleStatus");

            return await _clientHelper.ParseResponseAsync<RestaurantKitchenManagerDTO>(HttpResponse);
        }

        public async Task DeleteRestaurantKitchenManagerAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Restaurant/KitchenManager/{Id}");
        }

       
    }
}
