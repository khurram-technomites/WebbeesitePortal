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
    public class TicketDocumentClient : ITicketDocumentClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<TicketMessageClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public TicketDocumentClient(HttpClient client, ILogger<TicketMessageClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<IEnumerable<TicketDocumentDTO>> GetAllDocumetsAsync()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("TicketDocument");

            return await _clientHelper.ParseResponseAsync<IEnumerable<TicketDocumentDTO>>(HttpResponse);
        }
        public async Task<TicketDocumentDTO> GetTicketDocumentById(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"TicketDocument/GetById/{Id}");

            return await _clientHelper.ParseResponseAsync<TicketDocumentDTO>(HttpResponse);
        }
        public async Task<TicketDocumentDTO> CreateTicketDocument(TicketDocumentViewModel model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("TicketDocument", content);

            return await _clientHelper.ParseResponseAsync<TicketDocumentDTO>(HttpResponse);
        }
    }
}
