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
    public class SupplierItemCategoryClient : ISupplierItemCategoryClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SupplierItemCategoryClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SupplierItemCategoryClient(HttpClient client, ILogger<SupplierItemCategoryClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<SupplierItemCategoryDTO> Create(SupplierItemCategoryDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("Supplier/Category", content);

            return await _clientHelper.ParseResponseAsync<SupplierItemCategoryDTO>(HttpResponse);
        }

        public async Task<SupplierItemCategoryDTO> Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"Supplier/Category/{Id}");

            return await _clientHelper.ParseResponseAsync<SupplierItemCategoryDTO>(response);
        }

        public async Task<SupplierItemCategoryDTO> Edit(SupplierItemCategoryDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("Supplier/Category", content);

            return await _clientHelper.ParseResponseAsync<SupplierItemCategoryDTO>(HttpResponse);
        }

        public async Task<IEnumerable<SupplierItemCategoryDTO>> GetCategories()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Supplier/Category");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierItemCategoryDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SupplierItemCategoryDTO>> GetCategoryByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Supplier/Category/By/{Id}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierItemCategoryDTO>>(HttpResponse);
        }
        public async Task<SupplierItemCategoryDTO> ToggleActiveStatus(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            //HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.GetAsync($"Supplier/Category/ToggleStatus/{Id}");

            return await _clientHelper.ParseResponseAsync<SupplierItemCategoryDTO>(HttpResponse);
        }
    }
}
