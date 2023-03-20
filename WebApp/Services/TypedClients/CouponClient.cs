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
    public class CouponClient : ICouponClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<CouponClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public CouponClient(HttpClient client, ILogger<CouponClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<CouponDTO> AddCouponAsync(CouponDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Coupon", content);

            return await _clientHelper.ParseResponseAsync<CouponDTO>(HttpResponse);
        }

        public async Task<CouponDTO> DeleteCouponAsync(long CouponId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            return await _clientHelper.ParseResponseAsync <CouponDTO>(await _client.DeleteAsync($"Coupon/{CouponId}"));
        }

        public async Task<IEnumerable<CouponDTO>> GetAllCouponsAsync(long RestaurantId = 0)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            if (RestaurantId != 0)
            {
                var HttpResponse = await _client.GetAsync("Coupon/GetAll/" + RestaurantId);
                return await _clientHelper.ParseResponseAsync<IEnumerable<CouponDTO>>(HttpResponse);
            }
            else
            {
                var HttpResponse = await _client.GetAsync("Coupon/GetAll/" + RestaurantId);
                return await _clientHelper.ParseResponseAsync<IEnumerable<CouponDTO>>(HttpResponse);
            }
        }


        public async Task<IEnumerable<CouponDTO>> GetAllAdminCouponsAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Coupon/GetAllAdmin/");
            return await _clientHelper.ParseResponseAsync<IEnumerable<CouponDTO>>(HttpResponse);

        }


        public async Task<CouponDTO> GetCouponByIdAsync(long CouponId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Coupon/" + CouponId);

            return await _clientHelper.ParseResponseAsync<CouponDTO>(HttpResponse);
        }


        public async Task<CouponDTO> UpdateCouponAsync(CouponDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Coupon", content);

            return await _clientHelper.ParseResponseAsync<CouponDTO>(HttpResponse);
        }

        public async Task<CouponDTO> ToggleActiveStatus(long CouponId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Coupon/ToggleStatus/{CouponId}");

            return await _clientHelper.ParseResponseAsync<CouponDTO>(HttpResponse);
        }
    }
}
