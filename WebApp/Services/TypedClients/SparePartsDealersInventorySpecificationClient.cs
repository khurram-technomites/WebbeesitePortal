using HelperClasses.DTOs.SparePartsDealer;
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
    public class SparePartsDealersInventorySpecificationClient : ISparePartsDealerInventorySpecificationClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ISparePartsDealerInventorySpecificationClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public SparePartsDealersInventorySpecificationClient(HttpClient client, ILogger<ISparePartsDealerInventorySpecificationClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<SparePartsDealerSpecificationsDTO> AddSparePartsDealerInventorySpecification(SparePartsDealerSpecificationsDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("SparePartsDealerInventorySpecifications", content);

            return await _clientHelper.ParseResponseAsync<SparePartsDealerSpecificationsDTO>(HttpResponse);
        }

        public async Task DeleteSparePartsDealerInventorySpecification(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"SparePartsDealerInventorySpecifications/{Id}");
        }

        public async Task<SparePartsDealerSpecificationsDTO> GetSparePartsDealerInventorySpecificationByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SparePartsDealerInventorySpecifications/{Id}");

            return await _clientHelper.ParseResponseAsync<SparePartsDealerSpecificationsDTO>(HttpResponse);
        }
        public async Task<IEnumerable<SparePartsDealerSpecificationsDTO>> GetSparePartsDealerInventorySpecificationBySpareParts()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SparePartsDealerInventorySpecifications");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartsDealerSpecificationsDTO>>(HttpResponse);
        }
        public async Task<SparePartsDealerSpecificationsDTO> GetSparePartsDealerInventorySpecificationBySparePartsDealerID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SparePartsDealerInventorySpecifications/{Id}");

            return await _clientHelper.ParseResponseAsync<SparePartsDealerSpecificationsDTO>(HttpResponse);
        }

        public async Task<SparePartsDealerSpecificationsDTO> UpdateSparePartsDealerInventorySpecification(SparePartsDealerSpecificationsDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("SparePartsDealerInventorySpecifications", content);

            return await _clientHelper.ParseResponseAsync<SparePartsDealerSpecificationsDTO>(HttpResponse);
        }

        public async Task<SparePartsDealerSpecificationsDTO> GetSparePartsDealerInventorySpecificationByCarMakeID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SparePartsDealerInventorySpecifications/CarMake/{Id}");

            return await _clientHelper.ParseResponseAsync<SparePartsDealerSpecificationsDTO>(HttpResponse);
        }
        public async Task<SparePartsDealerSpecificationsDTO> GetSparePartsDealerInventorySpecificationByCarModelID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SparePartsDealerInventorySpecifications/{Id}");

            return await _clientHelper.ParseResponseAsync<SparePartsDealerSpecificationsDTO>(HttpResponse);
        }
    }
}
