using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
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
    public class CustomerClient : ICustomerClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<CustomerClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public CustomerClient(HttpClient client, ILogger<CustomerClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _tokenManager = tokenManager;
        }

        //public async Task<CustomerDTO> AddCustomerAsync(CustomerDTO Entity)
        //{
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

        //    HttpContent content = _clientHelper.CreateHttpContent(Entity);
        //    var HttpResponse = await _client.PostAsync("Customer/Accounty", content);

        //    return await _clientHelper.ParseResponseAsync<CustomerDTO>(HttpResponse);
        //}
        public async Task DeleteCustomerAsync(long CustomerId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"Customer/{CustomerId}");
        }

        public async Task<IEnumerable<RestaurantCustomerDTO>> GetAllCustomersByRestaurantAsync(long RestaurantId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Customer/GetAll/{RestaurantId}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantCustomerDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            var HttpResponse = await _client.GetAsync("Customer/GetAll");
            return await _clientHelper.ParseResponseAsync<IEnumerable<CustomerDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<RestaurantDTO>> GetAllSupplierCustomersAsync()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            var HttpResponse = await _client.GetAsync("Restaurant");
            return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<object>> GetAllCustomerDropDownByRestaurant(long RestaurantId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            
                var HttpResponse = await _client.GetAsync("Customer/DropDown/" + RestaurantId);
                return await _clientHelper.ParseResponseAsync<IEnumerable<object>>(HttpResponse);

        }

        public async Task<IEnumerable<object>> GetAllCustomerDropDownByAdminAsync()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            var HttpResponse = await _client.GetAsync("Customer/DropDownByAdmin/");
            return await _clientHelper.ParseResponseAsync<IEnumerable<object>>(HttpResponse);

        }

        /*ublic async Task<IEnumerable<CustomerDTO>> GetAllCustomersByRestaurantAsync(long CustomerId)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());


            var HttpResponse = await _client.GetAsync("Customer/GetAll/" + CustomerId);
            return await _clientHelper.ParseResponseAsync<IEnumerable<CustomerDTO>>(HttpResponse);



        }*/

        public async Task<CustomerDTO> GetCustomerByIdAsync(long CustomerId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Customer/" + CustomerId);

            return await _clientHelper.ParseResponseAsync<CustomerDTO>(HttpResponse);
        }

        public async Task<CustomerDTO> UpdateCustomerAsync(CustomerDTO Entity)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.PutAsync("Customer", content);

            return await _clientHelper.ParseResponseAsync<CustomerDTO>(HttpResponse);
        }

        public async Task<CustomerDTO> ToggleActiveStatus(long CustomerId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Customer/ToggleStatus/{CustomerId}");

            return await _clientHelper.ParseResponseAsync<CustomerDTO>(HttpResponse);
        }

        public async Task<IEnumerable<CustomerDTO>> GetCustomers()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Customers/GetAll");

            return await _clientHelper.ParseResponseAsync<IEnumerable<CustomerDTO>>(HttpResponse);
        }
       
        public async Task<IEnumerable<CustomerDTO>> GetCustomersByRestaurantId(long RestaurantId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Customer/ToggleStatus/{RestaurantId}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<CustomerDTO>>(HttpResponse);
        }
    }
}
