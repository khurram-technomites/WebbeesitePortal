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
    public class ClientContentMediaClient: IClientContentMediaClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ClientContentMediaClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public ClientContentMediaClient(HttpClient client, ILogger<ClientContentMediaClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<ClientContentMediaDTO> Create(ClientContentMediaDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("ClientContentMedia", content);

            return await _clientHelper.ParseResponseAsync<ClientContentMediaDTO>(HttpResponse);
        }
        public async Task<IEnumerable<ClientContentMediaDTO>> CreateRange(IEnumerable<ClientContentMediaDTO> model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("ClientContentMedia/Range", content);

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientContentMediaDTO>>(HttpResponse);
        }
        public async Task Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"ClientContentMedia/{Id}");
        }

   
        public async Task<IEnumerable<ClientContentMediaDTO>> GetClientContent()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("ClientContentMedia");

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientContentMediaDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<ClientContentMediaDTO>> GetClientContentByClientId(long ClientId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"ClientContentMedia/ClientId/{ClientId}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientContentMediaDTO>>(HttpResponse);
        }

        public async Task<ClientContentMediaDTO> GetClientContentByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"ClientContentMedia/{Id}");

            return await _clientHelper.ParseResponseAsync<ClientContentMediaDTO>(HttpResponse);
        }
    }
}
