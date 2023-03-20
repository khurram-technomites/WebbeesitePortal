using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebAPI.Models;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApp.Services.TypedClients
{
    public class GarageAppointmentManagementClient: IGarageAppointmentManagementClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageAppointmentManagementClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;

        public GarageAppointmentManagementClient(HttpClient client, ILogger<GarageAppointmentManagementClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<IEnumerable<GarageAppointmentManagementDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/AppointmentManagement");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageAppointmentManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageAppointmentManagementDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/AppointmentManagement/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageAppointmentManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageAppointmentManagementDTO>> GetAllByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{Id}/AppointmentManagement/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageAppointmentManagementDTO>>(HttpResponse);
        }


        public async Task<GarageAppointmentManagementDTO> AddGarageAppointmentManagementAsync(GarageAppointmentManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Garage/AppointmentManagement", content);

            return await _clientHelper.ParseResponseAsync<GarageAppointmentManagementDTO>(HttpResponse);
        }

        public async Task<GarageAppointmentManagementDTO> UpdateGarageAppointmentManagementAsync(GarageAppointmentManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Garage/AppointmentManagement", content);

            return await _clientHelper.ParseResponseAsync<GarageAppointmentManagementDTO>(HttpResponse);
        }

        public async Task DeleteGarageAppointmentManagementAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/AppointmentManagement/{Id}");
        }
    }
}
