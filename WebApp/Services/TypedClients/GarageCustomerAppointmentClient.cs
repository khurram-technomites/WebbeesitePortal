using HelperClasses.DTOs.GarageCMS;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebAPI.Interfaces.IServices.Domains;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApp.Services.TypedClients
{
    public class GarageCustomerAppointmentClient: IGarageCustomerAppointmentClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageContentManagementClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;

        public GarageCustomerAppointmentClient(HttpClient client, ILogger<GarageContentManagementClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<GarageCustomerAppointmentDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/CustomerAppointment");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageCustomerAppointmentDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageCustomerAppointmentDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/CustomerAppointment/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageCustomerAppointmentDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageCustomerAppointmentDTO>> GetAllByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{Id}/CustomerAppointment/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageCustomerAppointmentDTO>>(HttpResponse);
        }


        public async Task<GarageCustomerAppointmentDTO> AddGarageCustomerAppointmentAsync(GarageCustomerAppointmentDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Garage/CustomerAppointment", content);

            return await _clientHelper.ParseResponseAsync<GarageCustomerAppointmentDTO>(HttpResponse);
        }

        public async Task<GarageCustomerAppointmentDTO> UpdateGarageCustomerAppointmentAsync(GarageCustomerAppointmentDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Garage/CustomerAppointment", content);

            return await _clientHelper.ParseResponseAsync<GarageCustomerAppointmentDTO>(HttpResponse);
        }

        public async Task DeleteGarageCustomerAppointmentAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/CustomerAppointment/{Id}");
        }
    }
}
