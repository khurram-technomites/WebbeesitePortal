using HelperClasses.DTOs.Restaurant;
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
    public class RestaurantWaiterClient : IRestaurantWaiterClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<RestaurantWaiterClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public RestaurantWaiterClient(HttpClient client, ILogger<RestaurantWaiterClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<IEnumerable<RestaurantWaiterDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Restaurant/Waiter");

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantWaiterDTO>>(HttpResponse);
        }

        public async Task<RestaurantWaiterDTO> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Restaurant/Waiter/" + Id);

            return await _clientHelper.ParseResponseAsync<RestaurantWaiterDTO>(HttpResponse);
        }

        public async Task<IEnumerable<RestaurantWaiterDTO>> GetAllByRestaurantIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Restaurant/{Id}/Waiter/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantWaiterDTO>>(HttpResponse);
        }

        public async Task<RestaurantWaiterDTO> GetByRestaurantBranchIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Restaurant/Waiter/ByBranch/" + Id);

            return await _clientHelper.ParseResponseAsync<RestaurantWaiterDTO>(HttpResponse);
        }

        public async Task<RestaurantWaiterDTO> AddRestaurantWaiterAsync(RestaurantWaiterDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Restaurant/Waiter", content);

            return await _clientHelper.ParseResponseAsync<RestaurantWaiterDTO>(HttpResponse);
        }

        public async Task<RestaurantWaiterDTO> UpdateRestaurantWaiterAsync(RestaurantWaiterDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Restaurant/Waiter", content);

            return await _clientHelper.ParseResponseAsync<RestaurantWaiterDTO>(HttpResponse);
        }
        public async Task<RestaurantWaiterDTO> ToggleActiveStatus(long RestaurantWaiterStaffId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Restaurant/Waiter/ToggleStatus/{RestaurantWaiterStaffId}");

            return await _clientHelper.ParseResponseAsync<RestaurantWaiterDTO>(HttpResponse);
        }
        public async Task DeleteRestaurantWaiterAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Restaurant/Waiter/{Id}");
        }
    }
}
