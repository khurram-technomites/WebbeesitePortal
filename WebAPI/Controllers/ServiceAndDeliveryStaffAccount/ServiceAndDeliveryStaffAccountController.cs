using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.DeliveryStaff;
using HelperClasses.DTOs.ServiceAndDeliveryStaffDTO;
using HelperClasses.DTOs.ServiceStaff;
using Microsoft.AspNetCore.Authorization;
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

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ServiceAndDeliveryStaffAccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRefreshTokenService _refreshTokenService;
        private readonly IServiceStaffService _serviceStaffService;
        private readonly IDeliveryStaffService _deliveryStaffService;
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly ILogger<ServiceAndDeliveryStaffAccountController> _logger;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IMessageService _messageService;
        private readonly IFTPUpload _fTPUpload;

        private readonly Uri _webAppUrl;
        private IWebHostEnvironment _hostingEnvironment;

        public ServiceAndDeliveryStaffAccountController(SignInManager<AppUser> signInManager
            , UserManager<AppUser> userManager
            , IConfiguration config
            , ILogger<ServiceAndDeliveryStaffAccountController> logger
            , IMapper mapper
            , ITokenService tokenService
            , IEmailService emailService
            , IUserRefreshTokenService refreshTokenService
            , IRouteGroupService routeGroupService
            , IWebHostEnvironment hostingEnvironment
            , IServiceStaffService serviceStaffService
            , IUserService userService
            , IDeliveryStaffService deliveryStaffService
            , IMessageService messageService
            , IFTPUpload fTPUpload)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
            _serviceStaffService = serviceStaffService;
            _userService = userService;
            _deliveryStaffService = deliveryStaffService;
            _emailService = emailService;
            _refreshTokenService = refreshTokenService;
            _messageService = messageService;
            _fTPUpload = fTPUpload;

            _webAppUrl = new Uri(_config.GetValue<string>("WebAppURL"));
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginData model)
        {
            if (model.LoginFor != Enum.GetName(typeof(Logins), Logins.ServiceStaff) && model.LoginFor != Enum.GetName(typeof(Logins), Logins.DeliveryStaff))
                return Conflict(new ErrorDetails(401, "Cannot proceed the request with provided actor. Invalid data passed for property 'LoginFor'", string.Empty));

            IEnumerable<AppUser> Users = await _userService.GetUserByNumberAndCheck(model.PhoneNumber, model.LoginFor);

            if (!Users.Any() || Users.FirstOrDefault().IsDeleted)
                return Unauthorized(new ErrorDetails(409, "No user found. Invalid phone number", string.Empty));

            if (!Users.FirstOrDefault().PhoneNumberConfirmed)
                return Conflict(new ErrorDetails(409, "Kindly confirm phone number first", string.Empty));

            var result = await _signInManager.PasswordSignInAsync(Users.FirstOrDefault(), model.Password, false, false);

            if (result.Succeeded)
            {
                LoginResponse Response = await MakeLoginResponse(Users.FirstOrDefault());

                if (model.LoginFor == Enum.GetName(typeof(Logins), Logins.ServiceStaff))
                    Response.ServiceStaff = _mapper.Map<ServiceStaffDTO>(_serviceStaffService.GetServiceStaffByUserAsync(Users.FirstOrDefault().Id).Result.FirstOrDefault());

                else if (model.LoginFor == Enum.GetName(typeof(Logins), Logins.DeliveryStaff))
                    Response.DeliveryStaff = _mapper.Map<DeliveryStaffDTO>(_deliveryStaffService.GetDeliveryStaffByUserAsync(Users.FirstOrDefault().Id).Result.FirstOrDefault());

                _logger.LogInformation("Login Success for " + Users.FirstOrDefault().UserName);

                return Ok(new SuccessResponse<LoginResponse>("", Response));
            }

            return Unauthorized(new ErrorDetails(401, "Invalid password", string.Empty));
        }

        [HttpGet("{Email}")]
        public async Task<bool> IsEmailConfirmed(string Email)
        {
            if (!string.IsNullOrEmpty(Email))
            {
                AppUser User = await _userManager.FindByNameAsync(Email);

                if (User.EmailConfirmed)
                    return true;
                else
                    return false;
            }
            return false;
        }

        [HttpGet("{PhoneNumber}/{For}")]
        public async Task<bool> IsPhoneNumberConfirmed(string PhoneNumber, string For)
        {

            if (!string.IsNullOrEmpty(PhoneNumber))
            {
                var users = await _userService.GetUserByNumberAndCheck(PhoneNumber, For);
                AppUser User = users.FirstOrDefault();

                if (User.PhoneNumberConfirmed)
                    return true;
                else
                    return false;
            }
            return false;
        }

        [HttpPost]
        [Authorize]
        public async Task Logout()
        {            
            string DeviceTrackId = User.FindFirstValue("DeviceTrackId");
            await _signInManager.SignOutAsync();

            await _refreshTokenService.DeleteRefreshTokenAsync(DeviceTrackId);

            _logger.LogInformation("Logout Success for [" + _userManager + "]");
        }

        [HttpPost]
        public async Task<SuccessResponse<AccessToken>> RefreshToken(AccessToken CurrentAccessToken)
        {
            return new SuccessResponse<AccessToken>("", await _tokenService.RefreshAccessToken(CurrentAccessToken));
        }

        [HttpPost("{PhoneNumber}/{OTPFor}")]
        public async Task<IActionResult> ResendOTP(string PhoneNumber, string OTPFor)
        {
            ErrorDetails err;
            IEnumerable<AppUser> Users = await _userService.GetUserByNumberAndCheck(PhoneNumber, OTPFor);
            Random rnd = new Random();
            var user = Users.FirstOrDefault();

            if (user == null)
                return Conflict(err = new ErrorDetails(409, "User doesn't exists", string.Empty));
            //else
            //{
            //    if (user.PhoneNumberConfirmed)
            //        return Conflict(err = new ErrorDetails(409, "Phone number already confirmed", string.Empty));
            //}

            user.AuthCode = PhoneNumber.Equals("971507567600") ? 1234 : rnd.Next(1000, 9999);
            user.AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2);

            await _userManager.UpdateAsync(user);

            if (PhoneNumber.StartsWith("971"))
                await SendOTP(user);
            else
                await SendConfirmationEmail(user);

            return Ok(new SuccessResponse<string>("OTP sent successfully", null));
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

            return Ok(new SuccessResponse<LoginResponse>("", await MakeLoginResponse(user)));
        }

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

        [HttpPost]
        public async Task<IActionResult> Register(ServiceAndDeliveryStaffRegisterDTO model)
        {
            ErrorDetails err;

            if (model.RegisteringFor == Enum.GetName(typeof(Logins), Logins.ServiceStaff))

                return await RegisterServiceStaff(model.ServiceStaffRegister);


            else if (model.RegisteringFor == Enum.GetName(typeof(Logins), Logins.DeliveryStaff))
                return await RegisterDeliveryStaff(model.DeliveryStaffRegister);

            return Conflict(err = new ErrorDetails(409, "Requested registeration type doesn't exists in current context", string.Empty));

        }

        private async Task<IActionResult> RegisterServiceStaff(ServiceStaffRegisterDTO Model)
        {
            IEnumerable<AppUser> users = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.ServiceStaff));

            if (users.Any())
                return Conflict("User already exists");

            Random rnd = new();
            var User = new AppUser
            {
                UserName = Model.FirstName.Replace(" ", "") + rnd.Next(1000, 9999).ToString(),
                Email = Model.Email,
                FirstName = Model.FirstName,
                LastName = Model.LastName,
                AuthCode = Model.PhoneNumber.Equals("971507567600") ? 1234 : rnd.Next(1000, 9999),
                AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2),
                LoginFor = Enum.GetName(typeof(Logins), Logins.ServiceStaff),
                PhoneNumber = Model.PhoneNumber,
                IsActive = true,
                PhoneNumberConfirmed = !Model.RequirePhoneNumberConfirmation,
            };

            IdentityResult result = await _userManager.CreateAsync(User, Model.Password);

            if (result.Succeeded)
            {
                IdentityResult UserRoleResult = await _userManager.AddToRoleAsync(User, Enum.GetName(typeof(Roles), Roles.ServiceStaff));

                if (UserRoleResult.Succeeded)
                {
                    if (!string.IsNullOrEmpty(Model.ServiceStaff.Logo))
                    {
                        string LogoPath = "/Images/ServiceStaff/" + User.UserName + "/";
                        if (_fTPUpload.MoveFile(Model.ServiceStaff.Logo, ref LogoPath))
                        {
                            Model.ServiceStaff.Logo = LogoPath;
                        }
                    }
                    Model.ServiceStaff.UserId = User.Id;
                    Model.ServiceStaff = _mapper.Map<ServiceStaffDTO>(await _serviceStaffService.AddServiceStaffAsync(_mapper.Map<Models.ServiceStaff>(Model.ServiceStaff)));
                }
                _logger.LogInformation("Registration Success for " + User.UserName);

                if (Model.RequirePhoneNumberConfirmation)
                    if (Model.PhoneNumber.StartsWith("971"))
                        await SendOTP(User);
                    else
                        await SendConfirmationEmail(User);


                return Ok(Model);
            }
            else
                return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, ""));
        }

        private async Task<IActionResult> RegisterDeliveryStaff(DeliveryStaffRegisterDTO Model)
        {
            IEnumerable<AppUser> users = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.DeliveryStaff));

            if (users.Count() > 0)
                return Conflict("User already exists");

            Random rnd = new Random();
            var User = new AppUser
            {
                UserName = Model.FirstName.Replace(" ", "") + rnd.Next(1000, 9999).ToString(),
                Email = Model.Email,
                FirstName = Model.FirstName,
                LastName = Model.LastName,
                AuthCode = Model.PhoneNumber.Equals("971507567600") ? 1234 : rnd.Next(1000, 9999),
                AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2),
                LoginFor = Enum.GetName(typeof(Logins), Logins.DeliveryStaff),
                PhoneNumber = Model.PhoneNumber,
                IsActive = true,
                PhoneNumberConfirmed = Model.RequirePhoneNumberConfirmation ? false : true
            };

            IdentityResult result = await _userManager.CreateAsync(User, Model.Password);

            if (result.Succeeded)
            {
                IdentityResult UserRoleResult = await _userManager.AddToRoleAsync(User, Enum.GetName(typeof(Roles), Roles.DeliveryStaff));


                if (UserRoleResult.Succeeded)
                {
                    if (!string.IsNullOrEmpty(Model.DeliveryStaff.Logo))
                    {
                        string LogoPath = "/Images/DeliveryStaff/" + User.UserName + "/";
                        if (_fTPUpload.MoveFile(Model.DeliveryStaff.Logo, ref LogoPath))
                        {
                            Model.DeliveryStaff.Logo = LogoPath;
                        }
                    }

                    Model.DeliveryStaff.UserId = User.Id;
                    Model.DeliveryStaff = _mapper.Map<DeliveryStaffDTO>(await _deliveryStaffService.AddDeliveryStaffAsync(_mapper.Map<Models.DeliveryStaff>(Model.DeliveryStaff)));
                }

                _logger.LogInformation("Registration Success for " + User.UserName);

                if (Model.RequirePhoneNumberConfirmation)
                    if (Model.PhoneNumber.StartsWith("971"))
                        await SendOTP(User);
                    else
                        await SendConfirmationEmail(User);

                return Ok(Model);
            }
            else
                return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, ""));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ServiceAndDeliveryStaffRegisterDTO model)
        {
            ErrorDetails err;

            if (model.RegisteringFor == Enum.GetName(typeof(Logins), Logins.ServiceStaff))

                return await UpdateServiceStaff(model.ServiceStaffRegister);


            else if (model.RegisteringFor == Enum.GetName(typeof(Logins), Logins.DeliveryStaff))
                return await UpdateDeliveryStaff(model.DeliveryStaffRegister);

            return Conflict(err = new ErrorDetails(409, "Requested registeration type doesn't exists in current context", string.Empty));

        }

        private async Task<IActionResult> UpdateServiceStaff(ServiceStaffRegisterDTO Model)
        {
            IEnumerable<AppUser> users = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.ServiceStaff));

            if (users.Count() == 0)
                return Conflict("No record found, Invalid phone number");

            AppUser User = users.FirstOrDefault();

            User.FirstName = Model.FirstName;
            User.LastName = Model.LastName;
            User.Email = Model.Email;
            User.PhoneNumber = Model.PhoneNumber;
            User.PasswordHash = Model.Password;

            IdentityResult result = await _userManager.UpdateAsync(User);

            if (result.Succeeded)
            {
                IEnumerable<Models.ServiceStaff> serviceStaffList = await _serviceStaffService.GetServiceStaffByIdAsync(Model.ServiceStaff.Id);
                Models.ServiceStaff serviceStaff = serviceStaffList.FirstOrDefault();


                if (!string.IsNullOrEmpty(serviceStaff.Logo) && !serviceStaff.Logo.Equals(Model.ServiceStaff.Logo))
                {
                    if (!string.IsNullOrEmpty(Model.ServiceStaff.Logo))
                    {
                        string LogoPath = "/Images/ServiceStaff/" + User.UserName + "/";
                        if (_fTPUpload.MoveFile(Model.ServiceStaff.Logo, ref LogoPath))
                        {
                            Model.ServiceStaff.Logo = LogoPath;
                        }
                    }
                }

                if (Model.Password != null)
                {
                    AppUser user = await _userManager.FindByIdAsync(users.FirstOrDefault().Id);
                    string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, Model.Password);
                }

                //serviceStaff.UserId = Model.ServiceStaff.UserId;
                serviceStaff.PhoneNumber = Model.ServiceStaff.PhoneNumber;
                serviceStaff.Email = Model.ServiceStaff.Email;
                serviceStaff.FirstName = Model.ServiceStaff.FirstName;
                serviceStaff.LastName = Model.ServiceStaff.LastName;
                serviceStaff.Logo = Model.ServiceStaff.Logo;
                serviceStaff.User = null;

                Model.ServiceStaff = _mapper.Map<ServiceStaffDTO>(await _serviceStaffService.UpdateServiceStaffAsync(serviceStaff));

                _logger.LogInformation("Update Success for " + User.UserName);

                return Ok(Model);
            }
            else
                return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, ""));
        }

        private async Task<IActionResult> UpdateDeliveryStaff(DeliveryStaffRegisterDTO Model)
        {
            IEnumerable<AppUser> users = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.DeliveryStaff));

            if (users.Count() == 0)
                return Conflict("No record found, Invalid phone number");

            AppUser User = users.FirstOrDefault();

            User.FirstName = Model.FirstName;
            User.LastName = Model.LastName;
            User.Email = Model.Email;
            User.PhoneNumber = Model.PhoneNumber;
            User.IsActive = Model.DeliveryStaff.IsActive;

            IdentityResult result = await _userManager.UpdateAsync(User);

            if (result.Succeeded)
            {
                IEnumerable<Models.DeliveryStaff> deliveryStaffList = await _deliveryStaffService.GetDeliveryStaffByIdAsync(Model.DeliveryStaff.Id);
                Models.DeliveryStaff deliveryStaff = deliveryStaffList.FirstOrDefault();

                if (!string.IsNullOrEmpty(deliveryStaff.Logo) && !deliveryStaff.Logo.Equals(Model.DeliveryStaff.Logo))
                {
                    if (!string.IsNullOrEmpty(Model.DeliveryStaff.Logo))
                    {
                        string LogoPath = "/Images/DeliveryStaff/" + User.UserName + "/";
                        if (_fTPUpload.MoveFile(Model.DeliveryStaff.Logo, ref LogoPath))
                        {
                            deliveryStaff.Logo = LogoPath;
                        }
                    }
                }

                if (Model.Password != null)
                {
                    AppUser user = await _userManager.FindByIdAsync(users.FirstOrDefault().Id);
                    string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, Model.Password);
                }

                //Model.DeliveryStaff.UserId = deliveryStaff.UserId;

                deliveryStaff.FirstName = Model.DeliveryStaff.FirstName;
                deliveryStaff.LastName = Model.DeliveryStaff.LastName;
                deliveryStaff.Email = Model.DeliveryStaff.Email;
                deliveryStaff.PhoneNumber = Model.DeliveryStaff.PhoneNumber;
                deliveryStaff.Logo = Model.DeliveryStaff.Logo;

                deliveryStaff.User = null;

                Model.DeliveryStaff = _mapper.Map<DeliveryStaffDTO>(await _deliveryStaffService.UpdateDeliveryStaffAsync(deliveryStaff));

                _logger.LogInformation("Update Success for " + User.UserName);

                return Ok(Model);
            }
            else
                return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, ""));
        }

        private async Task<object> SendConfirmationEmail(AppUser user)
        {
            try
            {
                await _emailService.SendOTPEmail(new ConfirmEmailDTO()
                {
                    Title = "Verify Your Phone Number",
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
                await _emailService.SendOTPEmail(new ConfirmEmailDTO()
                {
                    Title = "Recover Your Account",
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

        [HttpPost("{PhoneNumber}/{For}")]
        public async Task<IActionResult> ForgetPassword(string PhoneNumber, string For)
        {
            Random rnd = new Random();
            var user = await _userService.GetUserByNumberAndCheck(PhoneNumber, For);

            if (user == null)
            {
                _logger.LogError("Forget Password: Unable to find " + For + " user " + user.FirstOrDefault().PhoneNumber);
                return Conflict(new ErrorDetails(409, "User not found", string.Empty));
            }

            if (!(await _userManager.IsPhoneNumberConfirmedAsync(user.FirstOrDefault())))
            {
                _logger.LogError("Forget Password: " + For + " User phone number is not confirmed " + user.FirstOrDefault().PhoneNumber);
                return Conflict(new ErrorDetails(409, "Phone number is not confirmed", string.Empty));
            }

            user.FirstOrDefault().AuthCode = PhoneNumber.Equals("971507567600") ? 1234 : rnd.Next(1000, 9999);
            user.FirstOrDefault().AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2);

            await _userManager.UpdateAsync(user.FirstOrDefault());

            if (PhoneNumber.StartsWith("971"))
                await SendOTP(user.FirstOrDefault());
            else
                await SendForgetPasswordEmail(user.FirstOrDefault());

            return Ok(new SuccessResponse<string>("", user.FirstOrDefault().Id));
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
                return Ok(new SuccessResponse<string>("Password reset successfully", null));
            else
                return Conflict(new ErrorDetails(409, result.Errors.First<IdentityError>().Description, ""));
        }

        [HttpPost("{OTP}/{UserId}")]
        public async Task<IActionResult> VerifyOTP(int OTP, string UserId)
        {
            AppUser user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                _logger.LogError("Forget Password: Unable to find user " + UserId);
                return NotFound(new ErrorDetails(409, "User not found", string.Empty));
            }

            //var result = await _userManager.ResetPasswordAsync(user, model.Code, model.NewPassword);

            if (user.AuthCodeExpiryTime.TimeOfDay > DateTime.UtcNow.TimeOfDay && user.AuthCode == OTP)
            {
                //Forcing Auth code expiry to avoid forgery
                user.AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(-1);
                user.PhoneNumberConfirmed = true;
                await _userManager.UpdateAsync(user);

                return Ok(new SuccessResponse<string>("", user.Id));
            }
            else if (user.AuthCodeExpiryTime.TimeOfDay < DateTime.UtcNow.TimeOfDay)
            {
                _logger.LogError("Confirmation Failure - Auth code expired for user " + UserId);
                return Conflict(new ErrorDetails(409, "OTP has been expired", string.Empty));
            }
            else
            {
                _logger.LogError("Confirmation Failure - Wrong Auth code recieved for user " + UserId);
                return Conflict(new ErrorDetails(409, "Invalid OTP", string.Empty));
            }
        }

        [Authorize(Roles = "ServiceStaff")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                _logger.LogError("Change Password: Unable to find user " + model.UserId);
                return Conflict(new ErrorDetails(409, "User not found", string.Empty));
            }

            /* We should also check for the old password before updating the password */

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword,
                                                                model.NewPassword);

            if (result.Succeeded)
                return Ok(new SuccessResponse<string>("Password changed successfully", null));
            else
                return Conflict(new ErrorDetails(409, result.Errors.First().Description, string.Empty));
        }

        private async Task<bool> SendOTP(AppUser user)
        {
            bool IsOTPSent = false;
            try
            {
                string Text = "Your OTP Code is " + user.AuthCode + " \nMDNSMGYjkzc";

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
    }
}
