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
    public class RestaurantSubscriberClient : IRestaurantSubscriberClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<RestaurantSubscriberClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public RestaurantSubscriberClient(HttpClient client, ILogger<RestaurantSubscriberClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Restaurant/Subscriber/{Id}");
        }

        public async Task<IEnumerable<RestaurantSubcriberViewModel>> GetByRestaurant(long RestaurantId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Restaurant/{RestaurantId}/Subscriber");

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantSubcriberViewModel>>(HttpResponse);
        }

        public async Task SendMessage(string Email, string Body)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.PostAsync($"Restaurant/Subscriber/SendEmail?Email={Email}&Body={Body}", null);
        }
    }
}
