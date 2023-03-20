using HelperClasses.DTOs.Order;
using HelperClasses.DTOs.RestaurantDashboard;
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
    public class RestaurantDashboardClient : IRestaurantDashboardClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<RestaurantDashboardClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public RestaurantDashboardClient(HttpClient client, ILogger<RestaurantDashboardClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<RestaurantDashboardStatsDTO> GetRestaurantDashboardCount(long RestaurantId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Stats/Dashboard/RestaurantStatsCount/{RestaurantId}");

            return await _clientHelper.ParseResponseAsync<RestaurantDashboardStatsDTO>(HttpResponse);
        }

        public async Task<DashboardStatsDTO> GetPaymentMethodStats(long RestaurantId, long branchId = 0)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Restaurant/{RestaurantId}/DashboardStats/{branchId}");

            return await _clientHelper.ParseResponseJsonAsync<DashboardStatsDTO>(HttpResponse);
        }
    }
}
