using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Supplier;
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
    public class RestaurantCouponClient : IRestaurantCouponClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<RestaurantCouponClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public RestaurantCouponClient(HttpClient client, ILogger<RestaurantCouponClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<RestaurantCouponDTO> AddRestaurantCouponAsync(RestaurantCouponDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("RestaurantCoupon", content);

            return await _clientHelper.ParseResponseAsync<RestaurantCouponDTO>(HttpResponse);
        }

        public async Task DeleteRestaurantCouponAsync(long RestaurantCouponId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"RestaurantCoupon/{RestaurantCouponId}");
        }

        public async Task<RestaurantCouponDTO> GetRestaurantCoupon(long restaurantId, long SupplierCouponId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("RestaurantCoupon/Restaurant/" + restaurantId + "/Coupons/" + SupplierCouponId);
            return await _clientHelper.ParseResponseAsync<RestaurantCouponDTO>(HttpResponse);
        }


        public async Task<IEnumerable<RestaurantCouponDTO>> GetRestaurantCouponsByCoupon(long CouponId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("RestaurantCoupon/Coupons/" + CouponId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantCouponDTO>>(HttpResponse);
        }


        public async Task<IEnumerable<SupplierCouponDTO>> GetAllRestaurantCoupons(long RestaurantId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("RestaurantCoupon/Restaurant/" + RestaurantId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierCouponDTO>>(HttpResponse);
        }


        public async Task<RestaurantCouponDTO> UpdateRestaurantCouponAsync(RestaurantCouponDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("RestaurantCoupon", content);

            return await _clientHelper.ParseResponseAsync<RestaurantCouponDTO>(HttpResponse);
        }

        public async Task<RestaurantCouponDTO> ToggleActiveStatus(long RestaurantCouponId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"RestaurantCoupon/ToggleStatus/{RestaurantCouponId}");

            return await _clientHelper.ParseResponseAsync<RestaurantCouponDTO>(HttpResponse);
        }
    }
}
