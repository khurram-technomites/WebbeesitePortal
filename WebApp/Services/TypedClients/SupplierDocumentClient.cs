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
    public class SupplierDocumentClient : ISupplierDocumentClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SupplierDocumentClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;

        public SupplierDocumentClient(HttpClient client, ILogger<SupplierDocumentClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<IEnumerable<SupplierDocumentDTO>> GetDocumentBySupplierId(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Supplier/{Id}/Document");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierDocumentDTO>>(HttpResponse);
        }
        public async Task<SupplierDocumentDTO> AddSupplierDocument(SupplierDocumentDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PostAsync("Supplier/Document", content);

            return await _clientHelper.ParseResponseAsync<SupplierDocumentDTO>(HttpResponse);
        }

        public async Task DeleteSupplierDocument(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Supplier/Document/{Id}");
        }
    }
}
