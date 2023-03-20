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
    public class MenuItemOptionClient : IMenuItemOptionClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<MenuItemOptionClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public MenuItemOptionClient(HttpClient client, ILogger<MenuItemOptionClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<MenuItemOptionDTO> AddMenuItemOptionAsync(MenuItemOptionDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("MenuItemOption", content);

            return await _clientHelper.ParseResponseAsync<MenuItemOptionDTO>(HttpResponse);
        }

        public async Task DeleteMenuItemOptionAsync(long MenuItemOptionId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"MenuItemOption/{MenuItemOptionId}");
        }

        public async Task Delete(long MenuItemOptionId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"MenuItemOption/Delete/{MenuItemOptionId}");
        }

        public async Task<IEnumerable<MenuItemOptionDTO>> GetAllMenuItemOptionsAsync(long MenuItemId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("MenuItemOption/GetAll/MenuItems/" + MenuItemId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<MenuItemOptionDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<MenuItemOptionDTO>> GetMainPrice(long MenuItemId, long MenuItemOptionId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("MenuItemOption/GetMainPrice/MenuItems/" + MenuItemId + "/Option/" + MenuItemOptionId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<MenuItemOptionDTO>>(HttpResponse);
        }


        public async Task<MenuItemOptionDTO> GetMenuItemOptionByIdAsync(long MenuItemOptionId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("MenuItemOption/" + MenuItemOptionId);

            return await _clientHelper.ParseResponseAsync<MenuItemOptionDTO>(HttpResponse);
        }


        public async Task<MenuItemOptionDTO> UpdateMenuItemOptionAsync(MenuItemOptionDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("MenuItemOption", content);

            return await _clientHelper.ParseResponseAsync<MenuItemOptionDTO>(HttpResponse);
        }

    }
}
