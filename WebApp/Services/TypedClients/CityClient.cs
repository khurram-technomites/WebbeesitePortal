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
    public class CityClient : ICityClient 
    {
        private readonly HttpClient _client;
        private readonly ILogger<CityClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public CityClient(HttpClient client, ILogger<CityClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<CityDTO> Create(CityDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("City", content);

            return await _clientHelper.ParseResponseAsync<CityDTO>(HttpResponse);
        }

        public async Task<CityDTO> Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"City/{Id}");

            return await _clientHelper.ParseResponseAsync<CityDTO>(response);
        }

        public async Task<CityDTO> Edit(CityDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("City", content);

            return await _clientHelper.ParseResponseAsync<CityDTO>(HttpResponse);
        }

        public async Task<IEnumerable<CityDTO>> GetCities()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("City");

            return await _clientHelper.ParseResponseAsync<IEnumerable<CityDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<CityDTO>> GetCitiesMaster()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("City/Master");

            return await _clientHelper.ParseResponseAsync<IEnumerable<CityDTO>>(HttpResponse);
        }

        public async Task<CityDTO> GetCityByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"City/{Id}");

            return await _clientHelper.ParseResponseAsync<CityDTO>(HttpResponse);
        }
        public async Task<IEnumerable<CityDTO>> GetCityByCountryId(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"City/Countries/{Id}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<CityDTO>>(HttpResponse);
        }
        public async Task<CityDTO> ToggleActiveStatus(long CityId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            //HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.GetAsync($"City/ToggleStatus/{CityId}");

            return await _clientHelper.ParseResponseAsync<CityDTO>(HttpResponse);
        }
    }

}
