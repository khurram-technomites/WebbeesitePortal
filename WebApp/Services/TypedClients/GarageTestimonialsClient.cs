using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebAPI.Interfaces.IServices.Domains;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces.TypedClients;
using HelperClasses.DTOs.Restaurant;

namespace WebApp.Services.TypedClients
{
    public class GarageTestimonialsClient: IGarageTestimonialsClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<GarageTestimonialsClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public GarageTestimonialsClient(HttpClient client, ILogger<GarageTestimonialsClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<IEnumerable<GarageTestimonialsDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/Testimonials");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageTestimonialsDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageTestimonialsDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Garage/Testimonials/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageTestimonialsDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<GarageTestimonialsDTO>> GetAllByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/{Id}/Testimonials/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<GarageTestimonialsDTO>>(HttpResponse);
        }
        public async Task<long> GetAllCountByGarageIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/Count/{Id}/Testimonials/");

            return await _clientHelper.ParseResponseAsync<long>(HttpResponse);
        }

        public async Task<GarageTestimonialsDTO> AddGarageTestimonialsAsync(GarageTestimonialsDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Garage/Testimonials", content);

            return await _clientHelper.ParseResponseAsync<GarageTestimonialsDTO>(HttpResponse);
        }

        public async Task<GarageTestimonialsDTO> UpdateGarageTestimonialsAsync(GarageTestimonialsDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Garage/Testimonials", content);

            return await _clientHelper.ParseResponseAsync<GarageTestimonialsDTO>(HttpResponse);
        }
        public async Task<GarageTestimonialsDTO> ToggleActiveStatus(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Garage/Testimonials/{Id}/ToggleStatus");

            return await _clientHelper.ParseResponseAsync<GarageTestimonialsDTO>(HttpResponse);
        }
        public async Task DeleteGarageTestimonialsAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Garage/Testimonials/{Id}");
        }
    }
}
