using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using HelperClasses.DTOs.Aggregators;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HelperClasses.DTOs.CardScheme;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class CardSchemeClient : ICardSchemeClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<CardSchemeClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;

        public CardSchemeClient(HttpClient client, ILogger<CardSchemeClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<CardSchemeDTO>> GetAllCardSchemeAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("CardScheme");

            return await _clientHelper.ParseResponseJsonAsync<IEnumerable<CardSchemeDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<CardSchemeDTO>> GetCardSchemeByIdAsync(long CardSchemeId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("CardScheme/" + CardSchemeId);

            return await _clientHelper.ParseResponseJsonAsync<IEnumerable<CardSchemeDTO>>(HttpResponse);
        }
        public async Task<CardSchemeDTO> AddCardSchemeAsync(CardSchemeDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("CardScheme", content);

            return await _clientHelper.ParseResponseAsync<CardSchemeDTO>(HttpResponse);
        }
        public async Task<CardSchemeDTO> UpdateCardSchemeAsync(CardSchemeDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("CardScheme", content);

            return await _clientHelper.ParseResponseAsync<CardSchemeDTO>(HttpResponse);
        }
        public async Task DeleteCardSchemeAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"CardScheme/{Id}");
        }
    }
}
