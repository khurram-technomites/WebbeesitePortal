using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.RestaurantCashierStaff;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
	public class RestaurantPrinterSettingClient : IRestaurantPrinterSettingClient
	{
		private readonly HttpClient _client;
		private readonly ILogger<RestaurantPrinterSettingClient> _logger;
		private readonly IHttpClientHelper _clientHelper;
		private readonly ITokenManager _tokenManager;

		public RestaurantPrinterSettingClient(HttpClient client, ILogger<RestaurantPrinterSettingClient> logger, IHttpClientHelper clientHelper, ITokenManager tokenManager)
		{
			_client = client;
			_logger = logger;
			_clientHelper = clientHelper;
			_tokenManager = tokenManager;
		}
		public async Task<IEnumerable<RestaurantPrinterSettingDTO>> GetAllAsync()
		{

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync("Restaurant/PrinterSetting");

			return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantPrinterSettingDTO>>(HttpResponse);
		}

		public async Task<RestaurantPrinterSettingDTO> GetAllByIdAsync(long Id)
		{

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync("Restaurant/PrinterSetting/" + Id);

			return await _clientHelper.ParseResponseAsync<RestaurantPrinterSettingDTO>(HttpResponse);
		}

		public async Task<IEnumerable<RestaurantPrinterSettingDTO>> GetAllByRestaurantIdAsync(long Id)
		{

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync($"Restaurant/{Id}/PrinterSetting/");

			return await _clientHelper.ParseResponseAsync<IEnumerable<RestaurantPrinterSettingDTO>>(HttpResponse);
		}

		public async Task<RestaurantPrinterSettingDTO> GetByRestaurantBranchIdAsync(long Id)
		{

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync("Restaurant/PrinterSetting/ByBranch/" + Id);

			return await _clientHelper.ParseResponseAsync<RestaurantPrinterSettingDTO>(HttpResponse);
		}

		public async Task<RestaurantPrinterSettingDTO> AddRestaurantPrinterSettingAsync(RestaurantPrinterSettingDTO Entity)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
			HttpContent content = _clientHelper.CreateHttpContent(Entity);
			var HttpResponse = await _client.PostAsync("Restaurant/PrinterSetting", content);

			return await _clientHelper.ParseResponseAsync<RestaurantPrinterSettingDTO>(HttpResponse);
		}

		public async Task<RestaurantPrinterSettingDTO> UpdateRestaurantPrinterSettingAsync(RestaurantPrinterSettingDTO Entity)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
			HttpContent content = _clientHelper.CreateHttpContent(Entity);
			var HttpResponse = await _client.PutAsync("Restaurant/PrinterSetting", content);

			return await _clientHelper.ParseResponseAsync<RestaurantPrinterSettingDTO>(HttpResponse);
		}

		public async Task DeleteRestaurantPrinterSettingAsync(long Id)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			await _client.DeleteAsync($"Restaurant/PrinterSetting/{Id}");
		}

		public async Task<RestaurantPrinterSettingDTO> ToggleActiveStatus(long Id)
		{

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync($"Restaurant/PrinterSetting/ToggleStatus/{Id}");

			return await _clientHelper.ParseResponseAsync<RestaurantPrinterSettingDTO>(HttpResponse);
		}
	}
}
