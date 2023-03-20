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
    public class MenuClient : IMenuClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<MenuClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public MenuClient(HttpClient client, ILogger<MenuClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<MenuDTO> AddMenuAsync(MenuDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Menu", content);

            return await _clientHelper.ParseResponseAsync<MenuDTO>(HttpResponse);
        }

        public async Task<MenuDTO> SavePosition(MenuDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Menu/SavePositions", content);

            return await _clientHelper.ParseResponseAsync<MenuDTO>(HttpResponse);
        }

        public async Task DeleteMenuAsync(long MenuId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Menu/{MenuId}");
        }

        public async Task<IEnumerable<MenuDTO>> GetAllMenusAsync(long RestaurantId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Menu/GetAll/" + RestaurantId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<MenuDTO>>(HttpResponse);
        }

        public async Task<MenuDTO> GetGeneralMenu()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Menu/General");

            return await _clientHelper.ParseResponseAsync<MenuDTO>(HttpResponse);
        }

        public async Task<IEnumerable<MenuDTO>> GetAllMenuByBranchIdAsync(long BranchId , long id = 0)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Menu/GetAll/RestaurantBranches/" + BranchId + "/" + id );

            return await _clientHelper.ParseResponseAsync<IEnumerable<MenuDTO>>(HttpResponse);
        }



        public async Task<MenuDTO> GetMenuByIdAsync(long MenuId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Menu/" + MenuId);

            return await _clientHelper.ParseResponseAsync<MenuDTO>(HttpResponse);
        }


        public async Task<MenuDTO> UpdateMenuAsync(MenuDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Menu", content);

            return await _clientHelper.ParseResponseAsync<MenuDTO>(HttpResponse);
        }

        public async Task<MenuDTO> ToggleActiveStatus(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Menu/ToggleStatus/{Id}");

            return await _clientHelper.ParseResponseAsync<MenuDTO>(HttpResponse);
        }

    }
}
