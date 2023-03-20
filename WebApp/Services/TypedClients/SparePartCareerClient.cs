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
    public class SparePartCareerClient : ISparePartCareerClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartCareerClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SparePartCareerClient(HttpClient client, ILogger<SparePartCareerClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<SparePartCareerDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/Careers");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartCareerDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartCareerDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/Careers/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartCareerDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartCareerDTO>> GetAllBySparePartDealerIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/{Id}/Careers");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartCareerDTO>>(HttpResponse);
        }


        public async Task<SparePartCareerDTO> AddSparePartCareerDTOAsync(SparePartCareerDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SpareParts/Careers", content);

            return await _clientHelper.ParseResponseAsync<SparePartCareerDTO>(HttpResponse);
        }

        public async Task<SparePartCareerDTO> UpdateSparePartCareerDTOAsync(SparePartCareerDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("SpareParts/Careers", content);

            return await _clientHelper.ParseResponseAsync<SparePartCareerDTO>(HttpResponse);
        }

        public async Task DeleteSparePartCareerDTOAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SpareParts/Careers/{Id}");
        }
    }
}
