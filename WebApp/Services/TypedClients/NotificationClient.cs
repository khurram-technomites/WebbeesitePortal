using HelperClasses.DTOs;
using HelperClasses.DTOs.NotificationFilter;
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
    public class NotificationClient : INotificationClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<NotificationClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly ITokenManager _tokenManager;
        public NotificationClient(HttpClient client, ILogger<NotificationClient> logger, IHttpClientHelper clientHelper,
            ITokenManager tokenManager)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;

            _tokenManager = tokenManager;
        }
        public async Task<NotificationDTO> Create(NotificationViewModel model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PostAsync("Notification", content);

            return await _clientHelper.ParseResponseAsync<NotificationDTO>(HttpResponse);
        }

        public async Task<NotificationDTO> Delete(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpResponseMessage response = await _client.DeleteAsync($"Notification/{Id}");

            return await _clientHelper.ParseResponseAsync<NotificationDTO>(response);
        }

        public async Task<NotificationDTO> Edit(NotificationDTO model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(model);
            var HttpResponse = await _client.PutAsync("Notification", content);

            return await _clientHelper.ParseResponseAsync<NotificationDTO>(HttpResponse);
        }

        public async Task<IEnumerable<NotificationReceiverDTO>> GetNotification(NotificationFilterDTO Model)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            HttpContent content = _clientHelper.CreateHttpContent(Model);
            var HttpResponse = await _client.PostAsync("Admin/Notification/GetAll/ByUser", content);

            return await _clientHelper.ParseResponseAsync<IEnumerable<NotificationReceiverDTO>>(HttpResponse);
        }
        public async Task<IEnumerable<NotificationReceiverDTO>> MarkNotificationsAsSeen(string UserId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.PostAsync($"Notification/MarkAllSeen/ByUser/{UserId}",null);

            return await _clientHelper.ParseResponseAsync<IEnumerable<NotificationReceiverDTO>>(HttpResponse);
        }

        public async Task<NotificationDTO> GetNotificationByID(long Id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.GetAsync($"Notification/{Id}");

            return await _clientHelper.ParseResponseAsync<NotificationDTO>(HttpResponse);
        }

        public async Task<NotificationReceiverDTO> MarkNotificationsAsRead(long NotificationId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());

            var HttpResponse = await _client.PutAsync($"Notification/MarkRead/{NotificationId}", null);

            return await _clientHelper.ParseResponseAsync<NotificationReceiverDTO>(HttpResponse);
        }
    }
}
