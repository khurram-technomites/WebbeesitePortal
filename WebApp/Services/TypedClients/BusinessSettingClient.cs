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
    public class BusinessSettingClient : IBusinessSettingClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<BusinessSettingClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public BusinessSettingClient(HttpClient client, ILogger<BusinessSettingClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<BusinessSettingDTO> Create(BusinessSettingDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("BusinessSetting", content);

            return await _clientHelper.ParseResponseAsync<BusinessSettingDTO>(HttpResponse);
        }

        public async Task<BusinessSettingDTO> Update(BusinessSettingDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("BusinessSetting", content);

            return await _clientHelper.ParseResponseAsync<BusinessSettingDTO>(HttpResponse);
        }

        public async Task<IEnumerable<BusinessSettingDTO>> GetBusinessSettings()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            /*HttpContent content = _clientHelper.CreateHttpContent();*/
            var HttpResponse = await _client.GetAsync("BusinessSetting");

            return await _clientHelper.ParseResponseAsync<IEnumerable<BusinessSettingDTO>>(HttpResponse);
        }
    }
}
