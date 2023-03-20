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
    public class ItemClient : IItemClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ItemClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public ItemClient(HttpClient client, ILogger<ItemClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<ItemDTO> AddItemAsync(ItemDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Item", content);

            return await _clientHelper.ParseResponseAsync<ItemDTO>(HttpResponse);
        }

        public async Task<IEnumerable<ItemDTO>> AddItemRangeAsync(IEnumerable<ItemDTO> Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Item/BulkUpload", content);

            return await _clientHelper.ParseResponseAsync<IEnumerable<ItemDTO>>(HttpResponse);
        }

        public async Task DeleteItemAsync(long ItemId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Item/{ItemId}");
        }

        public async Task<IEnumerable<ItemDTO>> GetAllItemsAsync(long RestaurantId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Item/GetAll/" + RestaurantId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<ItemDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<ItemDTO>> GetAllGeneralAsync(long restaurantId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Item/GetAll/General/Restaurants/" + restaurantId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<ItemDTO>>(HttpResponse);
        }
        public async Task<ItemDTO> GetItemByIdAsync(long ItemId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Item/" + ItemId);

            return await _clientHelper.ParseResponseAsync<ItemDTO>(HttpResponse);
        }

        public async Task<ItemDTO> GetByNameAsync(long RestaurantId , string Name)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Item/GetByName/Restaurants/{RestaurantId}?Name={Name}");

            return await _clientHelper.ParseResponseAsync<ItemDTO>(HttpResponse);
        }

        public async Task<ItemDTO> GetItemByCategoryIdAsync(long CategoryId, long RestaurantId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Item/ByCategory" + CategoryId + "/" + RestaurantId);

            return await _clientHelper.ParseResponseAsync<ItemDTO>(HttpResponse);
        }

        public async Task<IEnumerable<ItemDTO>> GetItemsByCategoryIdAsync(long CategoryId, long RestaurantId , long MenuId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Item/Category/" + CategoryId + "/Restaurants/" + RestaurantId + "/Menus/" + MenuId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<ItemDTO>>(HttpResponse);
        }

        public async Task<ItemDTO> UpdateItemAsync(ItemDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Item", content);

            return await _clientHelper.ParseResponseAsync<ItemDTO>(HttpResponse);
        }

        public async Task<ItemDTO> ToggleActiveStatus(long ItemId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Item/ToggleStatus/{ItemId}");

            return await _clientHelper.ParseResponseAsync<ItemDTO>(HttpResponse);
        }
    }
}
