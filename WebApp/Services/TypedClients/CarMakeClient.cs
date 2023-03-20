using HelperClasses.DTOs;
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
    public class CarMakeClient : ICarMakeClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<CarMakeClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public CarMakeClient(HttpClient client, ILogger<CarMakeClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<CarMakeDTO> AddCarMakeAsync(CarMakeDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("CarMake", content);

            return await _clientHelper.ParseResponseAsync<CarMakeDTO>(HttpResponse);
        }

        public async Task DeleteCarMakeAsync(long CarMakeId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"CarMake/{CarMakeId}");
        }

        public async Task<IEnumerable<CarMakeDTO>> GetAllCarMakesAsync()
        {       
            var HttpResponse = await _client.GetAsync("CarMake");

            return await _clientHelper.ParseResponseAsync<IEnumerable<CarMakeDTO>>(HttpResponse);
        }

        public async Task<CarMakeDTO> GetCarMakeByIdAsync(long CarMakeId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("CarMake/" + CarMakeId);

            return await _clientHelper.ParseResponseAsync<CarMakeDTO>(HttpResponse);
        }

        //public async Task<int> GetTotalRecordsOfCarMakes()
        //{
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

        //    var HttpResponse = await _client.GetAsync("CarMake/TotalRecords");

        //    return await _clientHelper.ParseResponseAsync<int>(HttpResponse);
        //}

        public async Task<CarMakeDTO> UpdateCarMakeAsync(CarMakeDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("CarMake", content);

            return await _clientHelper.ParseResponseAsync<CarMakeDTO>(HttpResponse);
        }

        public async Task<CarMakeDTO> ToggleActiveStatus(long CarMakeId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"CarMake/ToggleStatus/{CarMakeId}");

            return await _clientHelper.ParseResponseAsync<CarMakeDTO>(HttpResponse);
        }
    }
}
