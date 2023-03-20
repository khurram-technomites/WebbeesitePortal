using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using WebApp.Interfaces.Helpers;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using HelperClasses.DTOs.Authentication;
using System.Net.Http.Headers;
using WebApp.ViewModels;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System;
using HelperClasses.Classes;

namespace WebApp.Services.TypedClients
{
    public class GarageAndSparePartDealerAccountClient: IGarageAndSparePartDealerAccountClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<AuthenticateClient> _logger;
        private readonly IHttpClientHelper _clientHelper;

        private readonly IHttpContextAccessor _context;
        private readonly ITokenManager _tokenManager;
        private readonly IUserSessionManager _sessionManager;
        private readonly IMapper _mapper;
        public GarageAndSparePartDealerAccountClient(HttpClient client, ILogger<AuthenticateClient> logger, IHttpClientHelper clientHelper, IHttpContextAccessor context, ITokenManager tokenManager, IUserSessionManager sessionManager, IMapper mapper)
        {
            _client = client;
            _logger = logger;
            _clientHelper = clientHelper;
            _context = context;
            _tokenManager = tokenManager;
            _sessionManager = sessionManager;
            _mapper = mapper;
        }

        public async Task<string> ForgetPasswordAsync(string Email,string For)
        {
            HttpContent Content = _clientHelper.CreateHttpContent<string>(Email);

            HttpResponseMessage response = await _client.PostAsync($"GarageAndSparePartDealerAccount/ForgetPasswordByEmail/{Email}/{For}", null);

            string UserId = await _clientHelper.ParseResponseStringAsync(response);

            return UserId;
        }

        public async Task<string> VerifyOtp(string UserId, int OTP)
        {
            HttpResponseMessage response = await _client.PostAsync($"GarageAndSparePartDealerAccount/VerifyOTP/{OTP}/{UserId}", null);

            return await _clientHelper.ParseResponseStringAsync(response);
        }

        public Task<LoginResponse> ResendOTPAsync(string Contact,string OTPFor)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> ResetPasswordAsync(ChangePasswordDTO ResetData)
        {
            HttpContent Content = _clientHelper.CreateHttpContent<ChangePasswordDTO>(ResetData);

            HttpResponseMessage response = await _client.PostAsync("GarageAndSparePartDealerAccount/ResetPassword", Content);

            return response.StatusCode == System.Net.HttpStatusCode.OK ? true : false;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordViewModel changePasswordViewModel)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenManager.GetAccessToken());
            HttpContent content = _clientHelper.CreateHttpContent(changePasswordViewModel);
            var HttpResponse = await _client.PostAsync("GarageAndSparePartDealerAccount/ChangePassword", content);

            if (HttpResponse.IsSuccessStatusCode)
            {
                return true;
            }
            return false;

        }

        public async Task<LoginResponse> LoginAsync(string Phonenumber, string Password, bool RememberMe)
        {
            HttpContent Content = _clientHelper.CreateHttpContent<LoginData>(new LoginData()
            {
                Email =  Phonenumber,
                Password = Password,
                LoginFor = Enum.GetName(typeof(Logins), Logins.Garage)
            });

            HttpResponseMessage response = await _client.PostAsync("GarageAndSparePartDealerAccount/Login", Content);

            LoginResponse APIRes = await _clientHelper.ParseResponseJsonAsync<LoginResponse>(response);

            await SignInAsync(APIRes, RememberMe);
            return APIRes;
        }

        public async Task<LoginResponse> LoginSparePartAsync(string Phonenumber, string Password, bool RememberMe)
        {
            HttpContent Content = _clientHelper.CreateHttpContent<LoginData>(new LoginData()
            {
                PhoneNumber = "971" + Phonenumber,
                Password = Password,
                LoginFor = Enum.GetName(typeof(Logins), Logins.SparePartDealer)
            });

            HttpResponseMessage response = await _client.PostAsync("GarageAndSparePartDealerAccount/Login", Content);

            LoginResponse APIRes = await _clientHelper.ParseResponseJsonAsync<LoginResponse>(response);

            await SignInAsync(APIRes, RememberMe);
            return APIRes;
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
            if (response.Garage != null)
            {
                _sessionManager.SetGarageStore(response.Garage);
            }
            else if (response.SparePartsDealer != null)
            {
                _sessionManager.SetSparePartDealerStore(response.SparePartsDealer);
            }
            else
            {
                _sessionManager.SetUser(authData);
            }

            _logger.LogInformation("Sign In Successfull for user: " + authData.UserName);
        }
    }
}
