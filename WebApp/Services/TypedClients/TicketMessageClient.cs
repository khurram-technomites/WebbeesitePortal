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
    public class TicketMessageClient : ITicketMessageClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<TicketMessageClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public TicketMessageClient(HttpClient client, ILogger<TicketMessageClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<IEnumerable<TicketMessagesDTO>> GetAllTicketsAsync()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("TicketMessage");

            return await _clientHelper.ParseResponseAsync<IEnumerable<TicketMessagesDTO>>(HttpResponse);
        }
        public async Task<TicketMessagesDTO> GetTicketMessageById(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"TicketMessage/GetById/{Id}");

            return await _clientHelper.ParseResponseAsync<TicketMessagesDTO>(HttpResponse);
        }
        public async Task<IEnumerable<TicketMessagesDTO>> GetTicketMessageByTicketId(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"TicketMessage/TicketConversation/{Id}");

            return await _clientHelper.ParseResponseAsync<IEnumerable<TicketMessagesDTO>>(HttpResponse);
        }
        public async Task<TicketMessagesDTO> CreateTicketMessage(TicketMessageViewModel model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("TicketMessage", content);

            return await _clientHelper.ParseResponseAsync<TicketMessagesDTO>(HttpResponse);
        }
    }
}
