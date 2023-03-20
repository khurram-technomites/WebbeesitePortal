using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebAPI.Models;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApp.Services.TypedClients
{
    public class GarageSubscribersClient: IGarageSubscribersClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageSubscribersClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public GarageSubscribersClient(HttpClient client, ILogger<GarageSubscribersClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<GarageSubscribersDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/Subscribers");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageSubscribersDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageSubscribersDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/Subscribers/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageSubscribersDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageSubscribersDTO>> GetAllByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{Id}/Subscribers/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageSubscribersDTO>>(HttpResponse);
        }


        public async Task<GarageSubscribersDTO> AddGarageSubscribersAsync(GarageSubscribersDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Garage/Subscribers", content);

            return await _clientHelper.ParseResponseAsync<GarageSubscribersDTO>(HttpResponse);
        }

        public async Task<GarageSubscribersDTO> UpdateGarageSubscribersAsync(GarageSubscribersDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Garage/Subscribers", content);

            return await _clientHelper.ParseResponseAsync<GarageSubscribersDTO>(HttpResponse);
        }

        public async Task DeleteGarageSubscribersAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/Subscribers/{Id}");
        }
    }
}
