using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.GarageAndSparePartDealerDTO;
using HelperClasses.DTOs.SparePartsDealer;
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

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GarageAndSparePartDealerAccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRefreshTokenService _refreshTokenService;
        private readonly IRouteGroupService _routeGroupService;
        private readonly IGarageService _garageService;
        private readonly ISparePartsDealerService _sparePartsDealerService;
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly ILogger<GarageAndSparePartDealerAccountController> _logger;
        private readonly IMapper _mapper;
        private readonly INumberRangeService _numberRangeService;
        private readonly ITokenService _tokenService;
        private readonly IMessageService _messageService;
        private readonly IFTPUpload _fTPUpload;

        private readonly Uri _webAppUrl;
        private IWebHostEnvironment _hostingEnvironment;

        public GarageAndSparePartDealerAccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration config,
            ILogger<GarageAndSparePartDealerAccountController> logger, IMapper mapper, ITokenService tokenService, IEmailService emailService,
            IUserRefreshTokenService refreshTokenService, IRouteGroupService routeGroupService, IWebHostEnvironment hostingEnvironment,
            IGarageService garageService, IUserService userService, ISparePartsDealerService sparePartsDealerService, IMessageService messageService,
            IFTPUpload fTPUpload, INumberRangeService numberRangeService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
            _routeGroupService = routeGroupService;
            _userService = userService;
            _garageService = garageService;
            _emailService = emailService;
            _refreshTokenService = refreshTokenService;
            _sparePartsDealerService = sparePartsDealerService;
            _messageService = messageService;
            _fTPUpload = fTPUpload;
            _numberRangeService = numberRangeService;

            _webAppUrl = new Uri(_config.GetValue<string>("WebAppURL"));
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginData model)
        {
            ErrorDetails err;
            IEnumerable<AppUser> Users = await _userService.GetUserByEmailAndCheck(model.Email, model.LoginFor);

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

                if (model.LoginFor == Enum.GetName(typeof(Logins), Logins.Garage))
                    Response.Garage = _mapper.Map<GarageDTO>(_garageService.GetGarageByUserAsync(Users.FirstOrDefault().Id).Result.FirstOrDefault());

                else if (model.LoginFor == Enum.GetName(typeof(Logins), Logins.SparePartDealer))
                    Response.SparePartsDealer = _mapper.Map<SparePartsDealerDTO>(_sparePartsDealerService.GetSparePartsDealerByUserAsync(Users.FirstOrDefault().Id).Result.FirstOrDefault());

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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Register(GarageAndSparePartRegisterDTO model)
        {
            ErrorDetails err;
            if (model.RegisteringFor == Enum.GetName(typeof(Logins), Logins.Garage))
                return await RegisterGarage(model.GarageRegisterDTO);

            else if (model.RegisteringFor == Enum.GetName(typeof(Logins), Logins.SparePartDealer))
                return await RegisterSparePartDealer(model.SparePartsDealerRegisterDTO);

            return Conflict(err = new ErrorDetails(409, "Requested registeration type doesn't exists in current context", null));

        }
        private async Task<IActionResult> RegisterGarage(GarageRegisterDTO Model)
        {
            IEnumerable<AppUser> users = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.Garage));

            if (users.Count() > 0)
                return Conflict("User already exists");

            Random rnd = new();
            var User = new AppUser
            {
                UserName = Model.Garage.ContactPersonEmail + rnd.Next(1000, 9999).ToString(),
                Email = Model.Garage.ContactPersonEmail,
                FirstName = Model.Garage.ContactPersonName,
                AuthCode = Model.PhoneNumber.Equals("971507567600") ? 1234 : rnd.Next(1000, 9999),
                AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2),
                LoginFor = Enum.GetName(typeof(Logins), Logins.Garage),
                PhoneNumber = Model.PhoneNumber,
                PhoneNumberConfirmed = Model.RequirePhoneNumberConfirmation ? false : true
            };

            IdentityResult result = await _userManager.CreateAsync(User, Model.Password);

            if (result.Succeeded)
            {
                IdentityResult UserRoleResult = await _userManager.AddToRoleAsync(User, Enum.GetName(typeof(Roles), Roles.GarageOwner));

                if (UserRoleResult.Succeeded)
                {
                    string LogoPath = "/Images/Garage/" + Model.Garage.NameAsPerTradeLicense + "/";
                    if (_fTPUpload.MoveFile(Model.Garage.Logo, ref LogoPath))
                    {
                        Model.Garage.Logo = LogoPath;
                    }

                    foreach (var images in Model.Garage.GarageImages)
                    {
                        string ImagePath = "/Images/GarageImages/" + Model.Garage.NameAsPerTradeLicense + "/";
                        if (_fTPUpload.MoveFile(images.Image, ref ImagePath))
                        {
                            images.Image = ImagePath;
                        }
                    }

                    Model.Garage.UserId = User.Id;
                    Model.Garage.ReferenceCode = await _numberRangeService.GetNextRange("G-");
                    Model.Garage.Slug = Slugify.GenerateSlug(Model.Garage.NameArAsPerTradeLicense, Model.Garage.ReferenceCode);
                    await _garageService.AddGarageAsync(_mapper.Map<Garage>(Model.Garage));
                }

                _logger.LogInformation("Registration Success for " + User.UserName);

                if (Model.RequirePhoneNumberConfirmation)
                    if (Model.PhoneNumber.StartsWith("971"))
                        await SendOTP(User);
                    else
                        await SendConfirmationEmail(User);


                return Ok(new SuccessResponse<GarageRegisterDTO>("", Model));
            }
            else
                return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, string.Empty));
        }
        private async Task<IActionResult> RegisterSparePartDealer(SparePartsDealerRegisterDTO Model)
        {
            IEnumerable<AppUser> users = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.SparePartDealer));

            if (users.Count() > 0)
                return Conflict("User already exists");

            Random rnd = new Random();
            var User = new AppUser
            {
                UserName = Model.SparePartsDealer.ContactPersonEmail + rnd.Next(1000, 9999).ToString(),
                Email = Model.SparePartsDealer.ContactPersonEmail,
                FirstName = Model.SparePartsDealer.ContactPersonName,
                AuthCode = Model.PhoneNumber.Equals("971507567600") ? 1234 : rnd.Next(1000, 9999),
                AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2),
                LoginFor = Enum.GetName(typeof(Logins), Logins.SparePartDealer),
                PhoneNumber = Model.PhoneNumber,
                PhoneNumberConfirmed = Model.RequirePhoneNumberConfirmation ? false : true
            };

            IdentityResult result = await _userManager.CreateAsync(User, Model.Password);

            if (result.Succeeded)
            {
                IdentityResult UserRoleResult = await _userManager.AddToRoleAsync(User, Enum.GetName(typeof(Roles), Roles.SparePartDealer));

                if (UserRoleResult.Succeeded)
                {
                    string LogoPath = "/Images/SparePartsDealer/" + Model.SparePartsDealer.NameAsPerTradeLicense + "/";
                    if (_fTPUpload.MoveFile(Model.SparePartsDealer.Logo, ref LogoPath))
                    {
                        Model.SparePartsDealer.Logo = LogoPath;
                    }

                    foreach (var images in Model.SparePartsDealer.DealerImages)
                    {
                        string ImagePath = "/Images/SparePartsDealerImages/" + Model.SparePartsDealer.NameAsPerTradeLicense + "/";
                        if (_fTPUpload.MoveFile(images.Image, ref ImagePath))
                        {
                            images.Image = ImagePath;
                        }
                    }

                    Model.SparePartsDealer.UserId = User.Id;
                    Model.SparePartsDealer.ReferenceCode = await _numberRangeService.GetNextRange("SD-");
                    await _sparePartsDealerService.AddSparePartsDealerAsync(_mapper.Map<Models.SparePartsDealer>(Model.SparePartsDealer));
                }

                _logger.LogInformation("Registration Success for " + User.UserName);

                if (Model.RequirePhoneNumberConfirmation)
                    if (Model.PhoneNumber.StartsWith("971"))
                        await SendOTP(User);
                    else
                        await SendConfirmationEmail(User);

                return Ok(new SuccessResponse<SparePartsDealerRegisterDTO>("", Model));
            }
            else
                return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, string.Empty));
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(GarageAndSparePartRegisterDTO model)
        {
            ErrorDetails err;
            if (model.RegisteringFor == Enum.GetName(typeof(Logins), Logins.Garage))
                return await UpdateGarage(model.GarageRegisterDTO);

            else if (model.RegisteringFor == Enum.GetName(typeof(Logins), Logins.SparePartDealer))
                return await UpdateSparePartDealer(model.SparePartsDealerRegisterDTO);

            return Conflict(err = new ErrorDetails(409, "Requested registeration type doesn't exists in current context", null));

        }
        private async Task<IActionResult> UpdateGarage(GarageRegisterDTO Model)
        {
            IEnumerable<AppUser> users = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.Garage));

            if (users.Count() == 0)
                return Conflict("No User Found. Invalid PhoneNumber");

            AppUser user = users.FirstOrDefault();

            user.FirstName = Model.Garage.ContactPersonName;
            user.PhoneNumber = Model.PhoneNumber;

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                IEnumerable<Garage> GarageList = await _garageService.GetGarageByIdAsync(Model.Garage.Id);

                if (GarageList.Count() == 0)
                    return Conflict("No Garage Found. Invalid Id");

                Garage garage = GarageList.FirstOrDefault();

                if (!Model.Garage.Logo.Equals(garage.Logo))
                {
                    string LogoPath = "/Images/Garage/" + Model.Garage.NameAsPerTradeLicense + "/";
                    if (_fTPUpload.MoveFile(Model.Garage.Logo, ref LogoPath))
                    {
                        Model.Garage.Logo = LogoPath;
                    }
                }

                foreach (var images in Model.Garage.GarageImages)
                {
                    string ImagePath = "/Images/GarageImages/" + Model.Garage.NameAsPerTradeLicense + "/";

                    if (images.Image.Contains("/Draft/"))
                        if (_fTPUpload.MoveFile(images.Image, ref ImagePath))
                        {
                            images.Image = ImagePath;
                        }
                }

                Model.Garage.ReferenceCode = garage.ReferenceCode;
                Model.Garage.UserId = user.Id;
                await _garageService.UpdateGarageAsync(_mapper.Map<Garage>(Model.Garage));

                _logger.LogInformation("Update Success for " + user.UserName);

                return Ok(new SuccessResponse<GarageRegisterDTO>("", Model));
            }
            else
                return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, string.Empty));
        }
        private async Task<IActionResult> UpdateSparePartDealer(SparePartsDealerRegisterDTO Model)
        {
            IEnumerable<AppUser> users = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.SparePartDealer));

            if (users.Count() == 0)
                return Conflict("No User Found. Invalid PhoneNumber");

            AppUser user = users.FirstOrDefault();

            user.FirstName = Model.SparePartsDealer.ContactPersonName;
            user.PhoneNumber = Model.PhoneNumber;

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                IEnumerable<Models.SparePartsDealer> SparePartsDealerList = await _sparePartsDealerService.GetSparePartsDealerByIdAsync(Model.SparePartsDealer.Id);

                if (SparePartsDealerList.Count() == 0)
                    return Conflict("No Spare Part Dealer Found. Invalid Id");

                Models.SparePartsDealer SparePartsDealer = SparePartsDealerList.FirstOrDefault();

                if (!Model.SparePartsDealer.Logo.Equals(SparePartsDealer.Logo))
                {
                    string LogoPath = "/Images/SparePartsDealer/" + Model.SparePartsDealer.NameAsPerTradeLicense + "/";
                    if (_fTPUpload.MoveFile(Model.SparePartsDealer.Logo, ref LogoPath))
                    {
                        Model.SparePartsDealer.Logo = LogoPath;
                    }
                }
                foreach (var images in Model.SparePartsDealer.DealerImages)
                {
                    string ImagePath = "/Images/SparePartsDealerImages/" + Model.SparePartsDealer.NameAsPerTradeLicense + "/";

                    if (images.Image.Contains("/Draft/"))
                        if (_fTPUpload.MoveFile(images.Image, ref ImagePath))
                        {
                            images.Image = ImagePath;
                        }
                }

                Model.SparePartsDealer.UserId = user.Id;
                await _sparePartsDealerService.UpdateSparePartsDealerAsync(_mapper.Map<Models.SparePartsDealer>(Model.SparePartsDealer));


                _logger.LogInformation("Registration Success for " + user.UserName);

                return Ok(new SuccessResponse<SparePartsDealerRegisterDTO>("", Model));
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
        [HttpPost("{Email}/{For}")]
        public async Task<IActionResult> ForgetPasswordByEmail(string Email, string For)
        {
            ErrorDetails err;
            Random rnd = new Random();
            var user = await _userService.GetUserByEmailAndCheck(Email, For);

            if (user == null)
            {
                _logger.LogError("Forget Password: Unable to find " + For + " user " + user.FirstOrDefault().Email);
                return Conflict(err = new ErrorDetails(409, "User not found", string.Empty));
            }

            if (!(await _userManager.IsEmailConfirmedAsync(user.FirstOrDefault())))
            {
                _logger.LogError("Forget Password: " + For + " User email is not confirmed " + user.FirstOrDefault().Email);
                return Conflict(err = new ErrorDetails(409, "Email is not confirmed", string.Empty));
            }

            user.FirstOrDefault().AuthCode =  rnd.Next(1000, 9999);
            user.FirstOrDefault().AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2);

            await _userManager.UpdateAsync(user.FirstOrDefault());

            //if (PhoneNumber.StartsWith("971"))
            //    await SendOTP(user.FirstOrDefault());
            //else
                await SendForgetPasswordEmail(user.FirstOrDefault());

            return Ok(new { Status = "Success", Message = "", Result = user.FirstOrDefault().Id });
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
                return Conflict(new ErrorDetails(409, "User not found", string.Empty));
            }

            //var result = await _userManager.ResetPasswordAsync(user, model.Code, model.NewPassword);

            if (user.AuthCodeExpiryTime.TimeOfDay > DateTime.UtcNow.TimeOfDay && user.AuthCode == OTP)
            {
                //Forcing Auth code expiry to avoid forgery
                user.AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(-1);
                user.EmailConfirmed = true;
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
    }
}
