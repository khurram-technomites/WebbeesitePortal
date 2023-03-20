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
    public class MenuItemOptionValueClient : IMenuItemOptionValueClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<MenuItemOptionValueClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public MenuItemOptionValueClient(HttpClient client, ILogger<MenuItemOptionValueClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<MenuItemOptionValueDTO> AddMenuItemOptionValueAsync(MenuItemOptionValueDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("MenuItemOptionValues", content);

            return await _clientHelper.ParseResponseAsync<MenuItemOptionValueDTO>(HttpResponse);
        }

        public async Task DeleteMenuItemOptionValueAsync(long MenuItemOptionValueId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"MenuItemOptionValue/{MenuItemOptionValueId}");
        }

        public async Task<IEnumerable<MenuItemOptionValueDTO>> GetAllMenuItemOptionValuesAsync(long MenuItemOptionId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("MenuItemOptionValues/GetAll/MenuItemOptions/" + MenuItemOptionId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<MenuItemOptionValueDTO>>(HttpResponse);
        }



        public async Task<MenuItemOptionValueDTO> GetMenuItemOptionValueByIdAsync(long MenuItemOptionValueId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("MenuItemOptionValues/" + MenuItemOptionValueId);

            return await _clientHelper.ParseResponseAsync<MenuItemOptionValueDTO>(HttpResponse);
        }


        public async Task<MenuItemOptionValueDTO> UpdateMenuItemOptionValueAsync(MenuItemOptionValueDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("MenuItemOptionValues", content);

            return await _clientHelper.ParseResponseAsync<MenuItemOptionValueDTO>(HttpResponse);
        }

        public async Task Delete(long MenuItemOptionValueId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"MenuItemOptionValues/Delete/{MenuItemOptionValueId}");
        }

    }
}
