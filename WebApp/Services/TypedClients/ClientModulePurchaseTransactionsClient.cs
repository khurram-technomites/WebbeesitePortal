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
    public class ClientModulePurchaseTransactionsClient: IClientModulePurchaseTransactionsClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ClientModulePurchaseTransactionsClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public ClientModulePurchaseTransactionsClient(HttpClient client, ILogger<ClientModulePurchaseTransactionsClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<IEnumerable<ClientModulePurchaseTransactionsDTO>> GetAllTransactionsAsync(long VendorId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            var HttpResponse = await _client.GetAsync($"ClientTransactions/ByVendor/{VendorId}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientModulePurchaseTransactionsDTO>>(HttpResponse);
        }
        
    }
}
