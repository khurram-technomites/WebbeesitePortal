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
    public class RestaurantDocumentClient : IRestaurantDocumentClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<RestaurantDocumentClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public RestaurantDocumentClient(HttpClient client, ILogger<RestaurantDocumentClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<RestaurantDocumentDTO> AddDocument(RestaurantDocumentDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("Restaurant/Document", content);

            return await _clientHelper.ParseResponseAsync<RestaurantDocumentDTO>(HttpResponse);
        }

        public async Task DeleteDocument(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"Restaurant/Document/{Id}");
        }

        public async Task<RestaurantDocumentDTO> GetDocumentByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Restaurant/Document/{Id}");

            return await _clientHelper.ParseResponseAsync<RestaurantDocumentDTO>(HttpResponse);
        }

        public async Task<IEnumerable<RestaurantDocumentDTO>> GetDocumentByRestaurant(long RestaurantId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Restaurant/{RestaurantId}/Document");

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantDocumentDTO>>(HttpResponse);
        }

        public async Task<RestaurantDocumentDTO> UpdateDocument(RestaurantDocumentDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("Restaurant/Document", content);

            return await _clientHelper.ParseResponseAsync<RestaurantDocumentDTO>(HttpResponse);
        }
    }
}
