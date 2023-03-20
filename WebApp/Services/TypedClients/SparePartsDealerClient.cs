using HelperClasses.DTOs;
using HelperClasses.DTOs.SparePartsDealer;
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
    public class SparePartsDealerClient : ISparePartsDealerClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartsDealerClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;

        public SparePartsDealerClient(HttpClient client, ILogger<SparePartsDealerClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<SparePartsDealerDTO> GetSparePartsDealerByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SparePartsDealer/{Id}");

            return await _clientHelper.ParseResponseAsync<SparePartsDealerDTO>(HttpResponse);
        }
        public async Task<IEnumerable<SparePartsDealerDTO>> GetSparePartsDealers()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SparePartsDealer");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartsDealerDTO>>(HttpResponse);
        }
        public async Task<SparePartsDealerDTO> ToggleActiveStatus(long Id, bool flag, string RejectionReason = "")
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SparePartsDealer/{Id}/ToggleActiveStatus/{flag}?RejectionReason={RejectionReason}");

            return await _clientHelper.ParseResponseAsync<SparePartsDealerDTO>(HttpResponse);
        }
        public async Task<SparePartsDealerDTO> Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"SparePartsDealer/{Id}");

            return await _clientHelper.ParseResponseAsync<SparePartsDealerDTO>(response);
        }
        public async Task<SparePartsDealerRegisterDTO> Update(SparePartsDealerRegisterDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("ServiceStaff/SparePartDealer", content);

            return await _clientHelper.ParseResponseAsync<SparePartsDealerRegisterDTO>(HttpResponse);
        }
        public async Task<SparePartsDealerDTO> GetSparePartsDealerByUser()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SparePartsDealer/ByUser");

            return await _clientHelper.ParseResponseAsync<SparePartsDealerDTO>(HttpResponse);
        }
        public async Task<object> UpdateSparePartsDealer(SparePartsDealerDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("SparePartsDealer", content);

            return await _clientHelper.ParseResponseAsync<object>(HttpResponse);
        }
        public async Task<string> UpdateProfilePicture(long SparePartsDealerId, string Path)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HelperClasses.DTOs.GarageCMS.ThemeColorAndLogoDTO model = new()
            {
                Logo = Path
            };

            HttpContent content = _clientHelper.CreateHttpContent(model);

            var HttpResponse = await _client.PutAsync($"SparePartsDealer/{SparePartsDealerId}/ProfilePicture", content);

            return await HttpResponse.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateTheme(long SparePartsDealerId, string Theme)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HelperClasses.DTOs.GarageCMS.ThemeColorAndLogoDTO model = new()
            {
                ThemeColor = Theme
            };

            HttpContent content = _clientHelper.CreateHttpContent(model);

            var HttpResponse = await _client.PutAsync($"SparePartsDealer/{SparePartsDealerId}/Theme", content);

            return await HttpResponse.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateThumbnail(long SparePartsDealerId, string Path)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HelperClasses.DTOs.GarageCMS.ThemeColorAndLogoDTO model = new()
            {
                Thumbnail = Path
            };

            HttpContent content = _clientHelper.CreateHttpContent(model);

            var HttpResponse = await _client.PutAsync($"SparePartsDealer/{SparePartsDealerId}/ThumbnailImage", content);

            return await HttpResponse.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateSecondaryLogo(long SparePartsDealerId, string Path)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HelperClasses.DTOs.GarageCMS.ThemeColorAndLogoDTO model = new()
            {
                SecondaryLogo = Path
            };

            HttpContent content = _clientHelper.CreateHttpContent(model);

            var HttpResponse = await _client.PutAsync($"SparePartsDealer/{SparePartsDealerId}/SecondaryLogo", content);

            return await HttpResponse.Content.ReadAsStringAsync();
        }
    }
}
