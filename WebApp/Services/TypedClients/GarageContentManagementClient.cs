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
    public class GarageContentManagementClient : IGarageContentManagementClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageContentManagementClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public GarageContentManagementClient(HttpClient client, ILogger<GarageContentManagementClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<IEnumerable<GarageContentManagementDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/ContentManagement");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageContentManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageContentManagementDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/ContentManagement/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageContentManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageContentManagementDTO>> GetAllByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{Id}/ContentManagement");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageContentManagementDTO>>(HttpResponse);
        }


        public async Task<GarageContentManagementDTO> AddGarageContentManagementAsync(GarageContentManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Garage/ContentManagement", content);

            return await _clientHelper.ParseResponseAsync<GarageContentManagementDTO>(HttpResponse);
        }

        public async Task<GarageContentManagementDTO> UpdateGarageContentManagementAsync(GarageContentManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Garage/ContentManagement", content);

            return await _clientHelper.ParseResponseAsync<GarageContentManagementDTO>(HttpResponse);
        }

        public async Task DeleteGarageContentManagementAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/ContentManagement/{Id}");
        }
    }
}
