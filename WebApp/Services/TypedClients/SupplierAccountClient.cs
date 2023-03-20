using AutoMapper;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Services.TypedClients
{
    public class SupplierAccountClient : ISupplierAccountClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<AuthenticateClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly IHttpContextAccessor _context;
        private readonly ITokenManager _tokenManager;
        private readonly IUserSessionManager _sessionManager;
        private readonly IMapper _mapper;

        public SupplierAccountClient(HttpClient client, ILogger<AuthenticateClient> logger, IHttpClientHelper httpClientHelper,
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
        public async Task<LoginResponse> LoginAsync(string Username, string Password, bool RememberMe)
        {
            HttpContent Content = _clientHelper.CreateHttpContent<LoginData>(new LoginData()
            {
                Email = Username,
                Password = Password
            });

            HttpResponseMessage response = await _client.PostAsync("Supplier/Account/Login", Content);

            LoginResponse APIRes = await _clientHelper.ParseResponseAsync<LoginResponse>(response);

            await SignInAsync(APIRes, RememberMe);
            return APIRes;
        }
        public async Task<bool> IsEmailConfirmedByName(string Name)
        {
            HttpResponseMessage response = await _client.GetAsync($"Supplier/Account/IsEmailConfirmed/{Name}" );

            return await _clientHelper.ParseResponseAsync<bool>(response);
        }
        public async Task<bool> ChangePasswordAsync(ChangePasswordViewModel changePasswordViewModel)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(changePasswordViewModel);
            var HttpResponse = await _client.PutAsync("Supplier/Account/ChangePassword",content);

            if (HttpResponse.IsSuccessStatusCode)
            {
                return true;
            }
            return false;

        }
        private async Task SignInAsync(LoginResponse response, bool RememberMe = false)
        {

            UserAuthData authData = response.AuthData;
            //StoreDTO StoreData = _mapper.Map<StoreDTO>(response.Store);

            _tokenManager.SetTokenInfo(authData.TokenInfo);

            string TokenInfoStr = JsonConvert.SerializeObject(authData.TokenInfo);
            //string StoreInfoStr = JsonConvert.SerializeObject(StoreData);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.Sid, authData.UserId));
            identity.AddClaim(new Claim(ClaimTypes.Email, authData.Email));
            identity.AddClaim(new Claim(ClaimTypes.Name, authData.UserName));
            identity.AddClaim(new Claim("Firstname", authData.FirstName));
            //identity.AddClaim(new Claim("Lastname", authData.LastName));
            //identity.AddClaim(new Claim("Profile_Picture", (authData.PicturePath == null) ? "" : authData.PicturePath));
            identity.AddClaim(new Claim("AccessTokenInfo", TokenInfoStr));
            //identity.AddClaim(new Claim("UserStoreInfo", StoreInfoStr));

            if (authData.Claims.Count > 0)
            {
                foreach (CustomeClaims claim in authData.Claims)
                    identity.AddClaim(new Claim(claim.Type, claim.Value));
            }

            identity.AddClaim(new Claim(authData.UserRole.Name, authData.UserRole.Name));
            identity.AddClaim(new Claim(ClaimTypes.Role, authData.UserRole.Name));

            var principal = new ClaimsPrincipal(identity);

            _sessionManager.ClearSession();

            await _context.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                                                   new AuthenticationProperties { IsPersistent = RememberMe });
            if (response.Supplier != null)
            {
                _sessionManager.SetSupplierStore(response.Supplier);
            }
            else
            {
                _sessionManager.SetUser(authData);
            }

            _logger.LogInformation("Sign In Successfull for user: " + authData.UserName);
        }

        public async Task<LoginResponse> RegisterAsync(RegisterData Data)
        {
            HttpContent Content = _clientHelper.CreateHttpContent<RegisterData>(Data);

            HttpResponseMessage response = await _client.PostAsync("Supplier/Account/Register", Content);

            return await _clientHelper.ParseResponseAsync<LoginResponse>(response);
        }

        public async Task<LoginResponse> ResendConfirmEmailAsync(string Email)
        {
            HttpContent Content = _clientHelper.CreateHttpContent<string>(Email);

            HttpResponseMessage response = await _client.PostAsync("Supplier/Account/ResendConfirmEmail", Content);

            return await _clientHelper.ParseResponseAsync<LoginResponse>(response);
        }

        public Task<LoginResponse> ResendOTPAsync(string Contact)
        {
            throw new System.NotImplementedException();
        }



        public async Task<bool> ResetPasswordAsync(ChangePasswordDTO ResetData)
        {
            HttpContent Content = _clientHelper.CreateHttpContent<ChangePasswordDTO>(ResetData);

            HttpResponseMessage response = await _client.PostAsync("Supplier/Account/ResetPassword", Content);

            return response.StatusCode == System.Net.HttpStatusCode.OK ? true : false;
        }

        public async Task<string> VerifyOtp(string UserId, int OTP)
        {
            //HttpContent Content = _clientHelper.CreateHttpContent<ChangePasswordDTO>(new ChangePasswordDTO()
            //{
            //    UserId = userId,
            //    AuthCode = code
            //});

            HttpResponseMessage response = await _client.PostAsync($"Supplier/Account/VerifyOTP/VerifyOTPForSupplier/{UserId}/{OTP}", null);

            return await response.Content.ReadAsStringAsync();

        }
        public async Task<string> ForgetPasswordAsync(string Email)
        {
            HttpContent Content = _clientHelper.CreateHttpContent<string>(Email);

            HttpResponseMessage response = await _client.PostAsync("Supplier/Account/ForgetPassword/" + Email, null);
            //string h = await _clientHelper.ParseResponseAsync<string>(response);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
