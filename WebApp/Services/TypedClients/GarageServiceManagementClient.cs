using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebAPI.Models;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class GarageServiceManagementClient: IGarageServiceManagementClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageContentManagementClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public GarageServiceManagementClient(HttpClient client, ILogger<GarageContentManagementClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<GarageServiceManagementDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/ServiceManagement");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageServiceManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageServiceManagementDTO>> GetAllByIdAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/ServiceManagement/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageServiceManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageServiceManagementDTO>> GetAllByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{Id}/ServiceManagement/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageServiceManagementDTO>>(HttpResponse);
        }
        public async Task<long> GetCountAllByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/Count/{Id}/ServiceManagement/");

            return await _clientHelper.ParseResponseAsync<long>(HttpResponse);
        }


        public async Task<GarageServiceManagementDTO> AddGarageServiceManagementAsync(GarageServiceManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Garage/ServiceManagement", content);

            return await _clientHelper.ParseResponseAsync<GarageServiceManagementDTO>(HttpResponse);
        }

        public async Task<GarageServiceManagementDTO> UpdateGarageServiceManagementAsync(GarageServiceManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Garage/ServiceManagement", content);

            return await _clientHelper.ParseResponseAsync<GarageServiceManagementDTO>(HttpResponse);
        }

        public async Task DeleteGarageServiceManagementAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/ServiceManagement/{Id}");
        }

        public async Task<GarageServiceManagementDTO> ToggleStatus(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/ServiceManagement/ToggleStatus/{Id}");

            return await _clientHelper.ParseResponseAsync<GarageServiceManagementDTO>(HttpResponse);
        }
    }
}
