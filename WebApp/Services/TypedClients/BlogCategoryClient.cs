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
    public class BlogCategoryClient : IBlogCategoryClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<BlogCategoryClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;

        public BlogCategoryClient(ITokenManager tokenManager, IHttpClientHelper clientHelper, ILogger<BlogCategoryClient> logger, HttpClient client)
        {
            _tokenManager = tokenManager;
            _clientHelper = clientHelper;
            _logger = logger;
            _client = client;
        }

        public async Task<BlogCategoryDTO> AddBlogCategory(BlogCategoryDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PostAsync("Blog/Category", content);

            return await _clientHelper.ParseResponseAsync<BlogCategoryDTO>(HttpResponse);
        }

        public async Task<BlogCategoryDTO> ArchiveBlogCategory(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.DeleteAsync($"Blog/Category/{Id}");

            return await _clientHelper.ParseResponseAsync<BlogCategoryDTO>(HttpResponse);
        }

        public async Task<IEnumerable<BlogCategoryDTO>> GetAllBlogCategories()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Blog/Category");

            return await _clientHelper.ParseResponseAsync<IEnumerable<BlogCategoryDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<BlogCategoryDTO>> GetBlogCategoriesById(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Blog/Category/{Id}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<BlogCategoryDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<BlogCategoryDTO>> GetAllByGarageIdAsync(long GarageId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Blog/Category/GarageId/{GarageId}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<BlogCategoryDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<BlogCategoryDTO>> GetBlogCategoriesByModule(string Module)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Blog/Category/Module/{Module}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<BlogCategoryDTO>>(HttpResponse);
        }

        public async Task<BlogCategoryDTO> UpdateBlogCategory(BlogCategoryDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PutAsync("Blog/Category", content);

            return await _clientHelper.ParseResponseAsync<BlogCategoryDTO>(HttpResponse);
        }
    }
}
