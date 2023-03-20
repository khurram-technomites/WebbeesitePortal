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
    public class ClientModulePurchasesClient:IClientModulePurchasesClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ClientModulePurchasesClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public ClientModulePurchasesClient(HttpClient client, ILogger<ClientModulePurchasesClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<ClientModulePurchasesDTO> Create(ClientModulePurchasesDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("ClientModulePurchases", content);

            return await _clientHelper.ParseResponseAsync<ClientModulePurchasesDTO>(HttpResponse);
        }



        public async Task<ClientModulePurchasesDTO> Edit(ClientModulePurchasesDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("ClientModulePurchases", content);

            return await _clientHelper.ParseResponseAsync<ClientModulePurchasesDTO>(HttpResponse);
        }

        public async Task<IEnumerable<ClientModulePurchasesDTO>> GetPurchaseByGarageId(long GarageId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"ClientModulePurchases/GarageId/{GarageId}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientModulePurchasesDTO>>(HttpResponse);
        }
        public async Task<object> Paid(long InvoiceId, string PaymentId)
        {
          
            var HttpResponse = await _client.GetAsync("ClientModulePurchases/Paid/ " + InvoiceId + "?PaymentId=" + PaymentId);

            return await _clientHelper.ParseResponseAsync<object>(HttpResponse);
        }
        public async Task<object> GenerateInvoice(ClientModulePurchasesDTO model)
        {
            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("ClientModulePurchases/Invoice", content);

            return await _clientHelper.ParseResponseAsync<object>(HttpResponse);
        }



        public async Task<ClientModulePurchasesDTO> GetPurchaseByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"ClientModulePurchases/{Id}");

            return await _clientHelper.ParseResponseAsync<ClientModulePurchasesDTO>(HttpResponse);
        }

    }
}
