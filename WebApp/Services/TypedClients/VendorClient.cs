using HelperClasses.DTOs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Services.TypedClients
{
    public class VendorClient : IVendorClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<VendorClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public VendorClient(HttpClient client, ILogger<VendorClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<VendorDTO> Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"Vendors/{Id}");

            return await _clientHelper.ParseResponseAsync<VendorDTO>(response);
        }

        public async Task<object> Edit(VendorViewModel model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("Vendors", content);

            return await _clientHelper.ParseResponseAsync<object>(HttpResponse);
        }
        public async Task<object> Create(VendorViewModel model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("Vendors", content);

            return await _clientHelper.ParseResponseAsync<object>(HttpResponse);
        }

        public async Task<VendorDTO> GetVendorByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Vendors/{Id}");

            return await _clientHelper.ParseResponseAsync<VendorDTO>(HttpResponse);
        }

        public async Task<IEnumerable<VendorDTO>> GetVendors()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Vendors");

            return await _clientHelper.ParseResponseAsync<IEnumerable<VendorDTO>>(HttpResponse);
        }

        public async Task<VendorDTO> ToggleActiveStatus(long Id, bool flag, string RejectionReason = "")
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Vendors/{Id}/ToggleActiveStatus/{flag}?RejectionReason={RejectionReason}");

            return await _clientHelper.ParseResponseAsync<VendorDTO>(HttpResponse);
        }
    }
}
