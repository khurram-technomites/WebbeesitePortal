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
    public class DashboardClient : IDashboardClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<DashboardClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public DashboardClient(HttpClient client, ILogger<DashboardClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<AdminDashboardStatsDTO> GetAdminDashboardCount()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Stats/StatsCount");

            return await _clientHelper.ParseResponseAsync<AdminDashboardStatsDTO>(HttpResponse);
        }
        
        public async Task<VendorDashboardStatsDTO> GetVendorDashboardCount(long VendorId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.GetAsync($"Stats/VendorStatsCount/{VendorId}");
            return await _clientHelper.ParseResponseAsync<VendorDashboardStatsDTO>(response);
        }
    }
}
