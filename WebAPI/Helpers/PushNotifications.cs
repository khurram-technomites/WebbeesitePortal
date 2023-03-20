using HelperClasses.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Helpers
{
    public class PushNotifications
    {
        //private static readonly IIntegrationSettingService _integrationSettings;
        private static Uri _FireBasePushNotificationsURL = new Uri("https://fcm.googleapis.com/fcm/send");

        public static async Task<bool> SendPushNotification(string[] deviceTokens, string title, string body, object data, string ServerKey, bool forCustomer = true)
        {
            bool sent = false;

            if (deviceTokens.Count() > 0)
            {
                //Object creation

                var messageInformation = new MessageDTO()
                {
                    notification = new MessageNotificationDTO()
                    {
                        title = title,
                        body = body,
                        sound = "default",
                    },
                    data = data,
                    sound = "default",
                    registration_ids = deviceTokens
                };

                //Object to JSON STRUCTURE => using Newtonsoft.Json;
                string jsonMessage = JsonConvert.SerializeObject(messageInformation);

                //Create request to Firebase API
                var request = new HttpRequestMessage(HttpMethod.Post, _FireBasePushNotificationsURL);

                request.Headers.TryAddWithoutValidation("Authorization", "key=" + ServerKey);

                request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                HttpResponseMessage result;
                using var client = new HttpClient();
                result = await client.SendAsync(request);
                sent = sent && result.IsSuccessStatusCode;
            }

            return sent;
        }

    }
}
