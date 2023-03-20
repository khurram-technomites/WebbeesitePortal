using HelperClasses.DTOs.Order;
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
    public class RestaurantOrderClient  : IRestaurantOrderClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<RestaurantOrderClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public RestaurantOrderClient(HttpClient client, ILogger<RestaurantOrderClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrderByRestaurantAsync(long RestaurantId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("RestaurantOrder/GetAll/Restaurants/" + RestaurantId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<OrderDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrderByRestaurantBranchAsync(long BranchId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("RestaurantOrder/GetAll/Branch/" + BranchId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<OrderDTO>>(HttpResponse);
        }

        public async Task<OrderDTO> GetOrderByIdAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("RestaurantOrder/" + Id);

            return await _clientHelper.ParseResponseJsonAsync<OrderDTO>(HttpResponse);
        }

        public async Task<OrderDTO> UpdateStatusOrderAsync(OrderDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("RestaurantOrder/ToggleStatus", content);

            return await _clientHelper.ParseResponseAsync<OrderDTO>(HttpResponse);
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrdersByStatus(OrderDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("RestaurantOrder/GetByStatus", content);

            return await _clientHelper.ParseResponseAsync<IEnumerable<OrderDTO>>(HttpResponse);
        }

    }
}
