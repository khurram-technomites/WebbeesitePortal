using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApp.Services.TypedClients
{
    public class GarageBlogClient: IGarageBlogClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageBlogClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public GarageBlogClient(HttpClient client, ILogger<GarageBlogClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<GarageBlogDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/Blog");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageBlogDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageBlogDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/Blog/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageBlogDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageBlogDTO>> GetAllByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{Id}/Blog/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageBlogDTO>>(HttpResponse);
        }
        public async Task<long> GetCountByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{Id}/Blog/Count/");
            return await _clientHelper.ParseResponseAsync<long>(HttpResponse);
        }


        public async Task<GarageBlogDTO> AddGarageBlogAsync(GarageBlogDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Garage/Blog", content);

            return await _clientHelper.ParseResponseAsync<GarageBlogDTO>(HttpResponse);
        }

        public async Task<GarageBlogDTO> UpdateGarageBlogAsync(GarageBlogDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Garage/Blog", content);

            return await _clientHelper.ParseResponseAsync<GarageBlogDTO>(HttpResponse);
        }

        public async Task DeleteGarageBlogAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/Blog/{Id}");
        }

        public async Task<GarageBlogDTO> ToggleStatus(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/Blog/ToggleStatus/{Id}");

            return await _clientHelper.ParseResponseAsync<GarageBlogDTO>(HttpResponse);
        }
    }
}
