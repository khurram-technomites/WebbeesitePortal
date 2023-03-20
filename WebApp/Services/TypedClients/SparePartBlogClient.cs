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
    public class SparePartBlogClient : ISparePartBlogClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartBlogClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public SparePartBlogClient(HttpClient client, ILogger<SparePartBlogClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<IEnumerable<SparePartBlogDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/Blog");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartBlogDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartBlogDTO>> GetAllByIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SpareParts/Blog/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartBlogDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartBlogDTO>> GetAllBySparePartDealerIdAsync(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/{Id}/Blog/");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartBlogDTO>>(HttpResponse);
        }


        public async Task<SparePartBlogDTO> AddSparePartBlogAsync(SparePartBlogDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SpareParts/Blog", content);

            return await _clientHelper.ParseResponseAsync<SparePartBlogDTO>(HttpResponse);
        }

        public async Task<SparePartBlogDTO> UpdateSparePartBlogAsync(SparePartBlogDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("SpareParts/Blog", content);

            return await _clientHelper.ParseResponseAsync<SparePartBlogDTO>(HttpResponse);
        }

        public async Task DeleteSparePartBlogAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SpareParts/Blog/{Id}");
        }

        public async Task<SparePartBlogDTO> ToggleStatus(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SpareParts/Blog/ToggleStatus/{Id}");

            return await _clientHelper.ParseResponseAsync<SparePartBlogDTO>(HttpResponse);
        }
    }
}
