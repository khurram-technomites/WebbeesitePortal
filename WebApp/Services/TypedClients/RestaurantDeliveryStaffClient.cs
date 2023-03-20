using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
 
        public class RestaurantDeliveryStaffClient : IRestaurantDeliveryStaffClient
        {
            private readonly HttpClient _client;
            private readonly ILogger<RestaurantDeliveryStaffClient> _logger;
            private readonly IHttpClientHelper _clientHelper;

            private readonly ITokenManager _tokenManager;
            public RestaurantDeliveryStaffClient(HttpClient client, ILogger<RestaurantDeliveryStaffClient> logger, IHttpClientHelper clientHelper,
                ITokenManager tokenManager)
            {
                _client = client;
                _logger = logger;
                _clientHelper = clientHelper;
                _tokenManager = tokenManager;
            }

            public async Task<RestaurantDeliveryStaffDTO> AddRestaurantDeliveryStaffAsync(RestaurantDeliveryStaffDTO Entity)
            {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
                var HttpResponse = await _client.PostAsync("RestaurantDeliveryStaff/Create", content);

                return await _clientHelper.ParseResponseAsync<RestaurantDeliveryStaffDTO>(HttpResponse);
            }

            public async Task DeleteRestaurantDeliveryStaffAsync(long RestaurantDeliveryStaffId)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

                await _client.DeleteAsync($"RestaurantDeliveryStaff/{RestaurantDeliveryStaffId}");
            }

            public async Task<IEnumerable<RestaurantDeliveryStaffDTO>> GetAllRestaurantDeliveryStaffsAsync(long restaurantId)
            {

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

                var HttpResponse = await _client.GetAsync("RestaurantDeliveryStaff/GetAll/" + restaurantId);

                return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantDeliveryStaffDTO>>(HttpResponse);
            }

            public async Task<IEnumerable<RestaurantDeliveryStaffDTO>> GetAllRestaurantDeliveryStaffsAsync()
            {

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

                var HttpResponse = await _client.PostAsync("RestaurantDeliveryStaff/GetAll", null);

                return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantDeliveryStaffDTO>>(HttpResponse);
            }

            public async Task<RestaurantDeliveryStaffDTO> GetRestaurantDeliveryStaffByIdAsync(long RestaurantDeliveryStaffId)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

                var HttpResponse = await _client.GetAsync("RestaurantDeliveryStaff/" + RestaurantDeliveryStaffId);

                return await _clientHelper.ParseResponseAsync<RestaurantDeliveryStaffDTO>(HttpResponse);
            }

            //public async Task<int> GetTotalRecordsOfRestaurantDeliveryStaffs()
            //{
            //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            //    var HttpResponse = await _client.GetAsync("RestaurantDeliveryStaff/TotalRecords");

            //    return await _clientHelper.ParseResponseAsync<int>(HttpResponse);
            //}

            public async Task<RestaurantDeliveryStaffDTO> UpdateRestaurantDeliveryStaffAsync(RestaurantDeliveryStaffDTO Entity)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

                HttpContent content = _clientHelper.CreateHttpContent(Entity);
                var HttpResponse = await _client.PutAsync("RestaurantDeliveryStaff", content);

                return await _clientHelper.ParseResponseAsync<RestaurantDeliveryStaffDTO>(HttpResponse);
            }

            public async Task<RestaurantDeliveryStaffDTO> ToggleActiveStatus(long RestaurantDeliveryStaffId)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

                var HttpResponse = await _client.GetAsync($"RestaurantDeliveryStaff/ToggleStatus/{RestaurantDeliveryStaffId}");

                return await _clientHelper.ParseResponseAsync<RestaurantDeliveryStaffDTO>(HttpResponse);
            }
        }
    
}
