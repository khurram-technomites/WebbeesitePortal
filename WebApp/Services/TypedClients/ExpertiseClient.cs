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
    public class ExpertiseClient: IExpertiseClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ExpertiseClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;

        public ExpertiseClient(HttpClient client, ILogger<ExpertiseClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<ExpertiseDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Expertise");

            return await _clientHelper.ParseResponseAsync<IEnumerable<ExpertiseDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<ExpertiseDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Expertise/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<ExpertiseDTO>>(HttpResponse);
        }

        public async Task<ExpertiseDTO> AddExpertiseAsync(ExpertiseDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Expertise/", content);

            return await _clientHelper.ParseResponseAsync<ExpertiseDTO>(HttpResponse);
        }

        public async Task<ExpertiseDTO> UpdateExpertiseAsync(ExpertiseDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Expertise/", content);

            return await _clientHelper.ParseResponseAsync<ExpertiseDTO>(HttpResponse);
        }
        public async Task<ExpertiseDTO> ToggleActiveStatus(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            //HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.GetAsync($"Expertise/ToggleStatus/{Id}");

            return await _clientHelper.ParseResponseAsync<ExpertiseDTO>(HttpResponse);
        }


        public async Task DeleteExpertiseAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Expertise/{Id}");
        }
    }
}
