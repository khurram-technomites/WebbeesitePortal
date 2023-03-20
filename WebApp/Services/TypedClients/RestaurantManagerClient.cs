using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
	public class RestaurantManagerClient : IRestaurantManagerClient
	{
		private readonly HttpClient _client;
		private readonly ILogger<RestaurantManagerClient> _logger;
		private readonly IHttpClientHelper _clientHelper;
		private readonly ITokenManager _tokenManager;
		public RestaurantManagerClient(HttpClient client, ILogger<RestaurantManagerClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
		{
			_client = client;
			_logger = logger;
			_clientHelper = clientHelper;
			_tokenManager = tokenManager;
		}
		public async Task<IEnumerable<RestaurantManagerDTO>> GetAllAsync()
		{

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync("Restaurant/Manager");

			return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantManagerDTO>>(HttpResponse);
		}

		public async Task<RestaurantManagerDTO> GetAllByIdAsync(long Id)
		{

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync("Restaurant/Manager/" + Id);

			return await _clientHelper.ParseResponseAsync<RestaurantManagerDTO>(HttpResponse);
		}

		public async Task<IEnumerable<RestaurantManagerDTO>> GetAllByRestaurantIdAsync(long Id)
		{

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync($"Restaurant/{Id}/Manager/");

			return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantManagerDTO>>(HttpResponse);
		}

		public async Task<RestaurantManagerDTO> GetByRestaurantBranchIdAsync(long Id)
		{

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync("Restaurant/Manager/ByBranch/" + Id);

			return await _clientHelper.ParseResponseAsync<RestaurantManagerDTO>(HttpResponse);
		}

		public async Task<RestaurantManagerDTO> AddRestaurantManagerAsync(RestaurantManagerDTO Entity)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
			HttpContent content = _clientHelper.CreateHttpContent(Entity);
			var HttpResponse = await _client.PostAsync("Restaurant/Manager", content);

			return await _clientHelper.ParseResponseAsync<RestaurantManagerDTO>(HttpResponse);
		}

		public async Task<RestaurantManagerDTO> UpdateRestaurantManagerAsync(RestaurantManagerDTO Entity)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
			HttpContent content = _clientHelper.CreateHttpContent(Entity);
			var HttpResponse = await _client.PutAsync("Restaurant/Manager", content);

			return await _clientHelper.ParseResponseAsync<RestaurantManagerDTO>(HttpResponse);
		}
		public async Task<RestaurantManagerDTO> ToggleActiveStatus(long Id)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync($"Restaurant/Manager/ToggleStatus/{Id}");

			return await _clientHelper.ParseResponseAsync<RestaurantManagerDTO>(HttpResponse);
		}
		public async Task DeleteRestaurantManagerAsync(long Id)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			await _client.DeleteAsync($"Restaurant/Manager/{Id}");
		}
	}
}
