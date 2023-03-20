using HelperClasses.DTOs.Restaurant;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class RestaurantBranchClient : IRestaurantBranchClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<RestaurantBranchClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public RestaurantBranchClient(HttpClient client, ILogger<RestaurantBranchClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<RestaurantBranchDTO> AddRestaurantBranchAsync(RestaurantBranchDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("RestaurantBranch", content);

            return await _clientHelper.ParseResponseAsync<RestaurantBranchDTO>(HttpResponse);
        }

        public async Task DeleteRestaurantBranchAsync(long RestaurantBranchId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"RestaurantBranch/{RestaurantBranchId}");
        }

        public async Task<IEnumerable<RestaurantBranchDTO>> GetAllRestaurantBranchsAsync(long RestaurantId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("RestaurantBranch/GetAll/Restaurants/" + RestaurantId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantBranchDTO>>(HttpResponse);
        }



        public async Task<RestaurantBranchDTO> GetRestaurantBranchByIdAsync(long RestaurantBranchId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("RestaurantBranch/" + RestaurantBranchId);

            return await _clientHelper.ParseResponseAsync<RestaurantBranchDTO>(HttpResponse);
        }


        public async Task<RestaurantBranchDTO> UpdateRestaurantBranchAsync(RestaurantBranchDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("RestaurantBranch", content);

            return await _clientHelper.ParseResponseAsync<RestaurantBranchDTO>(HttpResponse);
        }

        public async Task<RestaurantBranchDTO> ToggleActiveStatus(long RestaurantBranchId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"RestaurantBranch/ToggleStatus/{RestaurantBranchId}");

            return await _clientHelper.ParseResponseAsync<RestaurantBranchDTO>(HttpResponse);
        }

        public async Task<RestaurantBranchDTO> ToggleCloseStatus(long RestaurantBranchId, TimeSpan? ClosingTimeSpan)
        {
            HttpResponseMessage HttpResponse;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            if (ClosingTimeSpan.HasValue)
            {
                HttpResponse = await _client.PutAsync($"RestaurantBranch/{RestaurantBranchId}/ToggleCloseStatus?ClosingTimeSpan={ClosingTimeSpan}", null);
            }
            else
            {
                HttpResponse = await _client.PutAsync($"RestaurantBranch/{RestaurantBranchId}/ToggleCloseStatus", null);
            }

            return await _clientHelper.ParseResponseAsync<RestaurantBranchDTO>(HttpResponse);
        }

        public async Task<RestaurantBranchDTO> ToggleMainStatus(long RestaurantBranchId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.PutAsync($"RestaurantBranch/{RestaurantBranchId}/ToggleMainStatus", null);

            return await _clientHelper.ParseResponseAsync<RestaurantBranchDTO>(HttpResponse);
        }
    }
}
