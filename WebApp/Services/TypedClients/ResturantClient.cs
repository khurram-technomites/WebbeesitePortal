using HelperClasses.DTOs;
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
    public class ResturantClient : IResturantClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ResturantClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public ResturantClient(HttpClient client, ILogger<ResturantClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<RestaurantDTO> Create(RestaurantDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("Restaurant", content);

            return await _clientHelper.ParseResponseAsync<RestaurantDTO>(HttpResponse);
        }

        public async Task<RestaurantDTO> Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"Returant/{Id}");

            return await _clientHelper.ParseResponseAsync<RestaurantDTO>(response);
        }


        public async Task<RestaurantDTO> Edit(RestaurantDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("Restaurant", content);

            return await _clientHelper.ParseResponseAsync<RestaurantDTO>(HttpResponse);
        }
        public async Task<RestaurantDTO> EditAboutUsImage(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            //HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync($"Restaurant/EditAboutUsImage/{Id}", null);

            return await _clientHelper.ParseResponseAsync<RestaurantDTO>(HttpResponse);
        }
        public async Task<RestaurantDTO> UnAssignSupplierCode(RestaurantDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("Restaurant/UnAssignSupplierCode", content);

            return await _clientHelper.ParseResponseAsync<RestaurantDTO>(HttpResponse);
        }
        public async Task<IEnumerable<RestaurantDTO>> GetResturant(PagingParameters Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PostAsync("Restaurant/GetAll", content);

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<RestaurantDTO>> GetRestaurantForDropDwonAsync(PagingParameters Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PostAsync("Restaurant/GetUnAssignedSupplierCodeRestaurant", content);

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<RestaurantDTO>> GetRestaurantForDropDwonAssignAsync(PagingParameters Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PostAsync("Restaurant/GetAssignedSupplierCodeRestaurant", content);

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantDTO>>(HttpResponse);
        }

        public async Task<RestaurantDTO> GetResturantByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Restaurant/{Id}");

            return await _clientHelper.ParseResponseAsync<RestaurantDTO>(HttpResponse);
        }

        public async Task<IEnumerable<RestaurantImagesDTO>> GetRestaurantImagesAsync(long RestaurantId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"RestaurantImage/ByRestaurant/{RestaurantId}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantImagesDTO>>(HttpResponse);
        }
    }
}
