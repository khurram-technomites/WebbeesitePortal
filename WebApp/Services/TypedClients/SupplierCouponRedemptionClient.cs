using HelperClasses.DTOs.Supplier;
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
    public class SupplierCouponRedemptionClient : ISupplierCouponRedemptionClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SupplierCouponRedemptionClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public SupplierCouponRedemptionClient(HttpClient client, ILogger<SupplierCouponRedemptionClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<SupplierCouponRedemptionDTO> AddSupplierCouponRedemptionAsync(SupplierCouponRedemptionDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("SupplierCouponRedemption", content);

            return await _clientHelper.ParseResponseAsync<SupplierCouponRedemptionDTO>(HttpResponse);
        }

        public async Task<IEnumerable<SupplierCouponRedemptionDTO>> GetSupplierCouponRedemptionsByRestaurant(string restaurantId, long SupplierCouponId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SupplierCouponRedemption/GetAll/Restaurant/" + restaurantId + "/Coupon/" + SupplierCouponId);
            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierCouponRedemptionDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<SupplierCouponRedemptionDTO>> GetAllSupplierCouponRedemptions(long SupplierCouponId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("SupplierCouponRedemption/GetAll/Coupons/" + SupplierCouponId);
            return await _clientHelper.ParseResponseAsync<IEnumerable<SupplierCouponRedemptionDTO>>(HttpResponse);
        }

    }
}
