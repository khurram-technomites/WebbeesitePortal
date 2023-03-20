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
    public class CustomerCouponClient : ICustomerCouponClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<CustomerCouponClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public CustomerCouponClient(HttpClient client, ILogger<CustomerCouponClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        public async Task<CustomerCouponDTO> AddCustomerCouponAsync(CustomerCouponDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PostAsync("CustomerCoupon", content);

            return await _clientHelper.ParseResponseAsync<CustomerCouponDTO>(HttpResponse);
        }

        public async Task DeleteCustomerCouponAsync(long CustomerCouponId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"CustomerCoupon/{CustomerCouponId}");
        }

        public async Task<CustomerCouponDTO> GetCustomerCoupon(long CustomerId, long CouponId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

                var HttpResponse = await _client.GetAsync("CustomerCoupon/Customers/" + CustomerId + "/Coupons/" + CouponId );
                return await _clientHelper.ParseResponseAsync<CustomerCouponDTO>(HttpResponse);
        }


        public async Task<IEnumerable<CustomerCouponDTO>> GetCustomerCouponsByCoupon(long CouponId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("CustomerCoupon/Coupons/" + CouponId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<CustomerCouponDTO>>(HttpResponse);
        }


        public async Task<IEnumerable<CustomerCouponDTO>> GetAllCustomerCoupons(long CustomerId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("CustomerCoupon/Customers/" + CustomerId);

            return await _clientHelper.ParseResponseAsync<IEnumerable<CustomerCouponDTO>>(HttpResponse);
        }


        public async Task<CustomerCouponDTO> UpdateCustomerCouponAsync(CustomerCouponDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("CustomerCoupon", content);

            return await _clientHelper.ParseResponseAsync<CustomerCouponDTO>(HttpResponse);
        }

        public async Task<CustomerCouponDTO> ToggleActiveStatus(long CustomerCouponId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"CustomerCoupon/ToggleStatus/{CustomerCouponId}");

            return await _clientHelper.ParseResponseAsync<CustomerCouponDTO>(HttpResponse);
        }
    }
}
