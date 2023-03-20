using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class GarageCustomerClient : IGarageCustomerClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageCustomerClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public GarageCustomerClient(HttpClient client, ILogger<GarageCustomerClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<object> Paid(long InvoiceId, string PaymentId)
        {
            _logger.LogError(string.Format("Paid API CHECK InvoiceId = {0}, PaymentId = {1}", InvoiceId, PaymentId));
            //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("GarageCustomer/Invoice/Paid/ " + InvoiceId + "?PaymentId=" + PaymentId);

            return await _clientHelper.ParseResponseAsync<object>(HttpResponse);
        }
    }
}
