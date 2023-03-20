using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using HelperClasses.DTOs.Aggregators;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HelperClasses.DTOs.Restaurant;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class RestaurantUserLogManagementClient : IRestaurantUserLogManagementClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<RestaurantUserLogManagementClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public RestaurantUserLogManagementClient(HttpClient client, ILogger<RestaurantUserLogManagementClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<RestaurantUserLogManagementDTO>> GetUserLogManagementByRestaurantIdAsync(long RestaurantId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Restaurant/{RestaurantId}/UserLogManagement");

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantUserLogManagementDTO>>(HttpResponse);
        }
    }
}
