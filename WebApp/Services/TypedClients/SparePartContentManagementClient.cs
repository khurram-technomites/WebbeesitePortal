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
    public class SparePartContentManagementClient : ISparePartContentManagementClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartContentManagementClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SparePartContentManagementClient(HttpClient client, ILogger<SparePartContentManagementClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<SparePartContentManagementDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/ContentManagement");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartContentManagementDTO>>(HttpResponse);
        }

        public async Task<SparePartContentManagementDTO> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/ContentManagement/" + Id);

            return await _clientHelper.ParseResponseAsync<SparePartContentManagementDTO>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartContentManagementDTO>> GetAllBySparePartDealerIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/{Id}/ContentManagement");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartContentManagementDTO>>(HttpResponse);
        }


        public async Task<SparePartContentManagementDTO> AddSparePartContentManagementAsync(SparePartContentManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SpareParts/ContentManagement", content);

            return await _clientHelper.ParseResponseAsync<SparePartContentManagementDTO>(HttpResponse);
        }

        public async Task<SparePartContentManagementDTO> UpdateSparePartContentManagementAsync(SparePartContentManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("SpareParts/ContentManagement", content);

            return await _clientHelper.ParseResponseAsync<SparePartContentManagementDTO>(HttpResponse);
        }

        public async Task DeleteSparePartContentManagementAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SpareParts/ContentManagement/{Id}");
        }
    }
}
