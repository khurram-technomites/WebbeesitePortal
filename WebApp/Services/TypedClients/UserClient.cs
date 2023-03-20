using HelperClasses.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class UserClient : IUserClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<UserClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public UserClient(HttpClient client, ILogger<UserClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<UserDTO> AddUserAsync(UserDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("User", content);

            return await _clientHelper.ParseResponseAsync<UserDTO>(HttpResponse);
        }

        public async Task DeleteUserAsync(string UserId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"User/{UserId}");
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("User");

            return await _clientHelper.ParseResponseAsync<IEnumerable<UserDTO>>(HttpResponse);
        }

        public async Task<UserDTO> GetUserByIdAsync(string UserId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"User/{UserId}");

            return await _clientHelper.ParseResponseAsync<UserDTO>(HttpResponse);
        }

        public async Task<bool> ChangePasswordAsync(string UserId , string OldPassword , string NewPassword)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            var HttpResponse = await _client.PutAsync($"User/ChangePassword/{UserId}/{OldPassword}/{NewPassword}" , null);

            if (HttpResponse.IsSuccessStatusCode)
            {
                return true;
            }
                return false;
          
        }
        public async Task<int> GetTotalRecordsOfUsers()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("User/TotalRecords");

            return await _clientHelper.ParseResponseAsync<int>(HttpResponse);
        }

        public async Task<UserDTO> UpdateUserAsync(UserDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("User", content);

            return await _clientHelper.ParseResponseAsync<UserDTO>(HttpResponse);
        }

        public async Task<UserDTO> ToggleActiveStatus(string UserId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            //HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync($"User/ToggleStatus/{UserId}", null);

            return await _clientHelper.ParseResponseAsync<UserDTO>(HttpResponse);
        }
    }
}
