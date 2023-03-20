using HelperClasses.DTOs;
using HelperClasses.DTOs.ServiceAndDeliveryStaffDTO;
using HelperClasses.DTOs.ServiceStaff;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class ServiceStaffClient : IServiceStaffClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ServiceStaffClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public ServiceStaffClient(HttpClient client, ILogger<ServiceStaffClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<ServiceStaffRegisterDTO> AddServiceStaffAsync(ServiceAndDeliveryStaffRegisterDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("ServiceAndDeliveryStaffAccount/Register", content);
            return await _clientHelper.ParseResponseAsync<ServiceStaffRegisterDTO>(HttpResponse);
        }

        public async Task DeleteServiceStaffAsync(long ServiceStaffId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"ServiceStaff/{ServiceStaffId}");
        }

        public async Task<IEnumerable<ServiceStaffDTO>> GetAllServiceStaffsAsync(PagingParameters paging)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent httpContent = _clientHelper.CreateHttpContent(paging);
            var HttpResponse = await _client.PostAsync("ServiceStaff/GetAll", httpContent);

            return await _clientHelper.ParseResponseAsync<IEnumerable<ServiceStaffDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<ServiceStaffDTO>> GetAllServiceStaffsAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.PostAsync("ServiceStaff/GetAll", null);

            return await _clientHelper.ParseResponseAsync<IEnumerable<ServiceStaffDTO>>(HttpResponse);
        }

        public async Task<ServiceStaffDTO> GetServiceStaffByIdAsync(long ServiceStaffId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"ServiceStaff/{ServiceStaffId}");

            return await _clientHelper.ParseResponseAsync<ServiceStaffDTO>(HttpResponse);
        }

        public async Task<ServiceStaffRegisterDTO> UpdateServiceStaffAsync(ServiceAndDeliveryStaffRegisterDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("ServiceAndDeliveryStaffAccount/Update", content);

            return await _clientHelper.ParseResponseAsync<ServiceStaffRegisterDTO>(HttpResponse);
        }
        public async Task<ServiceStaffDTO> ToggleActiveStatus(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            //HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.GetAsync($"ServiceStaff/ToggleStatus/{Id}");

            return await _clientHelper.ParseResponseAsync<ServiceStaffDTO>(HttpResponse);
        }


    }
}
