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
    public class CountriesClient : ICountryClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<CountriesClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;

        public CountriesClient(HttpClient client, ILogger<CountriesClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<CountryDTO> Create(CountryDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("Country", content);

            return await _clientHelper.ParseResponseAsync<CountryDTO>(HttpResponse);
        }

        public async Task<CountryDTO> Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"Country/{Id}");

            return await _clientHelper.ParseResponseAsync<CountryDTO>(response);            
        }

        public async Task<CountryDTO> Edit(CountryDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("Country", content);

            return await _clientHelper.ParseResponseAsync<CountryDTO>(HttpResponse);
        }

        public async Task<IEnumerable<CountryDTO>> GetCountries()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Country");

            return await _clientHelper.ParseResponseAsync<IEnumerable<CountryDTO>>(HttpResponse);
        }


        public async Task<IEnumerable<CountryDTO>> GetCountriesByMaster()
        {

            var HttpResponse = await _client.GetAsync("Country/Master");

            return await _clientHelper.ParseResponseAsync<IEnumerable<CountryDTO>>(HttpResponse);
        }

        public async Task<CountryDTO> GetCountryByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Country/{Id}");

            return await _clientHelper.ParseResponseAsync<CountryDTO>(HttpResponse);
        }
        public async Task<CountryDTO> ToggleActiveStatus(long CountryId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            //HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.GetAsync($"Country/ToggleStatus/{CountryId}");

            return await _clientHelper.ParseResponseAsync<CountryDTO>(HttpResponse);
        }
    }
}
