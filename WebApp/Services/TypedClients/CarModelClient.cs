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
    public class CarModelClient : ICarModelClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<CarModelClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public CarModelClient(HttpClient client, ILogger<CarModelClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<CarModelDTO> AddCarModelAsync(CarModelDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("CarModel", content);

            return await _clientHelper.ParseResponseAsync<CarModelDTO>(HttpResponse);
        }

        public async Task DeleteCarModelAsync(long CarModelId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"CarModel/{CarModelId}");
        }

        public async Task<IEnumerable<CarModelDTO>> GetAllCarModelsAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            
            var HttpResponse = await _client.GetAsync("CarModel");

            return await _clientHelper.ParseResponseAsync<IEnumerable<CarModelDTO>>(HttpResponse);
        }

        public async Task<CarModelDTO> GetCarModelByIdAsync(long CarModelId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("CarModel/" + CarModelId);

            return await _clientHelper.ParseResponseAsync<CarModelDTO>(HttpResponse);
        }
        public async Task<CarModelDTO> GetCarModelByMakeIdAsync(long CarMakeId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("ByMake/" + CarMakeId);

            return await _clientHelper.ParseResponseAsync<CarModelDTO>(HttpResponse);
        }

        //public async Task<int> GetTotalRecordsOfCarModels()
        //{
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

        //    var HttpResponse = await _client.GetAsync("CarModel/TotalRecords");

        //    return await _clientHelper.ParseResponseAsync<int>(HttpResponse);
        //}

        public async Task<CarModelDTO> UpdateCarModelAsync(CarModelDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("CarModel", content);

            return await _clientHelper.ParseResponseAsync<CarModelDTO>(HttpResponse);
        }

        public async Task<CarModelDTO> ToggleActiveStatus(long CarModelId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"CarModel/ToggleStatus/{CarModelId}");

            return await _clientHelper.ParseResponseAsync<CarModelDTO>(HttpResponse);
        }
    }
}
