using HelperClasses.DTOs.Aggregators;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using HelperClasses.DTOs;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.RestaurantCashierStaff;

namespace WebApp.Services.TypedClients
{
	public class AggregatorClient : IAggregatorClient
	{
		private readonly HttpClient _client;
		private readonly ILogger<AggregatorClient> _logger;
		private readonly IHttpClientHelper _clientHelper;

		private readonly ITokenManager _tokenManager;

		public AggregatorClient(HttpClient client, ILogger<AggregatorClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
		{
			_client = client;
			_logger = logger;
			_clientHelper = clientHelper;
			_tokenManager = tokenManager;
		}
		public async Task<IEnumerable<AggregatorDTO>> GetAllAggregatorAsync()
		{

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync("Aggregator");

			return await _clientHelper.ParseResponseJsonAsync<IEnumerable<AggregatorDTO>>(HttpResponse);
		}
		public async Task<IEnumerable<AggregatorDTO>> GetAggregatorByIdAsync(long Id)
		{

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync($"Aggregator/{Id}");

			return await _clientHelper.ParseResponseJsonAsync<IEnumerable<AggregatorDTO>>(HttpResponse);
		}
		public async Task<IEnumerable<AggregatorDTO>> GetAggregatorByRestaurantIdAsync(long RestaurantId)
		{

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync($"Aggregator/ByRestaurant/{RestaurantId}");

			return await _clientHelper.ParseResponseJsonAsync<IEnumerable<AggregatorDTO>>(HttpResponse);
		}

		public async Task<AggregatorDTO> AddAggregatorAsync(AggregatorDTO Entity)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			HttpContent content = _clientHelper.CreateHttpContent(Entity);
			var HttpResponse = await _client.PostAsync("Aggregator", content);

			return await _clientHelper.ParseResponseJsonAsync<AggregatorDTO>(HttpResponse);
		}

		public async Task<AggregatorDTO> UpdateAggregatorAsync(AggregatorDTO Entity)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			HttpContent content = _clientHelper.CreateHttpContent(Entity);
			var HttpResponse = await _client.PutAsync("Aggregator", content);

			return await _clientHelper.ParseResponseAsync<AggregatorDTO>(HttpResponse);
		}
		public async Task<AggregatorDTO> ToggleActiveStatus(long Id)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync($"Aggregator/{Id}/ToggleStatus");

			return await _clientHelper.ParseResponseAsync<AggregatorDTO>(HttpResponse);
		}
		public async Task DeleteAggregatorAsync(long Id)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			await _client.DeleteAsync($"Aggregator/{Id}");
		}

	}
}
