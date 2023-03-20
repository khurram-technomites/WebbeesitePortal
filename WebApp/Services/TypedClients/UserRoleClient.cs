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
    public class UserRoleClient : IUserRoleClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<UserRoleClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public UserRoleClient(HttpClient client, ILogger<UserRoleClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<IdentityUserRoleDTO> Create(string RoleName)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.PostAsync($"UserRole/{RoleName}", null);

            return await _clientHelper.ParseResponseAsync<IdentityUserRoleDTO>(HttpResponse);
        }

        public async Task<IdentityUserRoleDTO> Delete(string RoleId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"UserRole/{RoleId}");

            return await _clientHelper.ParseResponseAsync<IdentityUserRoleDTO>(response);
        }

        public async Task<IdentityUserRoleDTO> Edit(string RoleId , string RoleName)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            /*HttpContent content = _clientHelper.CreateHttpContent(RoleId);*/
            var HttpResponse = await _client.PutAsync($"UserRole/{RoleId}/{RoleName}", null);

            return await _clientHelper.ParseResponseAsync<IdentityUserRoleDTO>(HttpResponse);
        }

        public async Task<IEnumerable<IdentityUserRoleDTO>> GetUserRoles()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("UserRole");

            return await _clientHelper.ParseResponseAsync<IEnumerable<IdentityUserRoleDTO>>(HttpResponse);
        }

        public async Task<IdentityUserRoleDTO> GetUserRoleByRoleId(String RoleId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"UserRole/{RoleId}");

            return await _clientHelper.ParseResponseAsync<IdentityUserRoleDTO>(HttpResponse);
        }
    }
}
