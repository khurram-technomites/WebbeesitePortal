using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HelperClasses.DTOs.SparePartCMS;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class SparePartTestimonialClient : ISparePartTestimonialClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartTestimonialClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SparePartTestimonialClient(HttpClient client, ILogger<SparePartTestimonialClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<SparePartTestimonialDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/Testimonials");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartTestimonialDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartTestimonialDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/Testimonials/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartTestimonialDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartTestimonialDTO>> GetAllBySparePartDealerIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/{Id}/Testimonials/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartTestimonialDTO>>(HttpResponse);
        }


        public async Task<SparePartTestimonialDTO> AddSparePartTestimonialAsync(SparePartTestimonialDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SpareParts/Testimonials", content);

            return await _clientHelper.ParseResponseAsync<SparePartTestimonialDTO>(HttpResponse);
        }

        public async Task<SparePartTestimonialDTO> UpdateGSparePartTestimonialAsync(SparePartTestimonialDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("SpareParts/Testimonials", content);

            return await _clientHelper.ParseResponseAsync<SparePartTestimonialDTO>(HttpResponse);
        }
        public async Task<SparePartTestimonialDTO> ToggleActiveStatus(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/Testimonials/{Id}/ToggleStatus");

            return await _clientHelper.ParseResponseAsync<SparePartTestimonialDTO>(HttpResponse);
        }
        public async Task DeleteSparePartTestimonialAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SpareParts/Testimonials/{Id}");
        }
    }
}
