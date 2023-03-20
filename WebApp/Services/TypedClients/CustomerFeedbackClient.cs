using HelperClasses.DTOs;
using HelperClasses.DTOs.CustomerFeedback;
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
	public class CustomerFeedbackClient : ICustomerFeedbackClient
	{
		private readonly HttpClient _client;
		private readonly ILogger<CustomerFeedbackClient> _logger;
		private readonly IHttpClientHelper _clientHelper;
		private readonly ITokenManager _tokenManager;
		public CustomerFeedbackClient(HttpClient client, ILogger<CustomerFeedbackClient> logger, IHttpClientHelper clientHelper,
			ITokenManager tokenManager)
		{
			_client = client;
			_logger = logger;
			_clientHelper = clientHelper;
			_tokenManager = tokenManager;
		}

		public async Task<IEnumerable<CustomerFeedbackDTO>> GetAllAsync(PagingParameters paging)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
			HttpContent httpContent = _clientHelper.CreateHttpContent(paging);
			var HttpResponse = await _client.PostAsync("CustomerFeedback/GetAll", httpContent);

			return await _clientHelper.ParseResponseJsonAsync<IEnumerable<CustomerFeedbackDTO>>(HttpResponse);
		}

		public async Task<IEnumerable<CustomerFeedbackDTO>> GetAllAsync()
		{

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.PostAsync("CustomerFeedback/GetAll", null);

			return await _clientHelper.ParseResponseJsonAsync<IEnumerable<CustomerFeedbackDTO>>(HttpResponse);
		}

		public async Task<IEnumerable<CustomerFeedbackDTO>> GetByRestaurantIdAsync(long Id)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync("CustomerFeedback/Restaurant/" + Id);

			return await _clientHelper.ParseResponseJsonAsync<IEnumerable<CustomerFeedbackDTO>>(HttpResponse);
		}
		
		public async Task<CustomerFeedbackDTO> GetByIdAsync(long Id)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			var HttpResponse = await _client.GetAsync("CustomerFeedback/" + Id);

			return await _clientHelper.ParseResponseJsonAsync<CustomerFeedbackDTO>(HttpResponse);
		}

		public async Task<CustomerFeedbackDTO> AddAsync(CustomerFeedbackDTO Entity)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			HttpContent content = _clientHelper.CreateHttpContent(Entity);
			var HttpResponse = await _client.PostAsync("CustomerFeedback/", content);
			return await _clientHelper.ParseResponseJsonAsync<CustomerFeedbackDTO>(HttpResponse);
		}

		public async Task<CustomerFeedbackDTO> UpdateAsync(CustomerFeedbackDTO Entity)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			HttpContent content = _clientHelper.CreateHttpContent(Entity);
			var HttpResponse = await _client.PutAsync("CustomerFeedback/Update", content);

			return await _clientHelper.ParseResponseJsonAsync<CustomerFeedbackDTO>(HttpResponse);
		}
		
		public async Task<CustomerFeedbackDTO> DeleteAsync(long Id)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			HttpResponseMessage response = await _client.DeleteAsync($"CustomerFeedback/{Id}");

			return await _clientHelper.ParseResponseJsonAsync<CustomerFeedbackDTO>(response);
		}
		
		public async Task<CustomerFeedbackDTO> ToggleActiveStatus(long Id)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

			//HttpContent content = _clientHelper.CreateHttpContent(Entity);
			var HttpResponse = await _client.GetAsync($"CustomerFeedback/ToggleStatus/{Id}");

			return await _clientHelper.ParseResponseJsonAsync<CustomerFeedbackDTO>(HttpResponse);
		}
	}
}
