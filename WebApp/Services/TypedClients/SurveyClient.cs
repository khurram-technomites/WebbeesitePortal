using HelperClasses.DTOs.Survey;
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

namespace WebApp.Services.TypedClients
{
    public class SurveyClient : ISurveyClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SurveyClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public SurveyClient(HttpClient client, ILogger<SurveyClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<SurveyDTO> AddSurveyAsync(SurveyDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Survey", content);

            return await _clientHelper.ParseResponseJsonAsync<SurveyDTO>(HttpResponse);
        }

        public async Task DeleteSurveyAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Survey/{Id}");
        }

        public async Task<IEnumerable<SurveyDTO>> GetAllSurveyByRestaurantAsync(long RestaurantId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Survey/GetAll/Restaurants/" + RestaurantId);

            return await _clientHelper.ParseResponseJsonAsync<IEnumerable<SurveyDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SurveyDTO>> GetAllSurveyByBranchAsync(long RestaurantId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Survey/GetAll/RestaurantBranches/" + RestaurantId);

            return await _clientHelper.ParseResponseJsonAsync<IEnumerable<SurveyDTO>>(HttpResponse);
        }

        public async Task<SurveyDTO> GetSurveyByIdAsync(long SurveyId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Survey/" + SurveyId);

            return await _clientHelper.ParseResponseJsonAsync<SurveyDTO>(HttpResponse);
        }

        public async Task<SurveyDTO> ToggleActiveStatus(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Survey/ToggleStatus/{Id}");

            return await _clientHelper.ParseResponseAsync<SurveyDTO>(HttpResponse);
        }
        public async Task<SurveyDTO> UpdateSurveyAsync(SurveyDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Survey", content);

            return await _clientHelper.ParseResponseJsonAsync<SurveyDTO>(HttpResponse);
        }

    }
}
