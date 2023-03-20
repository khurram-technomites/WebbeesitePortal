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
    public class CouponCategoryClient : ICouponCategoryClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<CouponCategoryClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public CouponCategoryClient(HttpClient client, ILogger<CouponCategoryClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<CouponCategoryDTO> AddCouponCategoryAsync(CouponCategoryDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("CouponCategory", content);

            return await _clientHelper.ParseResponseAsync<CouponCategoryDTO>(HttpResponse);
        }

        public async Task DeleteCouponCategoryAsync(long CouponCategoryId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"CouponCategory/{CouponCategoryId}");
        }

        public async Task<IEnumerable<CouponCategoryDTO>> GetCouponCategoriesByCoupon(long CouponId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("CouponCategory/Coupons/" + CouponId);
            return await _clientHelper.ParseResponseAsync<IEnumerable<CouponCategoryDTO>>(HttpResponse);
        }


        public async Task<IEnumerable<CouponCategoryDTO>> GetAllCouponCategories()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("CouponCategory/GetAll");

            return await _clientHelper.ParseResponseAsync<IEnumerable<CouponCategoryDTO>>(HttpResponse);
        }


        public async Task<CouponCategoryDTO> GetCouponCategoryById(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("CouponCategory/" + Id);

            return await _clientHelper.ParseResponseAsync<CouponCategoryDTO>(HttpResponse);
        }

        public async Task<CouponCategoryDTO> GetCouponCategoryByCouponAndCategory(long CouponId , long CategoryId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("CouponCategory/Coupons/" + CouponId + "/Categories/" + CategoryId );

            return await _clientHelper.ParseResponseAsync<CouponCategoryDTO>(HttpResponse);
        }


        public async Task<CouponCategoryDTO> UpdateCouponCategoryAsync(CouponCategoryDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("CouponCategory", content);

            return await _clientHelper.ParseResponseAsync<CouponCategoryDTO>(HttpResponse);
        }

        public async Task<CouponCategoryDTO> ToggleActiveStatus(long CouponCategoryId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"CouponCategory/ToggleStatus/{CouponCategoryId}");

            return await _clientHelper.ParseResponseAsync<CouponCategoryDTO>(HttpResponse);
        }
    }
}
