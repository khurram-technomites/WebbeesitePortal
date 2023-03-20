using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HelperClasses.DTOs.SparePartCMS;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class SparePartCustomerFeedbackClient : ISparePartCustomerFeedbackClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartCustomerFeedbackClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SparePartCustomerFeedbackClient(HttpClient client, ILogger<SparePartCustomerFeedbackClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<SparePartCustomerFeedbackDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/CustomerFeedback");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartCustomerFeedbackDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartCustomerFeedbackDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/CustomerFeedback/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartCustomerFeedbackDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartCustomerFeedbackDTO>> GetAllBySparePartDealerIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/{Id}/CustomerFeedback");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartCustomerFeedbackDTO>>(HttpResponse);
        }


        public async Task<SparePartCustomerFeedbackDTO> AddSparePartCustomerFeedbackAsync(SparePartCustomerFeedbackDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SpareParts/CustomerFeedback", content);

            return await _clientHelper.ParseResponseAsync<SparePartCustomerFeedbackDTO>(HttpResponse);
        }

        public async Task<SparePartCustomerFeedbackDTO> UpdateSparePartCustomerFeedbackAsync(SparePartCustomerFeedbackDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("SpareParts/CustomerFeedback", content);

            return await _clientHelper.ParseResponseAsync<SparePartCustomerFeedbackDTO>(HttpResponse);
        }

        public async Task DeleteSparePartCustomerFeedbacksync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SpareParts/CustomerFeedback/{Id}");
        }
    }
}
