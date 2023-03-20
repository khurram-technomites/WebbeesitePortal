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
    public class SparePartAppointmentManagementClient : ISparePartAppointmentManagementClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartAppointmentManagementClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SparePartAppointmentManagementClient(HttpClient client, ILogger<SparePartAppointmentManagementClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<SparePartAppointmentManagementDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/AppointmentManagement");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartAppointmentManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartAppointmentManagementDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/AppointmentManagement/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartAppointmentManagementDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartAppointmentManagementDTO>> GetAllBySparePartDealerIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/{Id}/AppointmentManagement/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartAppointmentManagementDTO>>(HttpResponse);
        }


        public async Task<SparePartAppointmentManagementDTO> AddSparePartAppointmentManagementAsync(SparePartAppointmentManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SpareParts/AppointmentManagement", content);

            return await _clientHelper.ParseResponseAsync<SparePartAppointmentManagementDTO>(HttpResponse);
        }

        public async Task<SparePartAppointmentManagementDTO> UpdateSparePartAppointmentManagementAsync(SparePartAppointmentManagementDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("SpareParts/AppointmentManagement", content);

            return await _clientHelper.ParseResponseAsync<SparePartAppointmentManagementDTO>(HttpResponse);
        }

        public async Task DeleteSparePartAppointmentManagementAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SpareParts/AppointmentManagement/{Id}");
        }
    }
}
