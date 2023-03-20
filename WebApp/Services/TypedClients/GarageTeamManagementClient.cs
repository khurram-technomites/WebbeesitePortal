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
    public class GarageTeamManagementClient: IGarageTeamManagementClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageTeamManagementClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public GarageTeamManagementClient(HttpClient client, ILogger<GarageTeamManagementClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<GarageTeamManagementDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/TeamManagement");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageTeamManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageTeamManagementDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/TeamManagement/" + Id);

            return await _clientHelper.ParseResponseAsync <IEnumerable<GarageTeamManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageTeamManagementDTO>> GetAllByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{Id}/TeamManagement/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageTeamManagementDTO>>(HttpResponse);
        }
        public async Task<long> GetCountAllByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/Count/{Id}/TeamManagement/");

            return await _clientHelper.ParseResponseAsync<long>(HttpResponse);
        }


        public async Task<GarageTeamManagementDTO> AddGarageTeamManagementAsync(GarageTeamManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Garage/TeamManagement", content);

            return await _clientHelper.ParseResponseAsync<GarageTeamManagementDTO>(HttpResponse);
        }

        public async Task<GarageTeamManagementDTO> UpdateGarageTeamManagementAsync(GarageTeamManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Garage/TeamManagement", content);

            return await _clientHelper.ParseResponseAsync<GarageTeamManagementDTO>(HttpResponse);
        }

        public async Task DeleteGarageTeamManagementAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/TeamManagement/{Id}");
        }
    }
}
