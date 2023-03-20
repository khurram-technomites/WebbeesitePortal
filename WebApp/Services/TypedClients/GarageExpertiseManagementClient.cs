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
    public class GarageExpertiseManagementClient: IGarageExpertiseManagementClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageExpertiseManagementClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public GarageExpertiseManagementClient(HttpClient client, ILogger<GarageExpertiseManagementClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<IEnumerable<GarageExpertiseManagementDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/ExpertiseManagement");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageExpertiseManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageExpertiseManagementDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/ExpertiseManagement/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageExpertiseManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageExpertiseManagementDTO>> GetAllByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{Id}/ExpertiseManagement/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageExpertiseManagementDTO>>(HttpResponse);
        }
        public async Task<long> GetCountAllByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/Count/{Id}/ExpertiseManagement/");

            return await _clientHelper.ParseResponseAsync<long>(HttpResponse);
        }

        public async Task<GarageExpertiseManagementDTO> AddGarageExpertiseManagementAsync(GarageExpertiseManagementDTO Entity)
        
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Garage/ExpertiseManagement", content);

            return await _clientHelper.ParseResponseAsync<GarageExpertiseManagementDTO>(HttpResponse);
        }

        public async Task<GarageExpertiseManagementDTO> UpdateGarageExpertiseManagementAsync(GarageExpertiseManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Garage/ExpertiseManagement", content);

            return await _clientHelper.ParseResponseAsync<GarageExpertiseManagementDTO>(HttpResponse);
        }

        public async Task ArchiveGarageExpertiseManagementAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/ExpertiseManagement/{Id}");
        }

        public async Task DeleteGarageExpertiseManagementAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/ExpertiseManagement/Delete/{Id}");
        }
    }
}
