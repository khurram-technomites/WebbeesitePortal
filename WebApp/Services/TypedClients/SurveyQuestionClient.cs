using HelperClasses.DTOs.Survey;
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
    public class SurveyQuestionClient : ISurveyQuestionClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SurveyQuestionClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public SurveyQuestionClient(HttpClient client, ILogger<SurveyQuestionClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<SurveyQuestionDTO> AddSurveyQuestionAsync(SurveyQuestionDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SurveyQuestion", content);

            return await _clientHelper.ParseResponseAsync<SurveyQuestionDTO>(HttpResponse);
        }

        public async Task DeleteSurveyQuestionAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SurveyQuestion/{Id}");
        }

        public async Task<IEnumerable<SurveyQuestionDTO>> GetAllSurveyQuestionBySurveyAsync(long SurveyID)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SurveyQuestion/GetAll/Surveys/" + SurveyID);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SurveyQuestionDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SurveyQuestionDTO>> GetAllSurveyQuestionAsync(long RestaurantId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SurveyQuestion/GetAll/Restaurant/" + RestaurantId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SurveyQuestionDTO>>(HttpResponse);
        }

        public async Task<SurveyQuestionDTO> UpdateSurveyQuestionAsync(SurveyQuestionDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("SurveyQuestion", content);

            return await _clientHelper.ParseResponseAsync<SurveyQuestionDTO>(HttpResponse);
        }
        public async Task<SurveyQuestionDTO> ToggleActiveStatus(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SurveyQuestion/ToggleStatus/{Id}");

            return await _clientHelper.ParseResponseAsync<SurveyQuestionDTO>(HttpResponse);
        }
        public async Task<SurveyQuestionDTO> GetSurveyQuestionByIdAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SurveyQuestion/" + Id);

            return await _clientHelper.ParseResponseAsync<SurveyQuestionDTO>(HttpResponse);
        }
    }
}
