using HelperClasses.DTOs.Garage;
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
using WebApp.ViewModels;

namespace WebApp.Services.TypedClients
{
    public class GarageScheduleClient : IGarageScheduleClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageScheduleClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;

        public GarageScheduleClient(ITokenManager tokenManager, IHttpClientHelper clientHelper, ILogger<GarageScheduleClient> logger, HttpClient client)
        {
            _tokenManager = tokenManager;
            _clientHelper = clientHelper;
            _logger = logger;
            _client = client;
        }
        public async Task<IEnumerable<GarageScheduleViewModel>> AddandUpdateGarageSchedule(List<GarageScheduleDTO> Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PutAsync("Garage/Schedule", content);

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageScheduleViewModel>>(HttpResponse);
        }

        public async Task Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/Schedule/{Id}");
        }

        public async Task<IEnumerable<GarageScheduleViewModel>> GetAllByGarage(long GarageId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{GarageId}/Schedule");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageScheduleViewModel>>(HttpResponse);
        }
    }
}
