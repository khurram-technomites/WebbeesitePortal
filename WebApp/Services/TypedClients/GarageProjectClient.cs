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
    public class GarageProjectClient : IGarageProjectClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageProjectClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;

        public GarageProjectClient(HttpClient client, ILogger<GarageProjectClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<GarageProjectDTO> AddProjectAsync(GarageProjectDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PostAsync("Garage/Project", content);

            return await _clientHelper.ParseResponseAsync<GarageProjectDTO>(HttpResponse);
        }

        public async Task<GarageProjectDTO> ArchiveProjectAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.DeleteAsync($"Garage/Project/{Id}");

            return await _clientHelper.ParseResponseAsync<GarageProjectDTO>(HttpResponse);
        }

        public async Task<IEnumerable<GarageProjectDTO>> GetByGarageAsync(long GarageId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{GarageId}/Project");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageProjectDTO>>(HttpResponse);
        }
        public async Task<long> GetCountByGarageAsync(long GarageId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/Count/{GarageId}/Project");

            return await _clientHelper.ParseResponseAsync<long>(HttpResponse);
        }
        public async Task<GarageProjectDTO> GetByIdAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/Project/{Id}");

            return await _clientHelper.ParseResponseAsync<GarageProjectDTO>(HttpResponse);
        }

        public async Task<GarageProjectDTO> ToggleStatusAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.PutAsync($"Garage/Project/{Id}/Status", null);

            return await _clientHelper.ParseResponseAsync<GarageProjectDTO>(HttpResponse);
        }

        public async Task<GarageProjectDTO> UpdateProjectAsync(GarageProjectDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PutAsync("Garage/Project", content);

            return await _clientHelper.ParseResponseAsync<GarageProjectDTO>(HttpResponse);
        }
    }
}
