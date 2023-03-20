using HelperClasses.DTOs;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.GarageCMS;
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
    public class GarageClient : IGarageClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;

        public GarageClient(HttpClient client, ILogger<GarageClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<GarageDTO> GetGarageByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{Id}");

            return await _clientHelper.ParseResponseAsync<GarageDTO>(HttpResponse);
        }
        public async Task<IEnumerable<GarageDTO>> GetGarages()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageDTO>>(HttpResponse);
        }
        public async Task<GarageDTO> ToggleActiveStatus(long Id, bool flag, string RejectionReason = "")
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{Id}/ToggleActiveStatus/{flag}?RejectionReason={RejectionReason}");

            return await _clientHelper.ParseResponseAsync<GarageDTO>(HttpResponse);
        }
        public async Task<GarageDTO> Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"Garage/{Id}");

            return await _clientHelper.ParseResponseAsync<GarageDTO>(response);
        }
        public async Task<object> Add(GarageRegisterDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("Garage", content);

            return await _clientHelper.ParseResponseAsync<object>(HttpResponse);
        }
        public async Task<object> Edit(GarageRegisterDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("ServiceStaff/Garage", content);

            return await _clientHelper.ParseResponseAsync<object>(HttpResponse);
        }

        public async Task<object> UpdateGarage(GarageDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("Garage", content);

            return await _clientHelper.ParseResponseAsync<object>(HttpResponse);
        }
        public async Task<object> UpdateVendorGarage(GarageDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("Garage/Vendor", content);

            return await _clientHelper.ParseResponseAsync<object>(HttpResponse);
        }

        public async Task<GarageDTO> GetGarageByUser()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            var HttpResponse = await _client.GetAsync("Garage/ByUser");
            return await _clientHelper.ParseResponseAsync<GarageDTO>(HttpResponse);
        }
        public async Task<IEnumerable<GarageDTO>> GetGarageByVendor(long VendorId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            var HttpResponse = await _client.GetAsync($"Garage/{VendorId}/ByVendor");
            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageDTO>>(HttpResponse);
        }

        public async Task<string> UpdateProfilePicture(long GarageId, string Path)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            ThemeColorAndLogoDTO model = new()
            {
                Logo = Path
            };

            HttpContent content = _clientHelper.CreateHttpContent(model);

            var HttpResponse = await _client.PutAsync($"Garage/{GarageId}/ProfilePicture", content);

            return await HttpResponse.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateTheme(long GarageId, string Theme)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            ThemeColorAndLogoDTO model = new()
            {
                ThemeColor = Theme
            };

            HttpContent content = _clientHelper.CreateHttpContent(model);

            var HttpResponse = await _client.PutAsync($"Garage/{GarageId}/Theme", content);

            return await HttpResponse.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateThumbnail(long GarageId, string Path)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            ThemeColorAndLogoDTO model = new()
            {
                Thumbnail = Path
            };

            HttpContent content = _clientHelper.CreateHttpContent(model);

            var HttpResponse = await _client.PutAsync($"Garage/{GarageId}/ThumbnailImage", content);

            return await HttpResponse.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateSecondaryLogo(long GarageId, string Path)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            ThemeColorAndLogoDTO model = new()
            {
                SecondaryLogo = Path
            };

            HttpContent content = _clientHelper.CreateHttpContent(model);

            var HttpResponse = await _client.PutAsync($"Garage/{GarageId}/SecondaryLogo", content);

            return await HttpResponse.Content.ReadAsStringAsync();
        }
        public async Task<object> Paid(long InvoiceId, string PaymentId)
        {
            _logger.LogError(string.Format("Paid API CHECK InvoiceId = {0}, PaymentId = {1}", InvoiceId, PaymentId));
            //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/Request/Paid/" + InvoiceId + "?PaymentId=" + PaymentId);

            return await _clientHelper.ParseResponseAsync<object>(HttpResponse);
        }
    }
}

