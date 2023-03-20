using HelperClasses.DTOs;
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
    public class SupplierCouponClient : ISupplierCouponClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SupplierCouponClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public SupplierCouponClient(HttpClient client, ILogger<SupplierCouponClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<SupplierCouponDTO> AddCouponAsync(SupplierCouponDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("Supplier/Coupon", content);

            return await _clientHelper.ParseResponseAsync<SupplierCouponDTO>(HttpResponse);
        }

        public async Task<SupplierCouponDTO> DeleteCouponAsync(long CouponId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            return await _clientHelper.ParseResponseAsync<SupplierCouponDTO>(await _client.DeleteAsync($"Supplier/Coupon/{CouponId}"));
        }

        public async Task<IEnumerable<SupplierCouponDTO>> GetAllCouponsAsync(long RestaurantId = 0)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            if (RestaurantId != 0)
            {
                var HttpResponse = await _client.GetAsync("Supplier/Coupon/GetAll/" + RestaurantId);
                return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierCouponDTO>>(HttpResponse);
            }
            else
            {
                var HttpResponse = await _client.GetAsync("Supplier/Coupon/GetAll/" + RestaurantId);
                return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierCouponDTO>>(HttpResponse);
            }
        }

        public async Task<IEnumerable<SupplierCouponDTO>> GetAllCouponByRestaurantAsync(long RestaurantId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

                var HttpResponse = await _client.GetAsync("Supplier/Coupon/GetAll/Restaurants/" + RestaurantId);
                return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierCouponDTO>>(HttpResponse);
           
        }



        public async Task<IEnumerable<SupplierCouponDTO>> GetAllAdminCouponsAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Supplier/Coupon/GetAllAdmin/");
            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierCouponDTO>>(HttpResponse);

        }


        public async Task<SupplierCouponDTO> GetCouponByIdAsync(long CouponId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Supplier/Coupon/" + CouponId);

            return await _clientHelper.ParseResponseAsync<SupplierCouponDTO>(HttpResponse);
        }


        public async Task<SupplierCouponDTO> UpdateCouponAsync(SupplierCouponDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Supplier/Coupon", content);

            return await _clientHelper.ParseResponseAsync<SupplierCouponDTO>(HttpResponse);
        }

        public async Task<SupplierCouponDTO> ToggleActiveStatus(long CouponId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Supplier/Coupon/ToggleStatus/{CouponId}");

            return await _clientHelper.ParseResponseAsync<SupplierCouponDTO>(HttpResponse);
        }
    }
}
