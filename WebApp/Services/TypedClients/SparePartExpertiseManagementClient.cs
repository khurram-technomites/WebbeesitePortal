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
    public class SparePartExpertiseManagementClient : ISparePartExpertiseManagementClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartExpertiseManagementClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SparePartExpertiseManagementClient(HttpClient client, ILogger<SparePartExpertiseManagementClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<SparePartExpertiseManagementDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/ExpertiseManagement");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartExpertiseManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartExpertiseManagementDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/ExpertiseManagement/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartExpertiseManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartExpertiseManagementDTO>> GetAllBySparePartDealerIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/{Id}/ExpertiseManagement/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartExpertiseManagementDTO>>(HttpResponse);
        }


        public async Task<SparePartExpertiseManagementDTO> AddSparePartExpertiseManagementAsync(SparePartExpertiseManagementDTO Entity)

        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SpareParts/ExpertiseManagement", content);

            return await _clientHelper.ParseResponseAsync<SparePartExpertiseManagementDTO>(HttpResponse);
        }

        public async Task<SparePartExpertiseManagementDTO> UpdateSparePartExpertiseManagementAsync(SparePartExpertiseManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("SpareParts/ExpertiseManagement", content);

            return await _clientHelper.ParseResponseAsync<SparePartExpertiseManagementDTO>(HttpResponse);
        }

        public async Task ArchiveSparePartExpertiseManagementAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SpareParts/ExpertiseManagement/{Id}");
        }

        public async Task DeleteSparePartExpertiseManagementAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SpareParts/ExpertiseManagement/Delete/{Id}");
        }
    }
}
