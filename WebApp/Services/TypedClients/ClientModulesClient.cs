using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces.TypedClients;
using HelperClasses.DTOs;
using Microsoft.Extensions.Logging;
namespace WebApp.Services.TypedClients
{
    public class ClientModulesClient: IClientModulesClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ClientModulesClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public ClientModulesClient(HttpClient client, ILogger<ClientModulesClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<ClientModulesDTO> Create(ClientModulesDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("ClientDomainSuggestions", content);

            return await _clientHelper.ParseResponseAsync<ClientModulesDTO>(HttpResponse);
        }
        public async Task<IEnumerable<ClientModulesDTO>> CreateRange(IEnumerable<ClientModulesDTO> model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("ClientDomainSuggestions/Range", content);

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientModulesDTO>>(HttpResponse);
        }
        public async Task Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"ClientDomainSuggestions/{Id}");


        }


        public async Task<IEnumerable<ClientModulesDTO>> GetClientModule()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("ClientDomainSuggestions");

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientModulesDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<ClientModulesDTO>> GetClientModuleByClientId(long ClientId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"ClientModules/ClientId/{ClientId}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientModulesDTO>>(HttpResponse);
        }
        public async Task<LayoutModuleDTO> GetModuleByClientId(long ClientId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"ClientModules/{ClientId}/ForModule");

            return await _clientHelper.ParseResponseAsync<LayoutModuleDTO>(HttpResponse);
        }

        public async Task<ClientModulesDTO> GetClientModuleByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"ClientDomainSuggestions/{Id}");

            return await _clientHelper.ParseResponseAsync<ClientModulesDTO>(HttpResponse);
        }
    }
}
