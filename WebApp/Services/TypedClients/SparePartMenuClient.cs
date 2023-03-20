using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HelperClasses.DTOs.SparePartCMS;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class SparePartMenuClient : ISparePartMenuClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartMenuClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SparePartMenuClient(HttpClient client, ILogger<SparePartMenuClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<SparePartMenuDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/Menu");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartMenuDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartMenuDTO>> GetMenuBySparePartId(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/Menu/BySparePart/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartMenuDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartMenuDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/Menu/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartMenuDTO>>(HttpResponse);
        }


        public async Task<SparePartMenuDTO> AddSparePartMenuAsync(SparePartMenuDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SpareParts/Menu/", content);

            return await _clientHelper.ParseResponseAsync<SparePartMenuDTO>(HttpResponse);
        }

        public async Task<SparePartMenuDTO> UpdateSparePartMenuAsync(SparePartMenuDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("SpareParts/Menu/", content);

            return await _clientHelper.ParseResponseAsync<SparePartMenuDTO>(HttpResponse);
        }

        public async Task DeleteSparePartMenuAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SpareParts/Menu/{Id}");
        }
    }
}
