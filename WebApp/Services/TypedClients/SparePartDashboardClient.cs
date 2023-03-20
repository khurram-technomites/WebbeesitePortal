using HelperClasses.DTOs.SparePartDashboard;
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
    public class SparePartDashboardClient : ISparePartDashboardClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartDashboardClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SparePartDashboardClient(HttpClient client, ILogger<SparePartDashboardClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<SparePartDashboardStatsDTO> GetSparePartDashboardCount(long SparePartId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Stats/Dashboard/SparePart/{SparePartId}");

            return await _clientHelper.ParseResponseAsync<SparePartDashboardStatsDTO>(HttpResponse);
        }
    }
}
