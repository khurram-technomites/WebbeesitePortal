using HelperClasses.DTOs;
using HelperClasses.DTOs.RestaurantCashierStaff;
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
 
        public class RestaurantCashierStaffClient : IRestaurantCashierStaffClient
        {
            private readonly HttpClient _client;
            private readonly ILogger<RestaurantCashierStaffClient> _logger;
            private readonly IHttpClientHelper _clientHelper;

            private readonly ITokenManager _tokenManager;
            public RestaurantCashierStaffClient(HttpClient client, ILogger<RestaurantCashierStaffClient> logger, IHttpClientHelper clientHelper,
                ITokenManager tokenManager)
            {
                _client = client;
                _logger = logger;
                _clientHelper = clientHelper;
                _tokenManager = tokenManager;
            }

            public async Task<RestaurantCashierStaffDTO> AddRestaurantCashierStaffAsync(RestaurantCashierStaffDTO Entity)
            {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
                var HttpResponse = await _client.PostAsync("Restaurant/CashierStaff", content);

                return await _clientHelper.ParseResponseAsync<RestaurantCashierStaffDTO>(HttpResponse);
            }

            public async Task DeleteRestaurantCashierStaffAsync(long RestaurantCashierStaffId)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

                await _client.DeleteAsync($"Restaurant/CashierStaff/{RestaurantCashierStaffId}");
            }

            public async Task<IEnumerable<RestaurantCashierStaffDTO>> GetAllRestaurantCashierStaffsAsync(long Id)
            {

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

                var HttpResponse = await _client.GetAsync($"Restaurant/{Id}/CashierStaff/");

                return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantCashierStaffDTO>>(HttpResponse);
            }

            public async Task<IEnumerable<RestaurantCashierStaffDTO>> GetAllRestaurantCashierStaffsAsync()
            {

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

                var HttpResponse = await _client.PostAsync("Restaurant/CashierStaff", null);

                return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantCashierStaffDTO>>(HttpResponse);
            }

            public async Task<RestaurantCashierStaffDTO> GetRestaurantCashierStaffByIdAsync(long Id)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

                var HttpResponse = await _client.GetAsync("Restaurant/CashierStaff/" + Id);

                return await _clientHelper.ParseResponseAsync<RestaurantCashierStaffDTO>(HttpResponse);
            }

            //public async Task<int> GetTotalRecordsOfRestaurantCashierStaffs()
            //{
            //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            //    var HttpResponse = await _client.GetAsync("RestaurantCashierStaff/TotalRecords");

            //    return await _clientHelper.ParseResponseAsync<int>(HttpResponse);
            //}

            public async Task<RestaurantCashierStaffDTO> UpdateRestaurantCashierStaffAsync(RestaurantCashierStaffDTO Entity)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

                HttpContent content = _clientHelper.CreateHttpContent(Entity);
                var HttpResponse = await _client.PutAsync("Restaurant/CashierStaff", content);

                return await _clientHelper.ParseResponseAsync<RestaurantCashierStaffDTO>(HttpResponse);
            }

            public async Task<RestaurantCashierStaffDTO> ToggleActiveStatus(long RestaurantCashierStaffId)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

                var HttpResponse = await _client.GetAsync($"Restaurant/CashierStaff/{RestaurantCashierStaffId}/ToggleStatus");

                return await _clientHelper.ParseResponseAsync<RestaurantCashierStaffDTO>(HttpResponse);
            }
        }
    
}
