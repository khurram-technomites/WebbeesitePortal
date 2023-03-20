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
    public class GarageMenuManagementClient: IGarageMenuManagementClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageMenuManagementClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public GarageMenuManagementClient(HttpClient client, ILogger<GarageMenuManagementClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<IEnumerable<GarageMenuManagementDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/MenuManagement");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageMenuManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageMenuManagementDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/MenuManagement/" + Id);

            return await _clientHelper.ParseResponseAsync< IEnumerable<GarageMenuManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageMenuManagementDTO>> GetAllByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{Id}/MenuManagement/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageMenuManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageMenuManagementDTO>> GetAllByGarageMenuIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/Menu/{Id}/Management/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageMenuManagementDTO>>(HttpResponse);
        }


        public async Task<GarageMenuManagementDTO> AddGarageMenuManagementAsync(GarageMenuManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Garage/MenuManagement", content);

            return await _clientHelper.ParseResponseAsync<GarageMenuManagementDTO>(HttpResponse);
        }

        public async Task<GarageMenuManagementDTO> SavePositions(GarageMenuManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Garage/MenuManagement/SavePositions", content);

            return await _clientHelper.ParseResponseAsync<GarageMenuManagementDTO>(HttpResponse);
        }

        public async Task<GarageMenuManagementDTO> UpdateGarageMenuManagementAsync(GarageMenuManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Garage/MenuManagement", content);

            return await _clientHelper.ParseResponseAsync<GarageMenuManagementDTO>(HttpResponse);
        }

        public async Task DeleteGarageMenuManagementAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/MenuManagement/{Id}");
        }
    }
}
