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
    public class RestaurantRatingClient : IRestaurantRatingClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<RestaurantRatingClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public RestaurantRatingClient(HttpClient client, ILogger<RestaurantRatingClient> logger, IHttpClientHelper clientHelper,ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<IEnumerable<RestaurantRatingDTO>> GetRestaurantRatings()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("RestaurantRating");

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantRatingDTO>>(HttpResponse);
        }
        public async Task<RestaurantRatingDTO> GetRestaurantRatingByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"RestaurantRating/{Id}");

            return await _clientHelper.ParseResponseAsync<RestaurantRatingDTO>(HttpResponse);
        }
        public async Task<IEnumerable<RestaurantRatingDTO>> GetRestaurantRatingByRestaurantID(long RestaurantId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"RestaurantRating/ByResataurantId/{RestaurantId}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantRatingDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<RestaurantRatingDTO>> GetRestaurantRatingByStatus(String status , long RestaurantId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"RestaurantRating/Status/{status}/{RestaurantId}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantRatingDTO>>(HttpResponse);
        }
        public async Task<RestaurantRatingDTO> ToggleActiveStatus(long RatingId , string status)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            //HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.GetAsync($"RestaurantRating/ToggleStatus/{RatingId}/{status}");

            return await _clientHelper.ParseResponseAsync<RestaurantRatingDTO>(HttpResponse);
        }
        public async Task<RestaurantRatingDTO> Edit(RestaurantRatingDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("RestaurantRating", content);

            return await _clientHelper.ParseResponseAsync<RestaurantRatingDTO>(HttpResponse);
        }
    }
}
