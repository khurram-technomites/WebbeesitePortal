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
    public class GarageRepairClient : IGarageRepairClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageRepairClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public GarageRepairClient(HttpClient client, ILogger<GarageRepairClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<GarageRepairSpecificationViewModel> Add(long GarageId, long CarMakeId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.PostAsync($"Garage/{GarageId}/Specification/{CarMakeId}", null);

            return await _clientHelper.ParseResponseAsync<GarageRepairSpecificationViewModel>(HttpResponse);
        }

        public async Task Delete(long GarageId, long CarMakeId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/{GarageId}/Specification/{CarMakeId}");
        }
    }
}
