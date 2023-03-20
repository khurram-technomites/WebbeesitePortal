using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces.TypedClients;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using HelperClasses.DTOs;
namespace WebApp.Services.TypedClients
{
    public class ModuleClient:IModuleClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ModuleClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public ModuleClient(HttpClient client, ILogger<ModuleClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }
        public async Task<IEnumerable<ModuleDTO>> GetAllAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Module");

            return await _clientHelper.ParseResponseAsync<IEnumerable<ModuleDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<ModuleDTO>> GetModuleById(long Id)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Module/" + Id);

            return await _clientHelper.ParseResponseAsync<IEnumerable<ModuleDTO>>(HttpResponse);
        }

       


        public async Task<ModuleDTO> AddModuleAsync(ModuleDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Module", content);

            return await _clientHelper.ParseResponseAsync<ModuleDTO>(HttpResponse);
        }

        public async Task<ModuleDTO> UpdateModuleAsync(ModuleDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Module/", content);

            return await _clientHelper.ParseResponseAsync<ModuleDTO>(HttpResponse);
        }
        public async Task<ModuleDTO> ToggleActiveStatus(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            //HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.GetAsync($"Module/ToggleStatus/{Id}");

            return await _clientHelper.ParseResponseAsync<ModuleDTO>(HttpResponse);
        }
        public async Task DeleteModuleAsync(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Module/{Id}");
        }
    }
}
