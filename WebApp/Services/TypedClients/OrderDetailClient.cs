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
    public class OrderDetailClient : IOrderDetailClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<OrderDetailClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public OrderDetailClient(HttpClient client, ILogger<OrderDetailClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<OrderDetailDTO>> GetAllOrderDetailsAsync(long OrderId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("OrderDetail/GetAll/Orders/" + OrderId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<OrderDetailDTO>>(HttpResponse);

        }
    }
}
