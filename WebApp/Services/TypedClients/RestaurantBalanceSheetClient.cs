using HelperClasses.DTOs.Restaurant;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class RestaurantBalanceSheetClient : IRestaurantBalanceSheetClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<RestaurantBalanceSheetClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public RestaurantBalanceSheetClient(HttpClient client, ILogger<RestaurantBalanceSheetClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

		public async Task<IEnumerable<RestaurantBalanceSheetLogsDTO>> GetRestaurantBalanceSheetLogsByRestaurantAsync(long RestaurantId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Restaurant/{RestaurantId}/BalanceSheet/ShiftLogs");

            return await _clientHelper.ParseResponseJsonAsync<IEnumerable<RestaurantBalanceSheetLogsDTO>>(HttpResponse);
        }

		public async Task<IEnumerable<RestaurantBalanceSheetLogsDTO>> GetRestaurantBalanceSheetLogsByBranchAsync(long RestaurantBranchId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Restaurant/{RestaurantBranchId}/BalanceSheet/ShiftLogs");

            return await _clientHelper.ParseResponseJsonAsync<IEnumerable<RestaurantBalanceSheetLogsDTO>>(HttpResponse);
        }

        public async Task<RestaurantBalanceSheetLogsDTO> GetByIdAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Restaurant/BalanceSheet/{Id}/ShiftLogs");

            return await _clientHelper.ParseResponseJsonAsync<RestaurantBalanceSheetLogsDTO>(HttpResponse);
        }
    }
}
