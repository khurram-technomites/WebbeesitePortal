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
    public class GarageAwardClient : IGarageAwardClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageAwardClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public GarageAwardClient(HttpClient client, ILogger<GarageAwardClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<GarageAwardDTO> AddGarageAwardAsync(GarageAwardDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Garage/Award", content);

            return await _clientHelper.ParseResponseAsync<GarageAwardDTO>(HttpResponse);
        }

        public async Task DeleteGarageAwardAsync(long GarageAwardId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/Award/{GarageAwardId}");
        }

        public async Task<IEnumerable<GarageAwardDTO>> GetAllGarageAwardsAsync(long GarageId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/Award/GetAll/Garages/{GarageId}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageAwardDTO>>(HttpResponse);
        }
        public async Task<long> GetCountAllGarageAwardsAsync(long GarageId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/Award/GetAll/Garages/Count/{GarageId}");

            return await _clientHelper.ParseResponseAsync<long>(HttpResponse);
        }

        public async Task<GarageAwardDTO> GetGarageAwardByIdAsync(long GarageAwardId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/Award/" + GarageAwardId);

            return await _clientHelper.ParseResponseAsync<GarageAwardDTO>(HttpResponse);
        }

    

        public async Task<GarageAwardDTO> UpdateGarageAwardAsync(GarageAwardDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Garage/Award", content);

            return await _clientHelper.ParseResponseAsync<GarageAwardDTO>(HttpResponse);
        }

        
    }
}
