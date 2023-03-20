using HelperClasses.DTOs.Menu;
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
    public class MenuItemClient : IMenuItemClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<MenuItemClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public MenuItemClient(HttpClient client, ILogger<MenuItemClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<MenuItemDTO> AddMenuItemAsync(MenuItemDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("MenuItem", content);

            return await _clientHelper.ParseResponseAsync<MenuItemDTO>(HttpResponse);
        }

        public async Task<MenuItemDTO> SaveCategoryPosition(MenuItemDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("MenuItem/SaveCategoryPositions", content);

            return await _clientHelper.ParseResponseAsync<MenuItemDTO>(HttpResponse);
        }

        public async Task<MenuItemDTO> SavePosition(MenuItemDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("MenuItem/SavePositions", content);

            return await _clientHelper.ParseResponseAsync<MenuItemDTO>(HttpResponse);
        }

        public async Task DeleteMenuItemAsync(long MenuItemId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"MenuItem/{MenuItemId}");
        }

        public async Task<IEnumerable<MenuItemDTO>> GetAllMenuItemsAsync(long MenuId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("MenuItem/GetAll/Menu/" + MenuId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<MenuItemDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<MenuItemByMenuDTO>> GetAllMenuItemsByMenuAsync(long MenuId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"MenuItem/GetAll/Menu/{MenuId}/Categories");

            return await _clientHelper.ParseResponseAsync<IEnumerable<MenuItemByMenuDTO>>(HttpResponse);
        }

        public async Task<bool> CheckMainPrice(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse =  await _client.GetAsync($"MenuItem/CheckMainPrice/{Id}");

            return await _clientHelper.ParseResponseAsync<bool>(HttpResponse);
        }



        public async Task<MenuItemDTO> GetMenuItemByIdAsync(long MenuItemId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("MenuItem/" + MenuItemId);

            return await _clientHelper.ParseResponseAsync<MenuItemDTO>(HttpResponse);
        }


        public async Task<MenuItemDTO> UpdateMenuItemAsync(MenuItemDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("MenuItem", content);

            return await _clientHelper.ParseResponseAsync<MenuItemDTO>(HttpResponse);
        }

        public async Task<MenuItemDTO> ToggleActiveStatus(long MenuItemId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"MenuItem/ToggleStatus/{MenuItemId}");

            return await _clientHelper.ParseResponseAsync<MenuItemDTO>(HttpResponse);
        }
    }
}
