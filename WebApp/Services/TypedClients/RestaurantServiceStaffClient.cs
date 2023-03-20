using HelperClasses.DTOs;
using HelperClasses.DTOs.RestaurantServiceStaff;
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
    public class RestaurantServiceStaffClient :  IRestaurantServiceStaffClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<IRestaurantServiceStaffClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public RestaurantServiceStaffClient(HttpClient client, ILogger<RestaurantServiceStaffClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<RestaurantServiceStaffDTO> AddRestaurantServiceStaffAsync(RestaurantServiceStaffDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("RestaurantServiceStaff/Register", content);
            return await _clientHelper.ParseResponseAsync<RestaurantServiceStaffDTO>(HttpResponse);
        }

        public async Task DeleteRestaurantServiceStaffAsync(long ServiceStaffId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"RestaurantServiceStaff/{ServiceStaffId}");
        }

        public async Task<IEnumerable<RestaurantServiceStaffDTO>> GetAllRestaurantServiceStaffsAsync(PagingParameters paging)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent httpContent = _clientHelper.CreateHttpContent(paging);
            var HttpResponse = await _client.PostAsync("RestaurantServiceStaff/GetAll", httpContent);

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantServiceStaffDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<RestaurantServiceStaffDTO>> GetAllRestaurantServiceStaffsAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.PostAsync("RestaurantServiceStaff/GetAll", null);

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantServiceStaffDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<RestaurantServiceStaffDTO>> GetRestaurantServiceStaffByRestaurantIdAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"RestaurantServiceStaff/Restaurant/{Id}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantServiceStaffDTO>>(HttpResponse);
        }
        public async Task<RestaurantServiceStaffDTO> GetRestaurantServiceStaffByIdAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"RestaurantServiceStaff/RestaurantServiceStaff/{Id}");

            return await _clientHelper.ParseResponseAsync<RestaurantServiceStaffDTO>(HttpResponse);
        }

        public async Task<RestaurantServiceStaffDTO> UpdateRestaurantServiceStaffAsync(RestaurantServiceStaffDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("RestaurantServiceStaff/UpdateServiceStaff", content);

            return await _clientHelper.ParseResponseAsync<RestaurantServiceStaffDTO>(HttpResponse);
        }
        public async Task<RestaurantServiceStaffDTO> ToggleActiveStatus(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            //HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.GetAsync($"RestaurantServiceStaff/ToggleStatus/{Id}");

            return await _clientHelper.ParseResponseAsync<RestaurantServiceStaffDTO>(HttpResponse);
        }

    }
}
