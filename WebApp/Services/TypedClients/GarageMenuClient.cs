using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class GarageMenuClient: IGarageMenuClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageMenuClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public GarageMenuClient(HttpClient client, ILogger<GarageMenuClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<IEnumerable<GarageMenuDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/Menu");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageMenuDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageMenuDTO>> GetMenuByGarageId(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/Menu/Garages/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageMenuDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageMenuDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/Menu/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageMenuDTO>>(HttpResponse);
        }


        public async Task<GarageMenuDTO> AddGarageMenuAsync(GarageMenuDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Garage/Menu/", content);

            return await _clientHelper.ParseResponseAsync<GarageMenuDTO>(HttpResponse);
        }

        public async Task<GarageMenuDTO> UpdateGarageMenuAsync(GarageMenuDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Garage/Menu/", content);

            return await _clientHelper.ParseResponseAsync<GarageMenuDTO>(HttpResponse);
        }

        public async Task DeleteGarageMenuAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/Menu/{Id}");
        }
    }
}
