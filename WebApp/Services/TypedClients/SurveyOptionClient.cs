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
    public class SurveyOptionClient : ISurveyOptionClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SurveyOptionClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public SurveyOptionClient(HttpClient client, ILogger<SurveyOptionClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<SurveyOptionDTO> AddSurveyOptionAsync(SurveyOptionDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SurveyOption", content);

            return await _clientHelper.ParseResponseAsync<SurveyOptionDTO>(HttpResponse);
        }

        public async Task DeleteSurveyOptionAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SurveyOption/{Id}");
        }

        public async Task<IEnumerable<SurveyOptionDTO>> GetAllBySurveyIdAsync(long SurveyId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SurveyOption/GetAll/Surveys/" + SurveyId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SurveyOptionDTO>>(HttpResponse);
        }


        public async Task<SurveyOptionDTO> UpdateSurveyOptionAsync(SurveyOptionDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("SurveyOption", content);

            return await _clientHelper.ParseResponseAsync<SurveyOptionDTO>(HttpResponse);
        }

        public async Task<SurveyOptionDTO> GetSurveyOptionByIdAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SurveyOption/" + Id);

            return await _clientHelper.ParseResponseAsync<SurveyOptionDTO>(HttpResponse);
        }
    }
}
