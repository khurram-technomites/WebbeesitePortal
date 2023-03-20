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
    public class SupplierDashboardClient : ISupplierDashboardClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SupplierDashboardClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SupplierDashboardClient(HttpClient client, ILogger<SupplierDashboardClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<SupplierDashboardDTO> GetSupplierDashboardCount(long SupplierId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Stats/Dashboard/SupplierStatusCount/{SupplierId}");

            return await _clientHelper.ParseResponseAsync<SupplierDashboardDTO>(HttpResponse);
        }
    }
}
