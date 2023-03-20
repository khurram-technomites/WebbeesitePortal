using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApp.Services.TypedClients
{
    public class GarageCustomerFeedbackClient: IGarageCustomerFeedbackClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageCustomerFeedbackClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public GarageCustomerFeedbackClient(HttpClient client, ILogger<GarageCustomerFeedbackClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<GarageCustomerFeedbackDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/CustomerFeedback");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageCustomerFeedbackDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageCustomerFeedbackDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/CustomerFeedback/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageCustomerFeedbackDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageCustomerFeedbackDTO>> GetAllByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{Id}/CustomerFeedback");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageCustomerFeedbackDTO>>(HttpResponse);
        }


        public async Task<GarageCustomerFeedbackDTO> AddGarageCustomerFeedbackAsync(GarageCustomerFeedbackDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Garage/CustomerFeedback", content);

            return await _clientHelper.ParseResponseAsync<GarageCustomerFeedbackDTO>(HttpResponse);
        }

        public async Task<GarageCustomerFeedbackDTO> UpdateGarageCustomerFeedbackAsync(GarageCustomerFeedbackDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Garage/CustomerFeedback", content);

            return await _clientHelper.ParseResponseAsync<GarageCustomerFeedbackDTO>(HttpResponse);
        }

        public async Task DeleteGarageCustomerFeedbackAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/CustomerFeedback/{Id}");
        }
    }
}
