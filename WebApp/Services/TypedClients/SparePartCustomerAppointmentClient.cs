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
    public class SparePartCustomerAppointmentClient : ISparePartCustomerAppointmentClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartCustomerAppointmentClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SparePartCustomerAppointmentClient(HttpClient client, ILogger<SparePartCustomerAppointmentClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<IEnumerable<SparePartCustomerAppointmentDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/CustomerAppointment");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartCustomerAppointmentDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartCustomerAppointmentDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/CustomerAppointment/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartCustomerAppointmentDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartCustomerAppointmentDTO>> GetAllBySparePartDealerIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/{Id}/CustomerAppointment/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartCustomerAppointmentDTO>>(HttpResponse);
        }


        public async Task<SparePartCustomerAppointmentDTO> AddSparePartCustomerAppointmentAsync(SparePartCustomerAppointmentDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SpareParts/CustomerAppointment", content);

            return await _clientHelper.ParseResponseAsync<SparePartCustomerAppointmentDTO>(HttpResponse);
        }

        public async Task<SparePartCustomerAppointmentDTO> UpdateSparePartCustomerAppointmentAsync(SparePartCustomerAppointmentDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("SpareParts/CustomerAppointment", content);

            return await _clientHelper.ParseResponseAsync<SparePartCustomerAppointmentDTO>(HttpResponse);
        }

        public async Task DeleteSparePartCustomerAppointmenttAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SpareParts/CustomerAppointment/{Id}");
        }
    }
}
