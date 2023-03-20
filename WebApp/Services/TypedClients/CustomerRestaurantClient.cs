using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces.TypedClients;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Fatoorah;
using HelperClasses.DTOs.Order;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Services.TypedClients
{
    public class CustomerRestaurantClient: ICustomerRestaurantClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<CustomerRestaurantClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public CustomerRestaurantClient(HttpClient client, ILogger<CustomerRestaurantClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<object> Paid(long OrderId, string PaymentId)
        {
            _logger.LogError(string.Format("Paid API CHECK OrderId = {0}, PaymentId = {1}", OrderId, PaymentId));
            //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Customer/Restaurant/Paid/" + OrderId + "?PaymentId=" + PaymentId);

            return await _clientHelper.ParseResponseAsync<object>(HttpResponse);
        }
    }
}
