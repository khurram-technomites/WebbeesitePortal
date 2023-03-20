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
    public class GarageCareersClient: IGarageCareersClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageCareersClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public GarageCareersClient(HttpClient client, ILogger<GarageCareersClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<GarageCareerDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/Careers");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageCareerDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageCareerDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/Careers/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageCareerDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageCareerDTO>> GetAllByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{Id}/Careers");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageCareerDTO>>(HttpResponse);
        }


        public async Task<GarageCareerDTO> AddGarageCareerAsync(GarageCareerDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Garage/Careers", content);

            return await _clientHelper.ParseResponseAsync<GarageCareerDTO>(HttpResponse);
        }

        public async Task<GarageCareerDTO> UpdateGarageCareerAsync(GarageCareerDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Garage/Careers", content);

            return await _clientHelper.ParseResponseAsync<GarageCareerDTO>(HttpResponse);
        }

        public async Task DeleteGarageCareerAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/Careers/{Id}");
        }
    }
}
