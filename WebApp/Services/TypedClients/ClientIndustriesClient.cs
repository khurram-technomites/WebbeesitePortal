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
    public class ClientIndustriesClient : IClientIndustriesClient 
    {
        private readonly HttpClient _client;
        private readonly ILogger<ClientIndustriesClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public ClientIndustriesClient(HttpClient client, ILogger<ClientIndustriesClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<ClientIndustriesDTO> Create(ClientIndustriesDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("ClientIndustries", content);

            return await _clientHelper.ParseResponseAsync<ClientIndustriesDTO>(HttpResponse);
        }

        public async Task<ClientIndustriesDTO> Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"ClientIndustries/{Id}");

            return await _clientHelper.ParseResponseAsync<ClientIndustriesDTO>(response);
        }

        public async Task<ClientIndustriesDTO> Edit(ClientIndustriesDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("ClientIndustries", content);

            return await _clientHelper.ParseResponseAsync<ClientIndustriesDTO>(HttpResponse);
        }

        public async Task<IEnumerable<ClientIndustriesDTO>> GetIndustries()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("ClientIndustries");

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientIndustriesDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<ClientIndustriesDTO>> GetCitiesMaster()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("ClientIndustries/Master");

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientIndustriesDTO>>(HttpResponse);
        }

        public async Task<ClientIndustriesDTO> GetCityByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"ClientIndustries/{Id}");

            return await _clientHelper.ParseResponseAsync<ClientIndustriesDTO>(HttpResponse);
        }
        public async Task<ClientIndustriesDTO> ToggleActiveStatus(long CityId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            //HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.GetAsync($"ClientIndustries/ToggleStatus/{CityId}");

            return await _clientHelper.ParseResponseAsync<ClientIndustriesDTO>(HttpResponse);
        }
    }

}
