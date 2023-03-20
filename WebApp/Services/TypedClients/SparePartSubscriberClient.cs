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
    public class SparePartSubscriberClient : ISparePartSubscriberClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartSubscriberClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SparePartSubscriberClient(HttpClient client, ILogger<SparePartSubscriberClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<SparePartSubscriberDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/Subscribers");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartSubscriberDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartSubscriberDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/Subscribers/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartSubscriberDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartSubscriberDTO>> GetAllBySparePartDealerIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/{Id}/Subscribers/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartSubscriberDTO>>(HttpResponse);
        }


        public async Task<SparePartSubscriberDTO> AddSparePartSubscriberAsync(SparePartSubscriberDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SpareParts/Subscribers", content);

            return await _clientHelper.ParseResponseAsync<SparePartSubscriberDTO>(HttpResponse);
        }

        public async Task<SparePartSubscriberDTO> UpdateSparePartSubscriberAsync(SparePartSubscriberDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("SpareParts/Subscribers", content);

            return await _clientHelper.ParseResponseAsync<SparePartSubscriberDTO>(HttpResponse);
        }

        public async Task DeleteSparePartSubscriberAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SpareParts/Subscribers/{Id}");
        }
    }
}
