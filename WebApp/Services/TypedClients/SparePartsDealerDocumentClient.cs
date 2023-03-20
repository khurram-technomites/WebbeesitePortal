using HelperClasses.DTOs.SparePartsDealer;
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


namespace WebApp.Services.TypedClients
{
    public class SparePartsDealerDocumentClient : ISparePartsDocumentClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ISparePartsDocumentClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public SparePartsDealerDocumentClient(HttpClient client, ILogger<ISparePartsDocumentClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<SparePartsDealerDocumentDTO> AddDocument(SparePartsDealerDocumentDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("SparePartsDealerDocument/Document", content);

            return await _clientHelper.ParseResponseAsync<SparePartsDealerDocumentDTO>(HttpResponse);
        }

        public async Task DeleteDocument(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"SparePartsDealerDocument/Document/{Id}");
        }

        public async Task<SparePartsDealerDocumentDTO> GetDocumentByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SparePartsDealerDocument/Document/{Id}");

            return await _clientHelper.ParseResponseAsync<SparePartsDealerDocumentDTO>(HttpResponse);
        }

        public async Task<IEnumerable<SparePartsDealerDocumentDTO>> GetDocumentBySpareParts(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SparePartsDealerDocument/{Id}/Document");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartsDealerDocumentDTO>>(HttpResponse);
        }

        public async Task<SparePartsDealerDocumentDTO> UpdateDocument(SparePartsDealerDocumentDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("SparePartsDealerDocument/Document", content);

            return await _clientHelper.ParseResponseAsync<SparePartsDealerDocumentDTO>(HttpResponse);
        }
    }
}
