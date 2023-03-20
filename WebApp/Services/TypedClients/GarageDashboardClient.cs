using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using HelperClasses.DTOs.Supplier;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebAPI.Models;
using HelperClasses.DTOs.GarageDashboard;

namespace WebApp.Services.TypedClients
{
    public class GarageDashboardClient: IGarageDashboardClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageDashboardClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public GarageDashboardClient(HttpClient client, ILogger<GarageDashboardClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<GarageDashboardStatsDTO> GetGarageDashboardCount(long GarageId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Stats/Dashboard/Garage/{GarageId}");

            return await _clientHelper.ParseResponseAsync<GarageDashboardStatsDTO>(HttpResponse);
        }
    }
}
