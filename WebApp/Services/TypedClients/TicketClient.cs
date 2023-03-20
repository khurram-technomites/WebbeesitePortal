using HelperClasses.DTOs;
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
using WebApp.ViewModels;

namespace WebApp.Services.TypedClients
{
    public class TicketClient : ITicketClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<TicketClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public TicketClient(HttpClient client, ILogger<TicketClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<IEnumerable<TicketDTO>> GetAllTicketsAsync()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Ticket");

            return await _clientHelper.ParseResponseAsync<IEnumerable<TicketDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<TicketDTO>> GetTicketsByOpenStatus()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("Status");

            return await _clientHelper.ParseResponseAsync<IEnumerable<TicketDTO>>(HttpResponse);
        }
        public async Task<TicketDTO> GetTicket(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Ticket/GetById/{Id}");

            return await _clientHelper.ParseResponseAsync<TicketDTO>(HttpResponse);
        }
        public async Task<IEnumerable<TicketDTO>> GetTicketsByRestaurant(long RestaurantId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Ticket/Restaurant/{RestaurantId}");

            return await _clientHelper.ParseResponseAsync< IEnumerable<TicketDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<TicketDTO>> GetAllTicketsByModuleAsync(string Module)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Ticket/Supplier/List/{Module}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<TicketDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<TicketDTO>> GetTicketsBySupplier(long SupplierId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Ticket/Supplier/{SupplierId}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<TicketDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<TicketDTO>> GetTicketsByUser(string userID)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Ticket/User/{userID}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<TicketDTO>>(HttpResponse);
        }
        public async Task<TicketDTO> UpdateTicket(TicketViewModel model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("Ticket", content);

            return await _clientHelper.ParseResponseAsync<TicketDTO>(HttpResponse);
        }
        public async Task<TicketDTO> UpdateStatus(TicketViewModel model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("Ticket/UpdateStatus", content);

            return await _clientHelper.ParseResponseAsync<TicketDTO>(HttpResponse);
        }
        public async Task<TicketDTO> CreateTicket(TicketViewModel model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("Ticket", content);

            return await _clientHelper.ParseResponseAsync<TicketDTO>(HttpResponse);
        }
    }
}
