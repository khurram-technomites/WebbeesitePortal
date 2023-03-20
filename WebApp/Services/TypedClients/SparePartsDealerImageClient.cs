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
    public class SparePartsDealerImageClient : ISparePartsDealerImageClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SparePartsDealerImageClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;

        public SparePartsDealerImageClient(ITokenManager tokenManager, IHttpClientHelper clientHelper, ILogger<SparePartsDealerImageClient> logger, HttpClient client)
        {
            _tokenManager = tokenManager;
            _clientHelper = clientHelper;
            _logger = logger;
            _client = client;
        }

        public async Task Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            await _client.DeleteAsync($"SparePartsDealerImage/Image/{Id}");
        }

        public async Task<IEnumerable<SparePartDealerImagesViewModel>> GetBySpareParts(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"SparePartsDealerImage/{Id}/Image");

            return await _clientHelper.ParseResponseAsync<IEnumerable<SparePartDealerImagesViewModel>>(HttpResponse);
        }
    }
}
