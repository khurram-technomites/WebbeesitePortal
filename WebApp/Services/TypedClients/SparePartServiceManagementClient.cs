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
    public class SparePartServiceManagementClient : ISparePartServiceManagementClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartServiceManagementClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SparePartServiceManagementClient(HttpClient client, ILogger<SparePartServiceManagementClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<SparePartServiceManagementDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/ServiceManagement");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartServiceManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartServiceManagementDTO>> GetAllByIdAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/ServiceManagement/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartServiceManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartServiceManagementDTO>> GetAllBySparePartDealerIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/{Id}/ServiceManagement/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartServiceManagementDTO>>(HttpResponse);
        }


        public async Task<SparePartServiceManagementDTO> AddSparePartServiceManagementAsync(SparePartServiceManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SpareParts/ServiceManagement", content);

            return await _clientHelper.ParseResponseAsync<SparePartServiceManagementDTO>(HttpResponse);
        }

        public async Task<SparePartServiceManagementDTO> UpdateSparePartServiceManagementAsync(SparePartServiceManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("SpareParts/ServiceManagement", content);

            return await _clientHelper.ParseResponseAsync<SparePartServiceManagementDTO>(HttpResponse);
        }

        public async Task DeleteSparePartServiceManagementAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SpareParts/ServiceManagement/{Id}");
        }

        public async Task<SparePartServiceManagementDTO> ToggleStatus(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/ServiceManagement/ToggleStatus/{Id}");

            return await _clientHelper.ParseResponseAsync<SparePartServiceManagementDTO>(HttpResponse);
        }
    }
}
