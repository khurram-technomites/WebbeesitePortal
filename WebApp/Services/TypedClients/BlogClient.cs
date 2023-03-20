using HelperClasses.DTOs;
using HelperClasses.DTOs.Blog;
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
    public class BlogClient : IBlogClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<BlogClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public BlogClient(HttpClient client, ILogger<BlogClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<BlogDTO> Create(BlogDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("Blog", content);

            return await _clientHelper.ParseResponseAsync<BlogDTO>(HttpResponse);
        }

        public async Task<BlogDTO> Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"Blog/{Id}");

            return await _clientHelper.ParseResponseAsync<BlogDTO>(response);
        }

        public async Task<BlogDTO> Edit(BlogDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("Blog", content);

            return await _clientHelper.ParseResponseAsync<BlogDTO>(HttpResponse);
        }

        public async Task<IEnumerable<BlogDTO>> GetBlogs()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Blog");

            return await _clientHelper.ParseResponseAsync<IEnumerable<BlogDTO>>(HttpResponse);
        }

        public async Task<BlogDTO> GetBlogByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Blog/{Id}");

            return await _clientHelper.ParseResponseAsync<BlogDTO>(HttpResponse);
        }
        public async Task<BlogDTO> ToggleActiveStatus(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            //HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.GetAsync($"Blog/ToggleStatus/{Id}");

            return await _clientHelper.ParseResponseAsync<BlogDTO>(HttpResponse);
        }
    }
}
