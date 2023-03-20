using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class SupplierPackageClient : ISupplierPackageClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SupplierPackageClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;

        public SupplierPackageClient(HttpClient client, ILogger<SupplierPackageClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<SupplierPackageDTO>> GetAllAsync()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            var HttpResponse = await _client.GetAsync("Supplier/Package");
            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierPackageDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SupplierPackageDTO>> GetByIdAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            var HttpResponse = await _client.GetAsync($"Supplier/Package/GetBy/{Id}");
            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierPackageDTO>>(HttpResponse);
        }

        public async Task<SupplierPackageDTO> AddSupplierPackageAsync(SupplierPackageDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PostAsync("Supplier/Package", content);
            return await _clientHelper.ParseResponseAsync<SupplierPackageDTO>(HttpResponse);
        }

        public async Task<SupplierPackageDTO> UpdateSupplierPackageAsync(SupplierPackageDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PutAsync("Supplier/Package", content);
            return await _clientHelper.ParseResponseAsync<SupplierPackageDTO>(HttpResponse);
        }

        public async Task DeleteSupplierPackageAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            await _client.DeleteAsync($"Supplier/Package/{Id}");

        }
        public async Task<SupplierPackageDTO> ToggleActiveStatus(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            //HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.GetAsync($"Supplier/Package/ToggleStatus/{Id}");

            return await _clientHelper.ParseResponseAsync<SupplierPackageDTO>(HttpResponse);
        }
    }
}
