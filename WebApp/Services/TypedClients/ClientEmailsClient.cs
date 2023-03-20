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
    public class ClientEmailsClient: IClientEmailsClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ClientEmailsClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public ClientEmailsClient(HttpClient client, ILogger<ClientEmailsClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<ClientEmailsDTO> Create(ClientEmailsDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("ClientEmail", content);

            return await _clientHelper.ParseResponseAsync<ClientEmailsDTO>(HttpResponse);
        }
        public async Task<IEnumerable<ClientEmailsDTO>> CreateRange(IEnumerable<ClientEmailsDTO> model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("ClientEmail/Range", content);

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientEmailsDTO>>(HttpResponse);
        }
        public async Task Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"ClientEmail/{Id}");

           
        }


        public async Task<IEnumerable<ClientEmailsDTO>> GetClientEmails()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("ClientEmail");

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientEmailsDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<ClientEmailsDTO>> GetClientEmailsByClientId(long ClientId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"ClientEmail/ClientId/{ClientId}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientEmailsDTO>>(HttpResponse);
        }

        public async Task<ClientEmailsDTO> GetClientEmailsByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"ClientEmail/{Id}");

            return await _clientHelper.ParseResponseAsync<ClientEmailsDTO>(HttpResponse);
        }
    }
}
