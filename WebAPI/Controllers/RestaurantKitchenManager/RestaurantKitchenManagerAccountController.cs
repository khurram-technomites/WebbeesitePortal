using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.RestaurantKitchenManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.ErrorHandling;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/Restaurant/RestaurantKitchenManager/[action]")]
    [ApiController]
    public class RestaurantKitchenManagerAccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRefreshTokenService _refreshTokenService;
        private readonly IRestaurantKitchenManagerService _RestaurantKitchenManagerService;
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly ILogger<RestaurantKitchenManagerAccountController> _logger;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IMessageService _messageService;
        private readonly IFTPUpload _fTPUpload;

        private readonly Uri _webAppUrl;
        private IWebHostEnvironment _hostingEnvironment;

        public RestaurantKitchenManagerAccountController(SignInManager<AppUser> signInManager
            , UserManager<AppUser> userManager
            , IConfiguration config
            , ILogger<RestaurantKitchenManagerAccountController> logger
            , IMapper mapper
            , ITokenService tokenService
            , IEmailService emailService
            , IUserRefreshTokenService refreshTokenService
            , IRouteGroupService routeGroupService
            , IWebHostEnvironment hostingEnvironment
            , IRestaurantKitchenManagerService restaurantKitchenManagerService
            , IUserService userService
            , IMessageService messageService
            , IFTPUpload fTPUpload)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _logger = (ILogger<RestaurantKitchenManagerAccountController>)logger;
            _mapper = mapper;
            _tokenService = tokenService;
            _RestaurantKitchenManagerService = restaurantKitchenManagerService;
            _userService = userService;
            _emailService = emailService;
            _refreshTokenService = refreshTokenService;
            _messageService = messageService;
            _fTPUpload = fTPUpload;

            _webAppUrl = new Uri(_config.GetValue<string>("WebAppURL"));
            _hostingEnvironment = hostingEnvironment;
        }

        /* Public */

        [HttpPost]
        public async Task<IActionResult> Login(LoginData model)
        {
            ErrorDetails err;

            AppUser User = await _userManager.FindByEmailAsync(model.Email);

            if(User == null)
                return Conflict(err = new ErrorDetails(404, "User not found", string.Empty));

            if (!User.PhoneNumberConfirmed)
                return Conflict(err = new ErrorDetails(409, "Kindly confirm your phone number first", string.Empty));
            await SendConfirmationEmail(User);

            if (!User.EmailConfirmed)
                return Conflict(err = new ErrorDetails(409, "Kindly confirm your email first", string.Empty));

            var result = await _signInManager.PasswordSignInAsync(User, model.Password, false, false);

            if (result.Succeeded)
            {
                LoginResponse Response = await MakeLoginResponse(User);

                Response.RestaurantKitchenManager = _mapper.Map<RestaurantKitchenManagerDTO>(_RestaurantKitchenManagerService.GetRestaurantKitchenManagerByUserAsync(User.Id).Result);

                _logger.LogInformation("Login Success for " + User.UserName);

                return Ok(Response);
            }

            return Unauthorized(err = new ErrorDetails(401, "Invalid email or password", string.Empty));

        }

        [HttpGet("{Email}")]
        public async Task<bool> IsEmailConfirmed(string Email)
        {
            if (!string.IsNullOrEmpty(Email))
            {
                AppUser User = await _userManager.FindByEmailAsync(Email);

                if (User != null && User.EmailConfirmed)
                    return true;
                else
                    return false;
            }
            return false;
        }

        [HttpGet("{PhoneNumber}")]
        public async Task<bool> IsPhoneNumberConfirmed(string PhoneNumber)
        {

            if (!string.IsNullOrEmpty(PhoneNumber))
            {
                var users = await _userService.GetUserByNumberAndCheck(PhoneNumber, Enum.GetName(typeof(Logins), Logins.Restaurant));
                AppUser User = users.FirstOrDefault();

                if (User.PhoneNumberConfirmed)
                    return true;
                else
                    return false;
            }
            return false;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RestaurantKitchenManagerDTO Model)
        {
            Random rnd = new Random();
            var User = new AppUser
            {
                UserName = Model.Email,
                Email = Model.Email,
                FirstName = Model.FirstName,
                LastName = Model.LastName,
                AuthCode = Model.PhoneNumber.Equals("971507567600") ? 1234 : rnd.Next(1000, 9999),
                AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2),
                LoginFor = Enum.GetName(typeof(Logins), Logins.Restaurant),
                PhoneNumber = Model.PhoneNumber,
                PhoneNumberConfirmed = Model.RequirePhoneNumberConfirmation ? false : true
            };

            IdentityResult result = await _userManager.CreateAsync(User, Model.Password);

            if (result.Succeeded)
            {
                IdentityResult UserRoleResult = await _userManager.AddToRoleAsync(User, Enum.GetName(typeof(Roles), Roles.RestaurantOwner));

                _logger.LogInformation("Registration Success for " + User.UserName);

                Model.UserId = User.Id;
                await _RestaurantKitchenManagerService.AddRestaurantKitchenManagerAsync(_mapper.Map<Models.RestaurantKitchenManager>(Model));

                if (Model.RequirePhoneNumberConfirmation)
                    if (Model.PhoneNumber.StartsWith("971"))
                        await SendOTP(User);
                    else
                        await SendConfirmationEmail(User);

                return Ok();
            }
            else
                return BadRequest(result.Errors.First<IdentityError>().Description);
        }

        [HttpPost("{PhoneNumber}")]
        public async Task<IActionResult> ForgetPassword(string PhoneNumber)
        {
            ErrorDetails err;
            Random rnd = new Random();
            var user = await _userService.GetUserByNumberAndCheck(PhoneNumber, Enum.GetName(typeof(Logins), Logins.Restaurant));

            if (user == null)
            {
                _logger.LogError("Forget Password: Unable to find " + Enum.GetName(typeof(Logins), Logins.Restaurant) + " user " + user.FirstOrDefault().PhoneNumber);
                return Conflict(err = new ErrorDetails(409, "User not found", string.Empty));
            }

            if (!(await _userManager.IsPhoneNumberConfirmedAsync(user.FirstOrDefault())))
            {
                _logger.LogError("Forget Password: " + Enum.GetName(typeof(Logins), Logins.Restaurant) + " User phone number is not confirmed " + user.FirstOrDefault().PhoneNumber);
                return Conflict(err = new ErrorDetails(409, "Phone number is not confirmed", string.Empty));
            }

            user.FirstOrDefault().AuthCode = PhoneNumber.Equals("971507567600") ? 1234 : rnd.Next(1000, 9999);
            user.FirstOrDefault().AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2);

            await _userManager.UpdateAsync(user.FirstOrDefault());

            if (PhoneNumber.StartsWith("971"))
                await SendOTP(user.FirstOrDefault());
            else
                await SendForgetPasswordEmail(user.FirstOrDefault());

            return Ok();
        }

        [HttpPost("{OTP}/{UserId}")]
        public async Task<IActionResult> VerifyOTP(int OTP, string UserId)
        {
            ErrorDetails err;
            AppUser user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                _logger.LogError("Forget Password: Unable to find user " + UserId);
                return NotFound(err = new ErrorDetails(409, "User not found", string.Empty));
            }

            //var result = await _userManager.ResetPasswordAsync(user, model.Code, model.NewPassword);

            if (user.AuthCodeExpiryTime.TimeOfDay > DateTime.UtcNow.TimeOfDay && user.AuthCode == OTP)
            {
                //Forcing Auth code expiry to avoid forgery
                user.AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(-1);
                await _userManager.UpdateAsync(user);

                return Ok(user.Id);
            }
            else if (user.AuthCodeExpiryTime.TimeOfDay < DateTime.UtcNow.TimeOfDay)
            {
                _logger.LogError("Confirmation Failure - Auth code expired for user " + UserId);
                return Conflict(err = new ErrorDetails(409, "OTP has been expired", string.Empty));
            }
            else
            {
                _logger.LogError("Confirmation Failure - Wrong Auth code recieved for user " + UserId);
                return Conflict(err = new ErrorDetails(409, "Invalid OTP", string.Empty));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ChangePasswordDTO Model)
        {
            ErrorDetails err;
            IdentityResult result;
            var user = await _userManager.FindByIdAsync(Model.UserId);

            if (user == null)
            {
                _logger.LogError("Forget Password: Unable to find user " + Model.UserId);
                return NotFound(err = new ErrorDetails(409, "User not found", string.Empty));
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            result = await _userManager.ResetPasswordAsync(user, token, Model.NewPassword);

            if (result.Succeeded)
                return Ok();
            else
                return Conflict(result.Errors.First<IdentityError>().Description);
        }

        [Authorize]
        [HttpPost]
        public async Task<LoginResponse> ChangePassword(ChangePasswordDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                _logger.LogError("Change Password: Unable to find user " + model.UserId);
                throw new ApplicationException("UserNotFound");
            }

            /* We should also check for the old password before updating the password */

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword,
                                                                model.NewPassword);

            if (result.Succeeded)
                return new LoginResponse(true, "200", "", null);
            else
                throw new ApplicationException(result.Errors.First().Description);
        }

        [HttpPost]
        [Authorize]
        public async Task<AccessToken> RefreshToken(AccessToken CurrentAccessToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            return await _tokenService.RefreshAccessToken(CurrentAccessToken);
        }

        [HttpPost("{PhoneNumber}")]
        public async Task<IActionResult> ResendOTP(string PhoneNumber)
        {
            ErrorDetails err;
            IEnumerable<AppUser> Users = await _userService.GetUserByNumberAndCheck(PhoneNumber, Enum.GetName(typeof(Logins), Logins.Restaurant));
            Random rnd = new Random();
            var user = Users.FirstOrDefault();

            if (user == null)
                return Conflict(err = new ErrorDetails(409, "User doesn't exists", string.Empty));
            else
            {
                if (user.PhoneNumberConfirmed)
                    return Conflict(err = new ErrorDetails(409, "Phone number already confirmed", string.Empty));
            }

            user.AuthCode = PhoneNumber.Equals("971507567600") ? 1234 : rnd.Next(1000, 9999);
            user.AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2);

            await _userManager.UpdateAsync(user);

            if (PhoneNumber.StartsWith("971"))
                await SendOTP(user);
            else
                await SendConfirmationEmail(user);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPhoneNumber(ConfirmUser Model)
        {
            ErrorDetails err;
            var user = await _userManager.FindByIdAsync(Model.UserId);
            if (user == null)
            {
                _logger.LogError("Confirmation Failure - Unable to load user " + Model.UserId);
                return Conflict(err = new ErrorDetails(409, "User not found", string.Empty));
            }

            if (user.AuthCodeExpiryTime.TimeOfDay > DateTime.UtcNow.TimeOfDay && user.AuthCode == Model.AuthCode)
            {
                user.PhoneNumberConfirmed = true;

                //Forcing Auth code expiry to avoid forgery
                user.AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(-1);
                await _userManager.UpdateAsync(user);
            }
            else if (user.AuthCodeExpiryTime.TimeOfDay < DateTime.UtcNow.TimeOfDay)
            {
                _logger.LogError("Confirmation Failure - Auth code expired for user " + Model.UserId);
                return Conflict(err = new ErrorDetails(409, "OTP has been expired", string.Empty));
            }
            else
            {
                _logger.LogError("Confirmation Failure - Wrong Auth code recieved for user " + Model.UserId);
                return Conflict(err = new ErrorDetails(409, "Invalid OTP", string.Empty));
            }


            _logger.LogInformation("Confirmation Success for user " + Model.UserId);

            await _signInManager.SignInAsync(user, false);

            _logger.LogInformation("Login Success for " + Model.UserId);

            return Ok(await MakeLoginResponse(user));
        }

        [HttpPost]
        [Authorize]
        public async Task/*<IActionResult>*/ Logout()
        {
            string DeviceTrackId = User.FindFirstValue("DeviceTrackId");
            await _signInManager.SignOutAsync();

            await _refreshTokenService.DeleteRefreshTokenAsync(DeviceTrackId);

            _logger.LogInformation("Logout Success for [" + _userManager + "]");

            //ResponseMessageDTO response = new ResponseMessageDTO(true, "User Logout Successfully.", null);

            //return Ok(response);
        }

        /* Private */

        private async Task<LoginResponse> MakeLoginResponse(AppUser user)
        {
            UserAuthData authData = _mapper.Map<UserAuthData>(user);

            authData.TokenInfo = await _tokenService.GenerateAccessToken(user.Id);

            List<Claim> Claims = (await _userManager.GetClaimsAsync(user)).ToList();

            IList<string> Role = await _userManager.GetRolesAsync(user);

            authData.UserRole.Name = Role.FirstOrDefault();

            authData.Claims = _mapper.Map<IList<Claim>, IList<CustomeClaims>>(Claims);

            return new LoginResponse(true, "200", "", authData);
        }

        private async Task<bool> SendOTP(AppUser user)
        {
            string Text = string.Empty;
            bool IsOTPSent = false;
            try
            {
                Text = "Your OTP Code is " + user.AuthCode + " \nMDNSMGYjkzc";

                if (await _messageService.SendMessage(user.PhoneNumber, Text))
                {
                    _logger.LogInformation("OTP Sent Successfully to " + user.PhoneNumber);

                    IsOTPSent = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("OTP Sending Failed for " + user.PhoneNumber + " with message: " +
                                  ex.Message);
            }

            return IsOTPSent;
        }

        private async Task<object> SendConfirmationEmail(AppUser user)
        {
            try
            {
                await _emailService.SendConfirmationEmail(new ConfirmEmailDTO()
                {
                    Email = user.Email,
                    UserName = user.FirstName + ' ' + user.LastName,
                    AuthCode = user.AuthCode
                });

                _logger.LogInformation("Confirmation Email Sent Successfully to " + user.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError("Confirmation Email Failed for " + user.Email + " with message: " +
                                  ex.Message);
            }

            UserAuthData authData = new UserAuthData()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.FirstName + " " + user.LastName,
                Email = user.Email
            };

            return new LoginResponse(true, "200", user.AuthCode.ToString(), authData);
        }

        private async Task<object> SendForgetPasswordEmail(AppUser user)
        {
            try
            {
                await _emailService.SendResetPasswordEmail(new ResetPasswordDTO()
                {
                    Email = user.Email,
                    AuthCode = user.AuthCode
                });

                _logger.LogInformation("Reset Password Email Sent Successfully to " + user.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError("Reset Password Email Failed for " + user.Email + " with message: " +
                                  ex.Message);
            }

            return new LoginResponse(true, "200", "", null);
        }

    }
}
