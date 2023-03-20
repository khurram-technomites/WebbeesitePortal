using HelperClasses.DTOs;
using HelperClasses.DTOs.DeliveryStaff;
using HelperClasses.DTOs.ServiceAndDeliveryStaffDTO;
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
    public class DeliveryStaffClient : IDeliveryStaffClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<DeliveryStaffClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public DeliveryStaffClient(HttpClient client, ILogger<DeliveryStaffClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<DeliveryStaffRegisterDTO> AddDeliveryStaffAsync(ServiceAndDeliveryStaffRegisterDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("ServiceAndDeliveryStaffAccount/Register", content);
            return await _clientHelper.ParseResponseAsync<DeliveryStaffRegisterDTO>(HttpResponse);
        }

        public async Task<DeliveryStaffDTO> DeleteDeliveryStaffAsync(long DeliveryStaffId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpResponseMessage response = await _client.DeleteAsync($"DeliveryStaff/{DeliveryStaffId}");

            return await _clientHelper.ParseResponseAsync<DeliveryStaffDTO>(response);
        }

        public async Task<IEnumerable<DeliveryStaffDTO>> GetAllDeliveryStaffsAsync(PagingParameters paging)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent httpContent = _clientHelper.CreateHttpContent(paging);
            var HttpResponse = await _client.PostAsync("DeliveryStaff/GetAll", httpContent);

            return await _clientHelper.ParseResponseAsync<IEnumerable<DeliveryStaffDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<DeliveryStaffDTO>> GetAllDeliveryStaffsAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.PostAsync("DeliveryStaff/GetAll", null);

            return await _clientHelper.ParseResponseAsync<IEnumerable<DeliveryStaffDTO>>(HttpResponse);
        }

        public async Task<DeliveryStaffDTO> GetDeliveryStaffByIdAsync(long DeliveryStaffId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("DeliveryStaff/" + DeliveryStaffId);

            return await _clientHelper.ParseResponseAsync<DeliveryStaffDTO>(HttpResponse);
        }

        public async Task<DeliveryStaffRegisterDTO> UpdateDeliveryStaffAsync(ServiceAndDeliveryStaffRegisterDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("ServiceAndDeliveryStaffAccount/Update", content);

            return await _clientHelper.ParseResponseAsync<DeliveryStaffRegisterDTO>(HttpResponse);
        }
        public async Task<DeliveryStaffDTO> ToggleActiveStatus(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            //HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.GetAsync($"DeliveryStaff/ToggleStatus/{Id}");

            return await _clientHelper.ParseResponseAsync<DeliveryStaffDTO>(HttpResponse);
        }
    }
}
