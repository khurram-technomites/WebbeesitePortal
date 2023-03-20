using HelperClasses.DTOs;
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
    public class RestaurantContentManagementClient : IRestaurantContentManagementClient
    {

        private readonly HttpClient _client;
        private readonly ILogger<RestaurantContentManagementClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public RestaurantContentManagementClient(HttpClient client, ILogger<RestaurantContentManagementClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }

        public async Task<RestaurantContentManagementDTO> Create(RestaurantContentManagementDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("RestaurantContentManagement", content);

            return await _clientHelper.ParseResponseAsync<RestaurantContentManagementDTO>(HttpResponse);
        }

        public async Task<RestaurantContentManagementDTO> Update(RestaurantContentManagementDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("RestaurantContentManagement", content);

            return await _clientHelper.ParseResponseAsync<RestaurantContentManagementDTO>(HttpResponse);
        }

        public async Task<RestaurantContentManagementDTO> Footer(RestaurantContentManagementDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("RestaurantContentManagement/Footer", content);

            return await _clientHelper.ParseResponseAsync<RestaurantContentManagementDTO>(HttpResponse);
        }


        public async Task<IEnumerable<RestaurantContentManagementDTO>> GetRestaurantContentManagement()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            /*HttpContent content = _clientHelper.CreateHttpContent();*/
            var HttpResponse = await _client.GetAsync("RestaurantContentManagement");

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantContentManagementDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<RestaurantContentManagementDTO>> GetRestaurantContentManagementByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"RestaurantContentManagement/{Id}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantContentManagementDTO>>(HttpResponse);
        }
        public async Task<RestaurantContentManagementDTO> GetRestaurantContentManagementByRestaurantId(long RestaurantId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"RestaurantContentManagement/Restaurant/{RestaurantId}");

            return await _clientHelper.ParseResponseAsync<RestaurantContentManagementDTO>(HttpResponse);
        }
    }
}
