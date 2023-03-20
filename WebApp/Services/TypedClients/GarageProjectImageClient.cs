using HelperClasses.DTOs.GarageCMS;
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
    public class GarageProjectImageClient : IGarageProjectImageClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageProjectImageClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;

        public GarageProjectImageClient(HttpClient client, ILogger<GarageProjectImageClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<GarageProjectImageDTO> AddProjectImage(GarageProjectImageDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PostAsync("Garage/Project/Image", content);

            return await _clientHelper.ParseResponseAsync<GarageProjectImageDTO>(HttpResponse);
        }

        public async Task<IEnumerable<GarageProjectImageDTO>> GetByProject(long ProjectId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/Project/{ProjectId}/Images");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageProjectImageDTO>>(HttpResponse);
        }
    }
}
