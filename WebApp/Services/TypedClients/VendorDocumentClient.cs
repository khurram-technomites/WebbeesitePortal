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
using WebApp.ViewModels;
namespace WebApp.Services.TypedClients
{
    public class VendorDocumentClient:IVendorDocumentClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<VendorDocumentClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;

        public VendorDocumentClient(ITokenManager tokenManager, IHttpClientHelper clientHelper, ILogger<VendorDocumentClient> logger, HttpClient client)
        {
            _tokenManager = tokenManager;
            _clientHelper = clientHelper;
            _logger = logger;
            _client = client;
        }

        public async Task<VendorDocumentViewModel> AddVendorDocument(VendorDocumentDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PostAsync("Vendor/Document", content);

            return await _clientHelper.ParseResponseAsync<VendorDocumentViewModel>(HttpResponse);
        }

        public async Task Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Vendor/Document/{Id}");
        }

        public async Task<IEnumerable<VendorDocumentViewModel>> GetAllByVendor(long VendorId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Vendor/{VendorId}/Document");

            return await _clientHelper.ParseResponseAsync<IEnumerable<VendorDocumentViewModel>>(HttpResponse);
        }
    }
}
