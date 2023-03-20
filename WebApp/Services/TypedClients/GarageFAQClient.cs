using HelperClasses.DTOs.Garage;
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
    public class GarageFAQClient : IGarageFAQClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageFAQClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;

        public GarageFAQClient(HttpClient client, ILogger<GarageFAQClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<GarageFAQDTO> AddFAQAsync(GarageFAQDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PostAsync("Garage/FAQ", content);

            return await _clientHelper.ParseResponseAsync<GarageFAQDTO>(HttpResponse);
        }

        public async Task<GarageFAQDTO> ArchiveFAQAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.DeleteAsync($"Garage/FAQ/{Id}");

            return await _clientHelper.ParseResponseAsync<GarageFAQDTO>(HttpResponse);
        }

        public async Task<IEnumerable<GarageFAQDTO>> GetFAQByGarageAsync(long GarageId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{GarageId}/FAQ");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageFAQDTO>>(HttpResponse);
        }

        public async Task<GarageFAQDTO> GetFAQByIdAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/FAQ/{Id}");

            return await _clientHelper.ParseResponseAsync<GarageFAQDTO>(HttpResponse);
        }

        public async Task<GarageFAQDTO> SavePosition(GarageFAQDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PutAsync("Garage/FAQ/SavePositions", content);

            return await _clientHelper.ParseResponseAsync<GarageFAQDTO>(HttpResponse);
        }

        public async Task<GarageFAQDTO> UpdateFAQAsync(GarageFAQDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PutAsync("Garage/FAQ", content);

            return await _clientHelper.ParseResponseAsync<GarageFAQDTO>(HttpResponse);
        }
    }
}
