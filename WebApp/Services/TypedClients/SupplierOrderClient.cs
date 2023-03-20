using HelperClasses.DTOs.Supplier;
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
    public class SupplierOrderClient : ISupplierOrderClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SupplierOrderClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SupplierOrderClient(HttpClient client, ILogger<SupplierOrderClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<SupplierOrderDTO>> GetAllSupplierOrderByRestaurantAsync(long RestaurantId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Supplier/Order/Restaurants/" + RestaurantId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierOrderDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SupplierOrderDTO>> GetAllOrderByRestaurantBranchAsync(long BranchId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("RestaurantOrder/GetAll/Branch/" + BranchId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierOrderDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SupplierOrderDTO>> GetOrderBySupplierIdAsync(long supplierId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Supplier/Order/GetBySupplierId/{supplierId}");

            return await _clientHelper.ParseResponseAsync <IEnumerable<SupplierOrderDTO>> (HttpResponse);
        }
        public async Task<IEnumerable<SupplierOrderDTO>> GetOrderByIdAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Supplier/Order/GetById/{Id}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierOrderDTO>>(HttpResponse);
        }
        public async Task<SupplierOrderDTO> UpdateStatusOrderAsync(SupplierOrderDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Supplier/Order/ToggleStatus", content);

            return await _clientHelper.ParseResponseAsync<SupplierOrderDTO>(HttpResponse);
        }

        public async Task<IEnumerable<SupplierOrderDTO>> GetAllOrdersByStatus(SupplierOrderDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Supplier/Order/GetByStatus", content);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierOrderDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<SupplierOrderDTO>> GetAllOrdersByStatusandDate(SupplierOrderDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Supplier/Order/GetByStatusandDate", content);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierOrderDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<SupplierOrderDTO>> GetAllRestaurantOrdersByStatus(SupplierOrderDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync($"Supplier/Order/GetByStatus/Restaurants", content);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierOrderDTO>>(HttpResponse);
        }

        public async Task<string> PlaceOrder(SupplierOrderPlacementDTO order)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(order);
             var HttpResponse = await _client.PostAsync("Supplier/Order/PlaceOrder" , content);

            return await HttpResponse.Content.ReadAsStringAsync();
        }

        public async Task<int> Paid(string PaymentId , long OrderId)
        {

            //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Supplier/Order/Paid/{OrderId}/{PaymentId}");

            return await _clientHelper.ParseResponseAsync<int>(HttpResponse);
        }
    }
}
