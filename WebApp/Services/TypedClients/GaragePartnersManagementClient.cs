﻿using Microsoft.Extensions.Logging;
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
    public class GaragePartnersManagementClient: IGaragePartnersManagementClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GaragePartnersManagementClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public GaragePartnersManagementClient(HttpClient client, ILogger<GaragePartnersManagementClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<IEnumerable<GaragePartnersManagementDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/PartnersManagement");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GaragePartnersManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GaragePartnersManagementDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/PartnersManagement/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<GaragePartnersManagementDTO>>(HttpResponse);
        }

        public async Task<GaragePartnersManagementDTO> GetMaxPosition(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{Id}/Position/");

            return await _clientHelper.ParseResponseAsync<GaragePartnersManagementDTO>(HttpResponse);
        }

        public async Task<IEnumerable<GaragePartnersManagementDTO>> GetAllByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{Id}/PartnersManagement/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GaragePartnersManagementDTO>>(HttpResponse);
        }

        public async Task<long> GetAllCountByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/Count/{Id}/PartnersManagement/");

            return await _clientHelper.ParseResponseAsync<long>(HttpResponse);
        }

        public async Task<GaragePartnersManagementDTO> AddGaragePartnersManagementAsync(GaragePartnersManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Garage/PartnersManagement", content);

            return await _clientHelper.ParseResponseAsync<GaragePartnersManagementDTO>(HttpResponse);
        }

        public async Task<GaragePartnersManagementDTO> UpdateGaragePartnersManagementAsync(GaragePartnersManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Garage/PartnersManagement", content);

            return await _clientHelper.ParseResponseAsync<GaragePartnersManagementDTO>(HttpResponse);
        }

        public async Task DeleteGaragePartnersManagementAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/PartnersManagement/{Id}");
        }
        public async Task<GaragePartnersManagementDTO> SavePositions(GaragePartnersManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Garage/PartnersManagement/SavePositions", content);

            return await _clientHelper.ParseResponseAsync<GaragePartnersManagementDTO>(HttpResponse);
        }
    }
}
