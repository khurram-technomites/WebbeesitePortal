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
    public class SparePartPartnersManagementClient : ISparePartPartnersManagementClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartPartnersManagementClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;

        public SparePartPartnersManagementClient(HttpClient client, ILogger<SparePartPartnersManagementClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<SparePartPartnersManagementDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/PartnersManagement");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartPartnersManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartPartnersManagementDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/PartnersManagement/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartPartnersManagementDTO>>(HttpResponse);
        }

        public async Task<SparePartPartnersManagementDTO> GetMaxPosition(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/{Id}/Position/");

            return await _clientHelper.ParseResponseAsync<SparePartPartnersManagementDTO>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartPartnersManagementDTO>> GetAllBySparePartDealerIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/{Id}/PartnersManagement/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartPartnersManagementDTO>>(HttpResponse);
        }


        public async Task<SparePartPartnersManagementDTO> AddSparePartPartnersManagementAsync(SparePartPartnersManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SpareParts/PartnersManagement", content);

            return await _clientHelper.ParseResponseAsync<SparePartPartnersManagementDTO>(HttpResponse);
        }

        public async Task<SparePartPartnersManagementDTO> UpdateSparePartPartnersManagementAsync(SparePartPartnersManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("SpareParts/PartnersManagement", content);

            return await _clientHelper.ParseResponseAsync<SparePartPartnersManagementDTO>(HttpResponse);
        }

        public async Task DeleteSparePartPartnersManagementAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SpareParts/PartnersManagement/{Id}");
        }
        public async Task<SparePartPartnersManagementDTO> SavePositions(SparePartPartnersManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SpareParts/PartnersManagement/SavePositions", content);

            return await _clientHelper.ParseResponseAsync<SparePartPartnersManagementDTO>(HttpResponse);
        }
    }
}
