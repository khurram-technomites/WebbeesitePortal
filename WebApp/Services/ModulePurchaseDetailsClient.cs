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

namespace WebApp.Services
{
    public class ModulePurchaseDetailsClient: IModulePurchaseDetailsClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ModulePurchaseDetailsClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public ModulePurchaseDetailsClient(HttpClient client, ILogger<ModulePurchaseDetailsClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<ModulePurchaseDetailsDTO> Create(ModulePurchaseDetailsDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("ModulePurchaseDetails", content);

            return await _clientHelper.ParseResponseAsync<ModulePurchaseDetailsDTO>(HttpResponse);
        }
        public async Task<List<ModulePurchaseDetailsDTO>> CreateRange(List<ModulePurchaseDetailsDTO> model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("ModulePurchaseDetails/Range", content);

            return await _clientHelper.ParseResponseAsync<List<ModulePurchaseDetailsDTO>>(HttpResponse);
        }
        public async Task<List<ModulePurchaseDetailsDTO>> UpdateRange(List<ModulePurchaseDetailsDTO> model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("ModulePurchaseDetails/Range", content);

            return await _clientHelper.ParseResponseAsync<List<ModulePurchaseDetailsDTO>>(HttpResponse);
        }


        public async Task<ModulePurchaseDetailsDTO> Edit(ModulePurchaseDetailsDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("ModulePurchaseDetails", content);

            return await _clientHelper.ParseResponseAsync<ModulePurchaseDetailsDTO>(HttpResponse);
        }

        public async Task<IEnumerable<ModulePurchaseDetailsDTO>> GetDetailsByPurchaseId(long PurchaseId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"ModulePurchaseDetails/PurchaseId/{PurchaseId}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<ModulePurchaseDetailsDTO>>(HttpResponse);
        }
        public async Task<int> GetDetailsByPurchaseIdAndName(long PurchaseId,string Name)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"ModulePurchaseDetails/{PurchaseId}/ByName/{Name}");

            return await _clientHelper.ParseResponseAsync<int>(HttpResponse);
        }
        public async Task Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"ModulePurchaseDetails/{Id}");
        }



        public async Task<ModulePurchaseDetailsDTO> GetDetailsByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"ModulePurchaseDetails/{Id}");

            return await _clientHelper.ParseResponseAsync<ModulePurchaseDetailsDTO>(HttpResponse);
        }

    }
}
