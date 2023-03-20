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
    public class SparePartMenuManagementClient : ISparePartMenuManagementClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartMenuManagementClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SparePartMenuManagementClient(HttpClient client, ILogger<SparePartMenuManagementClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<SparePartMenuManagementDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/MenuManagement");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartMenuManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartMenuManagementDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/MenuManagement/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartMenuManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartMenuManagementDTO>> GetAllBySparePartDealerIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/{Id}/MenuManagement/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartMenuManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartMenuManagementDTO>> GetAllBySparePartMenuIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/Menu/{Id}/Management/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartMenuManagementDTO>>(HttpResponse);
        }


        public async Task<SparePartMenuManagementDTO> AddSparePartMenuManagementAsync(SparePartMenuManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SpareParts/MenuManagement", content);

            return await _clientHelper.ParseResponseAsync<SparePartMenuManagementDTO>(HttpResponse);
        }

        public async Task<SparePartMenuManagementDTO> SavePositions(SparePartMenuManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SpareParts/MenuManagement/SavePositions", content);

            return await _clientHelper.ParseResponseAsync<SparePartMenuManagementDTO>(HttpResponse);
        }

        public async Task<SparePartMenuManagementDTO> UpdateSparePartMenuManagementAsync(SparePartMenuManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("SpareParts/MenuManagement", content);

            return await _clientHelper.ParseResponseAsync<SparePartMenuManagementDTO>(HttpResponse);
        }

        public async Task DeleteSparePartMenuManagementAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SpareParts/MenuManagement/{Id}");
        }
    }
}
