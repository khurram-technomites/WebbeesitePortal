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
    public class ClientTypesClient : IClientTypesClient 
    {
        private readonly HttpClient _client;
        private readonly ILogger<CityClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public ClientTypesClient(HttpClient client, ILogger<CityClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<ClientTypesDTO> Create(ClientTypesDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("ClientTypes", content);

            return await _clientHelper.ParseResponseAsync<ClientTypesDTO>(HttpResponse);
        }

        public async Task<ClientTypesDTO> Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"ClientTypes/{Id}");

            return await _clientHelper.ParseResponseAsync<ClientTypesDTO>(response);
        }

        public async Task<ClientTypesDTO> Edit(ClientTypesDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("ClientTypes", content);

            return await _clientHelper.ParseResponseAsync<ClientTypesDTO>(HttpResponse);
        }

        public async Task<IEnumerable<ClientTypesDTO>> GetCities()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("ClientTypes");

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientTypesDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<ClientTypesDTO>> GetCitiesMaster()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("ClientTypes/Master");

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientTypesDTO>>(HttpResponse);
        }

        public async Task<ClientTypesDTO> GetCityByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"ClientTypes/{Id}");

            return await _clientHelper.ParseResponseAsync<ClientTypesDTO>(HttpResponse);
        }
        public async Task<ClientTypesDTO> ToggleActiveStatus(long CityId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            //HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.GetAsync($"ClientTypes/ToggleStatus/{CityId}");

            return await _clientHelper.ParseResponseAsync<ClientTypesDTO>(HttpResponse);
        }
    }

}
