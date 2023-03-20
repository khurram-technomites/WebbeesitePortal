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
using WebApp.ViewModels;

namespace WebApp.Services.TypedClients
{
    public class SupplierItemClient : ISupplierItemClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SupplierItemClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public SupplierItemClient(HttpClient client, ILogger<SupplierItemClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<SupplierItemViewModel> AddAsync(SupplierItemDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PostAsync("Supplier/Item", content);

            return await _clientHelper.ParseResponseAsync<SupplierItemViewModel>(HttpResponse);
        }

        public async Task<SupplierItemViewModel> ArchiveAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.DeleteAsync($"Supplier/Item/{Id}");

            return await _clientHelper.ParseResponseAsync<SupplierItemViewModel>(HttpResponse);
        }

        public async Task<SupplierItemViewModel> GetByIdAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Supplier/Item/{Id}");

            return await _clientHelper.ParseResponseAsync<SupplierItemViewModel>(HttpResponse);
        }

        public async Task<IEnumerable<SupplierItemViewModel>> GetBySupplierAsync(long SupplierId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Supplier/{SupplierId}/Item");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierItemViewModel>>(HttpResponse);
        }

        public async Task<SupplierItemViewModel> ToggleActiveStatusAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.PutAsync($"Supplier/Item/{Id}/ToggleStatus", null);

            return await _clientHelper.ParseResponseAsync<SupplierItemViewModel>(HttpResponse);
        }

        public async Task<SupplierItemViewModel> UpdateAsync(SupplierItemDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PutAsync("Supplier/Item", content);

            return await _clientHelper.ParseResponseAsync<SupplierItemViewModel>(HttpResponse);
        }
    }
}
