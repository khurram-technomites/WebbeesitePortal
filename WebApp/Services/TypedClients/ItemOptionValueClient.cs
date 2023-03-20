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
    public class ItemOptionValueClient : IItemOptionValueClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ItemOptionValueClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public ItemOptionValueClient(HttpClient client, ILogger<ItemOptionValueClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<ItemOptionValueDTO> AddItemOptionValueAsync(ItemOptionValueDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("ItemOptionValue", content);

            return await _clientHelper.ParseResponseAsync<ItemOptionValueDTO>(HttpResponse);
        }

        public async Task DeleteItemOptionValueAsync(long ItemOptionValueId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"ItemOptionValue/{ItemOptionValueId}");
        }

        public async Task<IEnumerable<ItemOptionValueDTO>> GetAllItemOptionValuesAsync(long ItemOptionId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("ItemOptionValue/GetAll/ItemOptions/" + ItemOptionId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<ItemOptionValueDTO>>(HttpResponse);
        }



        public async Task<ItemOptionValueDTO> GetItemOptionValueByIdAsync(long ItemOptionValueId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("ItemOptionValue/" + ItemOptionValueId);

            return await _clientHelper.ParseResponseAsync<ItemOptionValueDTO>(HttpResponse);
        }

        public async Task<ItemOptionValueDTO> GetByNameAsync(long ItemOptionId , string Name )
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("ItemOptionValue/GetByName/" + Name + "/ItemOptions/" + ItemOptionId);

            return await _clientHelper.ParseResponseAsync<ItemOptionValueDTO>(HttpResponse);
        }

        public async Task<ItemOptionValueDTO> UpdateItemOptionValueAsync(ItemOptionValueDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("ItemOptionValue", content);

            return await _clientHelper.ParseResponseAsync<ItemOptionValueDTO>(HttpResponse);
        }

    }
}
