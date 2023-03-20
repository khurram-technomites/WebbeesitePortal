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
using WebApp.ViewModels;

namespace WebApp.Services.TypedClients
{
    public class SupplierCategoryClient : ISupplierCategoryClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SupplierCategoryClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public SupplierCategoryClient(HttpClient client, ILogger<SupplierCategoryClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<IEnumerable<SupplierItemCategoryViewModel>> GetAllAsync()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Supplier/Category");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierItemCategoryViewModel>>(HttpResponse);
        }
    }
}
