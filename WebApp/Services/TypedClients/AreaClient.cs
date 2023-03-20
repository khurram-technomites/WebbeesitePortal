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
    public class AreaClient : IAreaClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<AreaClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public AreaClient(HttpClient client, ILogger<AreaClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<AreaDTO> Create(AreaDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("Area", content);

            return await _clientHelper.ParseResponseAsync<AreaDTO>(HttpResponse);
        }

        public async Task<AreaDTO> Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"Area/{Id}");

            return await _clientHelper.ParseResponseAsync<AreaDTO>(response);
        }

        public async Task<AreaDTO> Edit(AreaDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("Area", content);

            return await _clientHelper.ParseResponseAsync<AreaDTO>(HttpResponse);
        }

        public async Task<IEnumerable<AreaDTO>> GetAreas()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Area");

            return await _clientHelper.ParseResponseAsync<IEnumerable<AreaDTO>>(HttpResponse);
        }

        public async Task<AreaDTO> GetAreaByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Area/{Id}");

            return await _clientHelper.ParseResponseAsync<AreaDTO>(HttpResponse);
        }
    }
}
