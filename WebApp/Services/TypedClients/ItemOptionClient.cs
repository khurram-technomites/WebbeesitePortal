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
    public class ItemOptionClient : IItemOptionClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ItemOptionClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public ItemOptionClient(HttpClient client, ILogger<ItemOptionClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<ItemOptionDTO> AddItemOptionAsync(ItemOptionDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("ItemOption", content);

            return await _clientHelper.ParseResponseAsync<ItemOptionDTO>(HttpResponse);
        }

        public async Task DeleteItemOptionAsync(long ItemOptionId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"ItemOption/{ItemOptionId}");
        }

        public async Task<IEnumerable<ItemOptionDTO>> GetMainPrice(long ItemId, long ItemOptionId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("ItemOption/GetMainPrice/Items/" + ItemId + "/Option/" + ItemOptionId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<ItemOptionDTO>>(HttpResponse);
        }


        public async Task Delete(long ItemOptionId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"ItemOption/Delete/{ItemOptionId}");
        }

        public async Task<IEnumerable<ItemOptionDTO>> GetAllItemOptionsAsync(long ItemId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("ItemOption/GetAll/Items/" + ItemId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<ItemOptionDTO>>(HttpResponse);
        }


        public async Task<ItemOptionDTO> GetItemOptionByIdAsync(long ItemOptionId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("ItemOption/" + ItemOptionId);

            return await _clientHelper.ParseResponseAsync<ItemOptionDTO>(HttpResponse);
        }

        public async Task<ItemOptionDTO> GetByName(long ItemId, string Name)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("ItemOption/GetByName/" + Name + "/Items/" + ItemId);

            return await _clientHelper.ParseResponseAsync<ItemOptionDTO>(HttpResponse);
        }


        public async Task<ItemOptionDTO> UpdateItemOptionAsync(ItemOptionDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("ItemOption", content);

            return await _clientHelper.ParseResponseAsync<ItemOptionDTO>(HttpResponse);
        }

    }
}
