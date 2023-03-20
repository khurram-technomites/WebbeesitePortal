using HelperClasses.DTOs.Garage;
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
using WebApp.ViewModels;

namespace WebApp.Services.TypedClients
{
    public class GarageDocumentClient : IGarageDocumentClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageDocumentClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;

        public GarageDocumentClient(ITokenManager tokenManager, IHttpClientHelper clientHelper, ILogger<GarageDocumentClient> logger, HttpClient client)
        {
            _tokenManager = tokenManager;
            _clientHelper = clientHelper;
            _logger = logger;
            _client = client;
        }

        public async Task<GarageDocumentViewModel> AddGarage(GarageDocumentDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PostAsync("Garage/Document", content);

            return await _clientHelper.ParseResponseAsync<GarageDocumentViewModel>(HttpResponse);
        }

        public async Task Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/Document/{Id}");
        }

        public async Task<IEnumerable<GarageDocumentViewModel>> GetAllByGarage(long GarageId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{GarageId}/Document");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageDocumentViewModel>>(HttpResponse);
        }
    }
}
