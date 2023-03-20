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
    public class GarageExpertiseClient: IGarageExpertiseClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageExpertiseClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public GarageExpertiseClient(HttpClient client, ILogger<GarageExpertiseClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<IEnumerable<GarageExpertiseDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/Expertise");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageExpertiseDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageExpertiseDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/Expertise/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageExpertiseDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageExpertiseDTO>> GetAllByGarageExpertiseManagementIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/Expertise/ByManagement/{Id}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageExpertiseDTO>>(HttpResponse);
        }


        public async Task<GarageExpertiseDTO> AddGarageExpertiseAsync(GarageExpertiseDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Garage/Expertise", content);

            return await _clientHelper.ParseResponseAsync<GarageExpertiseDTO>(HttpResponse);
        }

        public async Task<GarageExpertiseDTO> UpdateGarageExpertiseAsync(GarageExpertiseDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Garage/Expertise", content);

            return await _clientHelper.ParseResponseAsync<GarageExpertiseDTO>(HttpResponse);
        }

        public async Task ArchiveGarageExpertiseAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/Expertise/{Id}");
        }

        public async Task DeleteGarageExpertiseAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/Expertise/Delete/{Id}");
        }
    }
}
