using HelperClasses.DTOs.Restaurant;
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
    public class RestaurantBranchScheduleClient : IRestaurantBranchScheduleClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<RestaurantBranchScheduleClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public RestaurantBranchScheduleClient(HttpClient client, ILogger<RestaurantBranchScheduleClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<RestaurantBranchScheduleDTO> AddRestaurantBranchScheduleAsync(RestaurantBranchScheduleDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("RestaurantBranchSchedule", content);

            return await _clientHelper.ParseResponseAsync<RestaurantBranchScheduleDTO>(HttpResponse);
        }

        public async Task DeleteRestaurantBranchScheduleAsync(long RestaurantBranchScheduleId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"RestaurantBranchSchedule/{RestaurantBranchScheduleId}");
        }

        public async Task<IEnumerable<RestaurantBranchScheduleDTO>> GetAllRestaurantBranchSchedulesAsync(long BranchId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("RestaurantBranchSchedule/GetAll/RestaurantBranches/" + BranchId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantBranchScheduleDTO>>(HttpResponse);
        }



        public async Task<RestaurantBranchScheduleDTO> GetRestaurantBranchScheduleByIdAsync(long id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("RestaurantBranchSchedule/" + id);

            return await _clientHelper.ParseResponseAsync<RestaurantBranchScheduleDTO>(HttpResponse);
        }

        public async Task<RestaurantBranchScheduleDTO> UpdateRestaurantBranchScheduleAsync(RestaurantBranchScheduleDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("RestaurantBranchSchedule", content);

            return await _clientHelper.ParseResponseAsync<RestaurantBranchScheduleDTO>(HttpResponse);
        }
    }
}
