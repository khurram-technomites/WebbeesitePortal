using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.SparePartsDealer;
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
    public class SparePartsDealerScheduleClient : ISparePartsDealerScheduleClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartsDealerScheduleClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SparePartsDealerScheduleClient(HttpClient client, ILogger<SparePartsDealerScheduleClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<SparePartsDealerScheduleDTO> AddSparePartsDealerScheduleAsync(SparePartsDealerScheduleDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SparePartsDealerSchedule", content);

            return await _clientHelper.ParseResponseAsync<SparePartsDealerScheduleDTO>(HttpResponse);
        }

        public async Task DeleteSparePartsDealerScheduleAsync(long scheduleId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SparePartsDealerSchedule/{scheduleId}");
        }

        public async Task<IEnumerable<SparePartsDealerScheduleDTO>> GetAllSparePartsDealerSchedulesAsync(long sparePartsDealerId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SparePartsDealerSchedule/GetAll/SparePartsDealer/" + sparePartsDealerId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartsDealerScheduleDTO>>(HttpResponse);
        }



        public async Task<SparePartsDealerScheduleDTO> GetSparePartsDealerScheduleByIdAsync(long id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SparePartsDealerSchedule/" + id);

            return await _clientHelper.ParseResponseAsync<SparePartsDealerScheduleDTO>(HttpResponse);
        }

        public async Task<SparePartsDealerScheduleDTO> UpdateSparePartsDealerScheduleAsync(SparePartsDealerScheduleDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("SparePartsDealerSchedule", content);

            return await _clientHelper.ParseResponseAsync<SparePartsDealerScheduleDTO>(HttpResponse);
        }
    }
}
