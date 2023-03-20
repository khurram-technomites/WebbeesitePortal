using HelperClasses.DTOs.Restaurant;
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
    public class CategoryClient : ICategoryClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<CategoryClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public CategoryClient(HttpClient client, ILogger<CategoryClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<CategoryDTO> AddCategoryAsync(CategoryDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Category", content);

            return await _clientHelper.ParseResponseAsync<CategoryDTO>(HttpResponse);
        }

        public async Task DeleteCategoryAsync(long CategoryId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Category/{CategoryId}");
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategorysAsync(long RestaurantId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Category/GetAll/" + RestaurantId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<CategoryDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllGeneralCategoriesAsync(long restaurantId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Category/GetAll/General/Restaurants/" + restaurantId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<CategoryDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesDropDownByMenuIdAsync(long MenuId, long RestaurantId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Category/GetAll/Menus/" + MenuId + "/Restaurants/" + RestaurantId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<CategoryDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<CategoryDTO>> GetParentCategoriesAsync(long RestaurantId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Category/ParentCategories/" + RestaurantId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<CategoryDTO>>(HttpResponse);
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(long CategoryId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Category/" + CategoryId);

            return await _clientHelper.ParseResponseAsync<CategoryDTO>(HttpResponse);
        }

        public async Task<CategoryDTO> GetByName(long restaurantId, string Name)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Category/GetByName/" + Name + "/Restaurants/" + restaurantId);

            return await _clientHelper.ParseResponseAsync<CategoryDTO>(HttpResponse);
        }

        public async Task<CategoryDTO> GetCategoryByParentIdAsync(long ParentId, long RestaurantId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Category/ByParent" + ParentId + "/" + RestaurantId);

            return await _clientHelper.ParseResponseAsync<CategoryDTO>(HttpResponse);
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(CategoryDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Category", content);

            return await _clientHelper.ParseResponseAsync<CategoryDTO>(HttpResponse);
        }

        public async Task<CategoryDTO> ToggleActiveStatus(long CategoryId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Category/ToggleStatus/{CategoryId}");

            return await _clientHelper.ParseResponseAsync<CategoryDTO>(HttpResponse);
        }

        public async Task<IEnumerable<CategoryDTO>> AddCategoryRangeAsync(IEnumerable<CategoryDTO> Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Category/BulkUpload", content);

            return await _clientHelper.ParseResponseAsync<IEnumerable<CategoryDTO>>(HttpResponse);
        }

        public async Task<long> MaxPosition(long RestaurantId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Category/MaxPosition/" + RestaurantId);

            return await _clientHelper.ParseResponseAsync<long>(HttpResponse);
        }
    }
}
