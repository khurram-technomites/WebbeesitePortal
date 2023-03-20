using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HelperClasses.DTOs.SparePartCMS;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class SparePartTeamManagementClient : ISparePartTeamManagementClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageTeamManagementClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SparePartTeamManagementClient(HttpClient client, ILogger<GarageTeamManagementClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<SparePartTeamManagementDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/TeamManagement");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartTeamManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartTeamManagementDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/TeamManagement/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartTeamManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartTeamManagementDTO>> GetAllBySparePartDealerIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/{Id}/TeamManagement/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartTeamManagementDTO>>(HttpResponse);
        }


        public async Task<SparePartTeamManagementDTO> AddSparePartTeamManagementAsync(SparePartTeamManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SpareParts/TeamManagement", content);

            return await _clientHelper.ParseResponseAsync<SparePartTeamManagementDTO>(HttpResponse);
        }

        public async Task<SparePartTeamManagementDTO> UpdateSparePartTeamManagementAsync(SparePartTeamManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("SpareParts/TeamManagement", content);

            return await _clientHelper.ParseResponseAsync<SparePartTeamManagementDTO>(HttpResponse);
        }

        public async Task DeleteSparePartTeamManagementAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SpareParts/TeamManagement/{Id}");
        }
    }
}
