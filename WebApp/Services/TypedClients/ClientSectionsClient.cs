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

namespace WebApp.Services.TypedClients
{
    public class ClientSectionsClient : IClientSectionsClient 
    {
        private readonly HttpClient _client;
        private readonly ILogger<CityClient> _logger;
        private readonly IHttpClientHelper _clientHelper;
        private readonly ITokenManager _tokenManager;
        public ClientSectionsClient(HttpClient client, ILogger<CityClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<ClientSectionsDTO> Create(ClientSectionsDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("ClientSections", content);

            return await _clientHelper.ParseResponseAsync<ClientSectionsDTO>(HttpResponse);
        }

        public async Task<ClientSectionsDTO> Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"ClientSections/{Id}");

            return await _clientHelper.ParseResponseAsync<ClientSectionsDTO>(response);
        }

        public async Task<ClientSectionsDTO> Edit(ClientSectionsDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("ClientSections", content);

            return await _clientHelper.ParseResponseAsync<ClientSectionsDTO>(HttpResponse);
        }

        public async Task<IEnumerable<ClientSectionsDTO>> GetCities()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("ClientSections");

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientSectionsDTO>>(HttpResponse);
        }

        public async Task<IEnumerable<ClientSectionsDTO>> GetCitiesMaster()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync("ClientSections/Master");

            return await _clientHelper.ParseResponseAsync<IEnumerable<ClientSectionsDTO>>(HttpResponse);
        }

        public async Task<ClientSectionsDTO> GetCityByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"ClientSections/{Id}");

            return await _clientHelper.ParseResponseAsync<ClientSectionsDTO>(HttpResponse);
        }
        public async Task<ClientSectionsDTO> ToggleActiveStatus(long CityId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            //HttpContent content = _clientHelper.CreateHttpContent(Entity);
            var HttpResponse = await _client.GetAsync($"ClientSections/ToggleStatus/{CityId}");

            return await _clientHelper.ParseResponseAsync<ClientSectionsDTO>(HttpResponse);
        }
    }

}
