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
    public class SparePartExpertiseClient : ISparePartExpertiseClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartExpertiseClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SparePartExpertiseClient(HttpClient client, ILogger<SparePartExpertiseClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<SparePartExpertiseDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/Expertise");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartExpertiseDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartExpertiseDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/Expertise/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartExpertiseDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartExpertiseDTO>> GetAllBySparePartExpertiseManagementIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/Expertise/ByManagement/{Id}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartExpertiseDTO>>(HttpResponse);
        }


        public async Task<SparePartExpertiseDTO> AddSparePartExpertiseAsync(SparePartExpertiseDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SpareParts/Expertise", content);

            return await _clientHelper.ParseResponseAsync<SparePartExpertiseDTO>(HttpResponse);
        }

        public async Task<SparePartExpertiseDTO> UpdateSparePartExpertiseAsync(SparePartExpertiseDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("SpareParts/Expertise", content);

            return await _clientHelper.ParseResponseAsync<SparePartExpertiseDTO>(HttpResponse);
        }

        public async Task ArchiveSparePartExpertiseAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SpareParts/Expertise/{Id}");
        }

        public async Task DeleteSparePartExpertiseAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SpareParts/Expertise/Delete/{Id}");
        }
    }
}
