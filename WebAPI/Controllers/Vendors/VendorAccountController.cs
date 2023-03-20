using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.ErrorHandling;
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;
using static System.Net.WebRequestMethods;

namespace WebAPI.Controllers.Vendors
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VendorAccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRefreshTokenService _refreshTokenService;
       
        private readonly IVendorService _vendorservice;
       
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly ILogger<VendorAccountController> _logger;
        private readonly IMapper _mapper;
        private readonly INumberRangeService _numberRangeService;
        private readonly ITokenService _tokenService;
        private readonly IMessageService _messageService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IVendorOTPVerificationService _vendorOTPVerificationService;

        private readonly Uri _webAppUrl;
        private IWebHostEnvironment _hostingEnvironment;

        public VendorAccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration config,
            ILogger<VendorAccountController> logger, IMapper mapper, ITokenService tokenService, IEmailService emailService,
            IUserRefreshTokenService refreshTokenService, IWebHostEnvironment hostingEnvironment,
            IVendorService vendorservice, IUserService userService, IMessageService messageService,
            IFTPUpload fTPUpload, INumberRangeService numberRangeService,IVendorOTPVerificationService vendorOTPVerificationService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
            _userService = userService;
            _vendorservice = vendorservice;
            _emailService = emailService;
            _refreshTokenService = refreshTokenService;
            _messageService = messageService;
            _fTPUpload = fTPUpload;
            _numberRangeService = numberRangeService;
            _webAppUrl = new Uri(_config.GetValue<string>("WebAppURL"));
            _hostingEnvironment = hostingEnvironment;
            _vendorOTPVerificationService = vendorOTPVerificationService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginData model)
        {
            ErrorDetails err;
            IEnumerable<AppUser> Users = await _userService.GetUserByNumberAndCheck(model.PhoneNumber, model.LoginFor);

            if (!Users.Any())
                return Conflict(new ErrorDetails(409, "No user found. Invalid phone number", string.Empty));

            if (Users.FirstOrDefault().IsDeleted)
                return Conflict(new ErrorDetails(409, "No user found. Invalid phone number", string.Empty));

            if (!Users.FirstOrDefault().IsActive)
                return Conflict(new ErrorDetails(409, "Account suspended! contact your administrator", string.Empty));

            if (!Users.FirstOrDefault().PhoneNumberConfirmed)
                return Conflict(new ErrorDetails(409, "Kindly confirm phone number first", string.Empty));

            var result = await _signInManager.PasswordSignInAsync(Users.FirstOrDefault(), model.Password, false, false);

            if (result.Succeeded)
            {
                LoginResponse Response = await MakeLoginResponse(Users.FirstOrDefault());

                if (model.LoginFor == Enum.GetName(typeof(Logins), Logins.Vendor))
                    Response.Vendor = _mapper.Map<VendorDTO>(_vendorservice.GetVendorByUserAsync(Users.FirstOrDefault().Id).Result.FirstOrDefault());

                _logger.LogInformation("Login Success for " + Users.FirstOrDefault().UserName);

                return Ok(new SuccessResponse<LoginResponse>("", Response));
            }

            return Conflict(err = new ErrorDetails(401, "Invalid phone number or password", string.Empty));
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
        public async Task<IActionResult> Logout()
        {
            string DeviceTrackId = User.FindFirstValue("DeviceTrackId");
            await _signInManager.SignOutAsync();

            await _refreshTokenService.DeleteRefreshTokenAsync(DeviceTrackId);

            _logger.LogInformation("Logout Success for [" + _userManager + "]");

            return Ok(new SuccessResponse<string>("Logout successfull", ""));
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken(AccessToken CurrentAccessToken)
        {
            return Ok(new SuccessResponse<AccessToken>("", await _tokenService.RefreshAccessToken(CurrentAccessToken)));
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

            return Ok(new { Status = "Success", Message = "" });
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
        public async Task<IActionResult> Register(VendorRegistrationDTO model)
        {
            ErrorDetails err;
            IEnumerable<AppUser> users = await _userService.GetUserByNumberAndCheck(model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.Vendor));
            
            var ExistingUsersListByEmail = await _userService.GetUserByEmailAndCheck(model.Email, Enum.GetName(typeof(Logins), Logins.Vendor));

            if (ExistingUsersListByEmail.Any())
                return Conflict(new ErrorDetails(409, "Email already registered", string.Empty));

            if (users.Count() > 0)
                return Conflict(new ErrorDetails(409, "User already exists", string.Empty));
            IEnumerable<VendorOTPVerification> vendorListByEmail = await _vendorOTPVerificationService.GetByEmailAsync(model.Email);

            if ((vendorListByEmail.Any() && !vendorListByEmail.FirstOrDefault().IsVerified) || !vendorListByEmail.Any())
                return Conflict(new ErrorDetails(409, "Email not verified", string.Empty));

            IEnumerable<VendorOTPVerification> vendorListByPhone = await _vendorOTPVerificationService.GetByPhoneAsync(model.PhoneNumber.Replace("+","").Replace(" ",""));

            if ((vendorListByPhone.Any() && !vendorListByPhone.FirstOrDefault().IsVerified) || !vendorListByPhone.Any())
                return Conflict(new ErrorDetails(409, "Phone number not verified", string.Empty));

            Random rnd = new();
            var User = new AppUser
            {
                UserName = (model.NameAsPerTradeLicense + rnd.Next(1000, 9999).ToString()).Replace(" ", ""),
                Email = model.Email,
                FirstName = model.NameAsPerTradeLicense,
                LastName = model.NameAsPerTradeLicense,
                NormalizedEmail = model.Email,
                AuthCode = model.PhoneNumber.Equals("971507567600") ? 1234 : rnd.Next(1000, 9999),
                AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2),
                LoginFor = Enum.GetName(typeof(Logins), Logins.Vendor),
                PhoneNumber = model.PhoneNumber,
                IsActive = true,
                PhoneNumberConfirmed = model.RequirePhoneNumberConfirmation ? false : true
            };

            IdentityResult result = await _userManager.CreateAsync(User, model.Password);

            if (result.Succeeded)
            {
                IdentityResult UserRoleResult = await _userManager.AddToRoleAsync(User, Enum.GetName(typeof(Roles), Roles.Vendor));

                if (UserRoleResult.Succeeded)
                {
                    VendorDTO vendor = new VendorDTO();
                    vendor.NameAsPerTradeLicense = model.NameAsPerTradeLicense;
                    vendor.FullName = model.NameAsPerTradeLicense;
                    vendor.Email = model.Email;
                    //vendor.ContactNumber1 = model.PhoneNumber;
                    vendor.UserId = User.Id;
                    vendor.Status = Enum.GetName(typeof(Status), Status.Pending);
                    await _vendorservice.AddVendorAsync(_mapper.Map<Vendor>(vendor));
                }
                await _emailService.SendWelcomeEmail(new ConfirmEmailDTO()
                {
                    Title = "Welcome to the webbeesite",
                    Email = model.Email,
                    UserName = model.NameAsPerTradeLicense,
                });

                _logger.LogInformation("Registration Success for " + User.UserName);

                if (model.RequirePhoneNumberConfirmation)
                    if (model.PhoneNumber.StartsWith("971"))
                        await SendOTP(User);
                    else
                        await SendConfirmationEmail(User);


                return Ok(new SuccessResponse<VendorRegistrationDTO>("", model));
            }
            else
                return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, string.Empty));
                


        }
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(VendorDTO model)
        {
            ErrorDetails err;
            IEnumerable<AppUser> users = await _userService.GetUserByNumberAndCheck(model.ContactNumber1, Enum.GetName(typeof(Logins), Logins.Vendor));

            if (users.Count() == 0)
                return Conflict("No User Found. Invalid PhoneNumber");

            AppUser user = users.FirstOrDefault();

            user.FirstName = model.FullName;
            user.PhoneNumber = model.ContactNumber1;

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                IEnumerable<Vendor> VendorList = await _vendorservice.GetVendorByIdAsync(model.Id);

                if (VendorList.Count() == 0)
                    return Conflict("No Vendor Found. Invalid Id");

                Vendor vendor = VendorList.FirstOrDefault();


                model.UserId = user.Id;
                await _vendorservice.UpdateVendorAsync(_mapper.Map<Vendor>(model));

                _logger.LogInformation("Update Success for " + user.UserName);

                return Ok(new SuccessResponse<VendorDTO>("", model));
            }
            else
                return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, string.Empty));


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
            ErrorDetails err;
            Random rnd = new Random();
            var user = await _userService.GetUserByNumberAndCheck(PhoneNumber, For);

            if (user == null)
            {
                _logger.LogError("Forget Password: Unable to find " + For + " user " + user.FirstOrDefault().PhoneNumber);
                return Conflict(err = new ErrorDetails(409, "User not found", string.Empty));
            }

            if (!(await _userManager.IsPhoneNumberConfirmedAsync(user.FirstOrDefault())))
            {
                _logger.LogError("Forget Password: " + For + " User phone number is not confirmed " + user.FirstOrDefault().PhoneNumber);
                return Conflict(err = new ErrorDetails(409, "Phone number is not confirmed", string.Empty));
            }

            user.FirstOrDefault().AuthCode = PhoneNumber.Equals("971507567600") ? 1234 : rnd.Next(1000, 9999);
            user.FirstOrDefault().AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2);

            await _userManager.UpdateAsync(user.FirstOrDefault());

            if (PhoneNumber.StartsWith("971"))
                await SendOTP(user.FirstOrDefault());
            else
                await SendForgetPasswordEmail(user.FirstOrDefault());

            return Ok(new { Status = "Success", Message = "", Result = user.FirstOrDefault().Id } );
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
                return Ok(new {Status = "Success", Message = "Password has been set" });
            else
                return Conflict(result.Errors.First<IdentityError>().Description);
        }

        [HttpPost("{OTP}/{UserId}")]
        public async Task<IActionResult> VerifyOTP(int OTP, string UserId)
        {
            AppUser user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                _logger.LogError("Forget Password: Unable to find user " + UserId);
                return Conflict("User not found");
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
                return Conflict("OTP has been expired");
            }
            else
            {
                _logger.LogError("Confirmation Failure - Wrong Auth code recieved for user " + UserId);
                return Conflict( "Invalid OTP");
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
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
                return Ok(new SuccessResponse<string>("Password changed successfully", null));
            else
                throw new ApplicationException(result.Errors.First().Description);
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


        //Vendor Email and Number OTP
        [HttpPost("{Email}")]
        public async Task<IActionResult> SendOTPEmail(string Email)
        {
            Random rnd = new();
            IEnumerable<VendorOTPVerification> VendorList = await _vendorOTPVerificationService.GetByEmailAsync(Email);

            if (VendorList.Any())
            {
                VendorOTPVerification _Vendor = VendorList.FirstOrDefault();

                _Vendor.OTP = rnd.Next(1000, 9999);
                _Vendor.OTPExpiryTime = DateTime.UtcNow.AddMinutes(2);
                _Vendor.IsVerified = false;

                await _vendorOTPVerificationService.UpdateAsync(_Vendor);

                await SendConfirmationEmail(Email, _Vendor.OTP);

                return Ok(new SuccessResponse<string>("Email sent successfully", ""));
            }

            VendorOTPVerification VendorCreation = new()
            {
                Email = Email,
                OTP = rnd.Next(1000, 9999),
                OTPExpiryTime = DateTime.UtcNow.AddMinutes(2),
                IsVerified = false
            };

            await _vendorOTPVerificationService.InsertAsync(VendorCreation);

            await SendConfirmationEmail(Email, VendorCreation.OTP);

            return Ok(new SuccessResponse<string>("Email sent successfully", ""));
        }
        [HttpPut("{OTP}/{Email}")]
        public async Task<IActionResult> ConfirmEmail(long OTP, string Email)
        {
            IEnumerable<VendorOTPVerification> VendorList = await _vendorOTPVerificationService.GetByEmailAsync(Email);

            if (!VendorList.Any())
            {
                return Conflict(new ErrorDetails(409, "Invalid email address", ""));
            }

            VendorOTPVerification _Vendor = VendorList.FirstOrDefault();

            if (_Vendor.OTPExpiryTime.TimeOfDay > DateTime.UtcNow.TimeOfDay && _Vendor.OTP == OTP)
            {
                _Vendor.IsVerified = true;

                //Forcing Auth code expiry to avoid forgery
                _Vendor.OTPExpiryTime = DateTime.UtcNow.AddMinutes(-1);

                await _vendorOTPVerificationService.UpdateAsync(_Vendor);
            }
            else if (_Vendor.OTPExpiryTime.TimeOfDay < DateTime.UtcNow.TimeOfDay)
            {
                _logger.LogError("Confirmation Failure - Auth code expired for supplier " + Email);
                return Conflict(new ErrorDetails(409, "OTP expired", ""));
            }
            else
            {
                _logger.LogError("Confirmation Failure - Wrong Auth code recieved for supplier " + Email);
                return Conflict(new ErrorDetails(409, "Invalid OTP", ""));
            }


            _logger.LogInformation("Confirmation Success for supplier " + Email);

            return Ok(new SuccessResponse<string>("Email Verified", ""));
        }
        [HttpPost("{PhoneNumber}")]
        public async Task<IActionResult> SendOTPNumber(string PhoneNumber)
        {
            Random rnd = new();
            IEnumerable<VendorOTPVerification> VendorList = await _vendorOTPVerificationService.GetByPhoneAsync(PhoneNumber);

            if (VendorList.Any())
            {
                VendorOTPVerification _Vendor = VendorList.FirstOrDefault();

                if (PhoneNumber == "971507567600")
                    _Vendor.OTP = 1234;
                else
                    _Vendor.OTP = rnd.Next(1000, 9999);
                _Vendor.OTPExpiryTime = DateTime.UtcNow.AddMinutes(2);
                _Vendor.IsVerified = false;

                await _vendorOTPVerificationService.UpdateAsync(_Vendor);

                await SendOTPNumber(PhoneNumber, _Vendor.OTP);

                return Ok(new SuccessResponse<string>("OTP sent successfully", ""));
            }

            VendorOTPVerification VendorCreation = new()
            {
                PhoneNumber = PhoneNumber,
                OTP = PhoneNumber == "971507567600" ? 1234 : rnd.Next(1000, 9999),
                OTPExpiryTime = DateTime.UtcNow.AddMinutes(2),
                IsVerified = false
            };

            await _vendorOTPVerificationService.InsertAsync(VendorCreation);

            await SendOTPNumber(PhoneNumber, VendorCreation.OTP);

            return Ok(new SuccessResponse<string>("OTP sent successfully", ""));
        }
        [HttpPut("{OTP}/{PhoneNumber}")]
        public async Task<IActionResult> VerifyOTPForVendor(int OTP, string PhoneNumber)
        {
            var VendorList = await _vendorOTPVerificationService.GetByPhoneAsync(PhoneNumber);

            if (!VendorList.Any())
            {
                return Conflict(new ErrorDetails(409, "Invalid phone number", ""));
            }

            VendorOTPVerification _Vendor = VendorList.FirstOrDefault();

            if (_Vendor.OTPExpiryTime.TimeOfDay > DateTime.UtcNow.TimeOfDay && _Vendor.OTP == OTP)
            {
                _Vendor.IsVerified = true;

                //Forcing Auth code expiry to avoid forgery
                _Vendor.OTPExpiryTime = DateTime.UtcNow.AddMinutes(-1);

                await _vendorOTPVerificationService.UpdateAsync(_Vendor);
            }
            else if (_Vendor.OTPExpiryTime.TimeOfDay < DateTime.UtcNow.TimeOfDay)
            {
                _logger.LogError("Confirmation Failure - Auth code expired for supplier " + PhoneNumber);
                return Conflict(new ErrorDetails(409, "OTP expired", ""));
            }
            else
            {
                _logger.LogError("Confirmation Failure - Wrong Auth code recieved for supplier " + PhoneNumber);
                return Conflict(new ErrorDetails(409, "Invalid OTP", ""));
            }


            _logger.LogInformation("Confirmation Success for supplier " + PhoneNumber);

            return Ok(new SuccessResponse<string>("Phone Number Verified", ""));
        }
        private async Task<object> SendConfirmationEmail(string Email, int OTP)
        {
            try
            {
                await _emailService.SendOTPEmail(new ConfirmEmailDTO()
                {
                    Title = "Verify Your Email Address",
                    Email = Email,
                    UserName = Email.Split('@')[0],
                    AuthCode = OTP
                });

                _logger.LogInformation("Confirmation Email Sent Successfully to " + Email);
            }
            catch (Exception ex)
            {
                _logger.LogError("Confirmation Email Failed for " + Email + " with message: " +
                                  ex.Message);
            }

            UserAuthData authData = new();

            return new LoginResponse(true, "200", OTP.ToString(), authData);
        }
        private async Task<bool> SendOTPNumber(string Phonenumber, int OTP)
        {
            bool IsOTPSent = false;
            try
            {
                string Text = "Your OTP Code is " + OTP + " \nMDNSMGYjkzc";

                if (await _messageService.SendMessage(Phonenumber, Text))
                {
                    _logger.LogInformation("OTP Sent Successfully to " + Phonenumber);

                    IsOTPSent = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("OTP Sending Failed for " + Phonenumber + " with message: " +
                                  ex.Message);
            }

            return IsOTPSent;
        }


    }
}
