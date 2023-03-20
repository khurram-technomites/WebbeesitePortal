using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces.TypedClients;

namespace WebApp.Services.TypedClients
{
    public class AuthenticateClient : IAuthenticateClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<AuthenticateClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly IHttpContextAccessor _context;
        private readonly ITokenManager _tokenManager;
        private readonly IUserSessionManager _sessionManager;
        private readonly IMapper _mapper;

        public AuthenticateClient(HttpClient client, ILogger<AuthenticateClient> logger, IHttpClientHelper httpClientHelper,
                                    IHttpContextAccessor context, ITokenManager tokenManager, IUserSessionManager sessionManager,
                                    IMapper mapper)
        {
            _client = client;
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _tokenManager = tokenManager;
            _sessionManager = sessionManager;

            _clientHelper = httpClientHelper;
        }

        public async Task<LoginResponse> ConfirmEmailAsync(string UserId, int Code)
        {
            HttpContent Content = _clientHelper.CreateHttpContent<ConfirmUser>(new ConfirmUser()
            {
                AuthCode = Code,
                UserId = UserId
            });

            HttpResponseMessage response = await _client.PostAsync("Account/ConfirmEmail", Content);

            return await _clientHelper.ParseResponseAsync<LoginResponse>(response);
        }

        public async Task<LoginResponse> ForgetPasswordAsync(string Email)
        {
            HttpContent Content = _clientHelper.CreateHttpContent<string>(Email);

            HttpResponseMessage response = await _client.PostAsync("Account/ForgetPassword", Content);

            return await _clientHelper.ParseResponseAsync<LoginResponse>(response);
        }

        public async Task<bool> IsEmailConfirmedByName(string Name)
        {
            HttpResponseMessage response = await _client.GetAsync("Account/IsEmailConfirmed/" + Name);

            return await _clientHelper.ParseResponseAsync<bool>(response);
        }

        public async Task<UserAuthData> LoginAsync(string Username, string Password, bool RememberMe)
        {
            HttpContent Content = _clientHelper.CreateHttpContent<LoginData>(new LoginData()
            {
                Email = Username,
                Password = Password
            });

            HttpResponseMessage response = await _client.PostAsync("Admin/Login", Content);

            LoginResponse APIRes = await _clientHelper.ParseResponseAsync<LoginResponse>(response);

            await SignInAsync(APIRes, RememberMe);
            return APIRes.AuthData;
        }

        public async Task<LoginResponse> RegisterAsync(RegisterData Data)
        {
            HttpContent Content = _clientHelper.CreateHttpContent<RegisterData>(Data);

            HttpResponseMessage response = await _client.PostAsync("Account/Register", Content);

            return await _clientHelper.ParseResponseAsync<LoginResponse>(response);
        }

        public async Task<LoginResponse> ResendConfirmEmailAsync(string Email)
        {
            HttpContent Content = _clientHelper.CreateHttpContent<string>(Email);

            HttpResponseMessage response = await _client.PostAsync("Account/ResendConfirmEmail", Content);

            return await _clientHelper.ParseResponseAsync<LoginResponse>(response);
        }

        public async Task ResetPasswordAsync(ResetPasswordDTO ResetData)
        {
            HttpContent Content = _clientHelper.CreateHttpContent<ResetPasswordDTO>(ResetData);

            await _client.PostAsync("Account/ResetPassword", Content);
        }

        private async Task SignInAsync(LoginResponse response, bool RememberMe = false)
        {
            UserAuthData authData = response.AuthData;
            StoreDTO StoreData = _mapper.Map<StoreDTO>(response.Store);

            _tokenManager.SetTokenInfo(authData.TokenInfo);

            string TokenInfoStr = JsonConvert.SerializeObject(authData.TokenInfo);
            string StoreInfoStr = JsonConvert.SerializeObject(StoreData);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.Sid, authData.UserId));
            identity.AddClaim(new Claim(ClaimTypes.Email, authData.Email));
            identity.AddClaim(new Claim(ClaimTypes.Name, authData.UserName));
            identity.AddClaim(new Claim("Firstname", authData.FirstName));
            identity.AddClaim(new Claim("Lastname", authData.LastName));
            //identity.AddClaim(new Claim("Profile_Picture", (authData.PicturePath == null) ? "" : authData.PicturePath));
            identity.AddClaim(new Claim("AccessTokenInfo", TokenInfoStr));
            identity.AddClaim(new Claim("UserStoreInfo", StoreInfoStr));

            if(authData.Claims.Count > 0)
            {
                foreach (CustomeClaims claim in authData.Claims)
                    identity.AddClaim(new Claim(claim.Type, claim.Value));
            }            

            identity.AddClaim(new Claim(authData.UserRole.Name, authData.UserRole.Name));

            var principal = new ClaimsPrincipal(identity);

            _sessionManager.ClearSession();

            await _context.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                                                   new AuthenticationProperties { IsPersistent = RememberMe });

            //_sessionManager.SetUserStore(StoreData);

            _logger.LogInformation("Sign In Successfull for user: " + authData.UserName);
        }
    }
}
