using HelperClasses.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces.TypedClients;
namespace WebApp.Services.TypedClients
{
    public class ClientDomainSuggestionsClient: IClientDomainSuggestionsClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ClientDomainSuggestionsClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public ClientDomainSuggestionsClient(HttpClient client, ILogger<ClientDomainSuggestionsClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<ClientDomainSuggestionsDTO> Create(ClientDomainSuggestionsDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("ClientDomainSuggestions", content);

            return await _clientHelper.ParseResponseAsync<ClientDomainSuggestionsDTO>(HttpResponse);
        }
        public async Task<IEnumerable<ClientDomainSuggestionsDTO>> CreateRange(IEnumerable<ClientDomainSuggestionsDTO> model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("ClientDomainSuggestions/Range", content);

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientDomainSuggestionsDTO>>(HttpResponse);
        }
        public async Task Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"ClientDomainSuggestions/{Id}");

           
        }


        public async Task<IEnumerable<ClientDomainSuggestionsDTO>> GetClientContentDomain()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("ClientDomainSuggestions");

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientDomainSuggestionsDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<ClientDomainSuggestionsDTO>> GetClientContentDomainByClientId(long ClientId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"ClientDomainSuggestions/ClientId/{ClientId}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientDomainSuggestionsDTO>>(HttpResponse);
        }

        public async Task<ClientDomainSuggestionsDTO> GetClientContentDomainByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"ClientDomainSuggestions/{Id}");

            return await _clientHelper.ParseResponseAsync<ClientDomainSuggestionsDTO>(HttpResponse);
        }
    }
}
