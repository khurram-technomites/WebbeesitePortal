using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebAPI.Models;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using HelperClasses.DTOs.Garage;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HelperClasses.DTOs.SparePartCMS;

namespace WebApp.Services.TypedClients
{
    public class SparePartFAQClient : ISparePartFAQClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartFAQClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;

        public SparePartFAQClient(HttpClient client, ILogger<SparePartFAQClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<SparePartFAQDTO> AddFAQAsync(SparePartFAQDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PostAsync("SpareParts/FAQ", content);

            return await _clientHelper.ParseResponseAsync<SparePartFAQDTO>(HttpResponse);
        }

        public async Task<SparePartFAQDTO> ArchiveFAQAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.DeleteAsync($"SpareParts/FAQ/{Id}");

            return await _clientHelper.ParseResponseAsync<SparePartFAQDTO>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartFAQDTO>> GetFAQBySparePartIdAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/{Id}/FAQ");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartFAQDTO>>(HttpResponse);
        }

        public async Task<SparePartFAQDTO> GetFAQByIdAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/FAQ/{Id}");

            return await _clientHelper.ParseResponseAsync<SparePartFAQDTO>(HttpResponse);
        }

        public async Task<SparePartFAQDTO> SavePosition(SparePartFAQDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PutAsync("SpareParts/FAQ/SavePositions", content);

            return await _clientHelper.ParseResponseAsync<SparePartFAQDTO>(HttpResponse);
        }

        public async Task<SparePartFAQDTO> UpdateFAQAsync(SparePartFAQDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PutAsync("SpareParts/FAQ", content);

            return await _clientHelper.ParseResponseAsync<SparePartFAQDTO>(HttpResponse);
        }
    }
}
