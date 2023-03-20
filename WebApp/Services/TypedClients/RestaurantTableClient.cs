using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces.TypedClients;
using HelperClasses.DTOs.RestaurantCashierStaff;
using WebAPI.Models;
using HelperClasses.DTOs;

namespace WebApp.Services.TypedClients
{
	public class RestaurantTableClient : IRestaurantTableClient
	{
		private readonly HttpClient _client;
		private readonly ILogger<RestaurantTableClient> _logger;
		private readonly IHttpClientHelper _clientHelper;
		private readonly ITokenManager _tokenManager;
		public RestaurantTableClient(HttpClient client, ILogger<RestaurantTableClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
		{
			_client = client;
			_logger = logger;
			_clientHelper = clientHelper;
			_tokenManager = tokenManager;
		}
		public async Task<IEnumerable<RestaurantTableDTO>> GetAllAsync()
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync("Restaurant/Table");

			return await _clientHelper.ParseResponseJsonAsync<IEnumerable<RestaurantTableDTO>>(HttpResponse);
		}

		public async Task<RestaurantTableDTO> GetAllByIdAsync(long Id)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync($"Restaurant/Table/{Id}");


            return await _clientHelper.ParseResponseJsonAsync<RestaurantTableDTO>(HttpResponse);
		}

		public async Task<IEnumerable<RestaurantTableDTO>> GetAllByRestaurantIdAsync(long restaurantId)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync($"Restaurant/GetByRestaurantId/{restaurantId}/Table");


            return await _clientHelper.ParseResponseJsonAsync<IEnumerable<RestaurantTableDTO>>(HttpResponse);
		}

		public async Task<IEnumerable<RestaurantTableDTO>> GetByRestaurantBranchIdAsync(long branchId)
		{

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync($"Restaurant/GetByRestaurantBranchId/{branchId}/Table");


            return await _clientHelper.ParseResponseJsonAsync<IEnumerable<RestaurantTableDTO>>(HttpResponse);
		}

		public async Task<RestaurantTableDTO> AddRestaurantTableAsync(RestaurantTableDTO Entity)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
			HttpContent content = _clientHelper.CreateHttpContent(Entity);
			var HttpResponse = await _client.PostAsync("Restaurant/Table/", content);

			return await _clientHelper.ParseResponseJsonAsync<RestaurantTableDTO>(HttpResponse);
		}

		public async Task<RestaurantTableDTO> UpdateRestaurantTableAsync(RestaurantTableDTO Entity)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
			HttpContent content = _clientHelper.CreateHttpContent(Entity);
			var HttpResponse = await _client.PutAsync("Restaurant/Table/", content);

			return await _clientHelper.ParseResponseJsonAsync<RestaurantTableDTO>(HttpResponse);
		}
		public async Task<RestaurantTableDTO> ToggleActiveStatus(long Id)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync($"Restaurant/Table/{Id}/ToggleStatus");

			return await _clientHelper.ParseResponseJsonAsync<RestaurantTableDTO>(HttpResponse);
		}
		public async Task DeleteRestaurantTableAsync(long Id)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			await _client.DeleteAsync($"Restaurant/Table/{Id}");
		}
	}
}
