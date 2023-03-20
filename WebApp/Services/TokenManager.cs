using HelperClasses.DTOs.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApp.Interfaces;

namespace WebApp.Services
{
    public class TokenManager : ITokenManager
    {
        private String _acccessToken;
        private String _refreshToken;
        private DateTime _tokenExpiry;
        private String _issuer;

        private String _webAPIURL;
        private bool RefreshInProgress = false;
        private IHttpContextAccessor _contextAccessor;
        private ILogger _logger;

        // This class is being instantiated as Singletion so this will be called only ONCE
        public TokenManager(IHttpContextAccessor ContextAccessor, IConfiguration Configuration, ILogger<ITokenManager> logger)
        {
            _logger = logger;

            if (ContextAccessor != null)
            {
                _contextAccessor = ContextAccessor;

                Claim Claim = ContextAccessor.HttpContext.User.FindFirst("AccessTokenInfo");

                if (Claim != null)
                {
                    AccessToken TokenInfo = JsonConvert.DeserializeObject<AccessToken>(Claim.Value);
                    SetTokenInfo(TokenInfo);
                }
            }

            if (Configuration != null)
            {
                _webAPIURL = Configuration.GetValue<String>("APIURL");
            }
        }
        public string GetAccessToken()
        {
            // This may be unnecessary as refresh is a sync call. But just incase
            while (RefreshInProgress) ;

            if (IsTokenAboutToExpire())
                RefreshAccessToken();

            return _acccessToken;
        }

        public void SetTokenInfo(AccessToken Token)
        {
            if (Token != null)
            {
                _acccessToken = Token.Token;
                _refreshToken = Token.RefreshToken;
                _tokenExpiry = Token.ExpiryDate;
                _issuer = Token.Issuer;
            }
        }

        private bool IsTokenAboutToExpire()
        {
            return ((_tokenExpiry - DateTime.UtcNow).TotalMinutes <= 5);
        }

        private void RefreshAccessToken()
        {
            _logger.LogInformation("Refreshing Access Token.");

            AccessToken OldToken = new AccessToken(_acccessToken, _refreshToken, DateTime.UtcNow.AddDays(-365) /* Sentinal value */,
                                                   _issuer);
            String SerializeToken = JsonConvert.SerializeObject(OldToken);
            HttpContent Content = new StringContent(SerializeToken, Encoding.UTF8, "application/json");

            String requestUrl = _webAPIURL + "Admin/AdminAccount/RefreshToken";

            HttpClient _httpClient = new HttpClient();

            RefreshInProgress = true;

            var response = _httpClient.PostAsync(new Uri(requestUrl), Content).Result;

            if (response.IsSuccessStatusCode)
            {
                String data = response.Content.ReadAsStringAsync().Result;
                AccessToken NewToken = JsonConvert.DeserializeObject<AccessToken>(data);

                SetTokenInfo(NewToken);
            }
            else
            {
                // We're going to ignore in case of errors for NOW!!????
                RefreshInProgress = false;
            }

            RefreshInProgress = false;
        }

        public bool ForcefullyRefreshAccessToken()
        {
            _logger.LogInformation("Refreshing Access Token.");

            AccessToken OldToken = new AccessToken(_acccessToken, _refreshToken, DateTime.UtcNow.AddDays(-365) /* Sentinal value */,
                                                   _issuer);
            String SerializeToken = JsonConvert.SerializeObject(OldToken);
            HttpContent Content = new StringContent(SerializeToken, Encoding.UTF8, "application/json");

            String requestUrl = _webAPIURL + "Admin/RefreshToken";

            HttpClient _httpClient = new HttpClient();

            var response = _httpClient.PostAsync(new Uri(requestUrl), Content).Result;

            if (response.IsSuccessStatusCode)
            {
                String data = response.Content.ReadAsStringAsync().Result;
                AccessToken NewToken = JsonConvert.DeserializeObject<AccessToken>(data);

                SetTokenInfo(NewToken);

                return true;
            }
            else
                // We're going to ignore in case of errors for NOW!!????
                return false;
        }
    }
}
