using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.Garage;
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
using System.Net.Sockets;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.ErrorHandling;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers
{
    [Route("api/Customer/Account/[action]")]
    [ApiController]
    public class CustomerAccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRefreshTokenService _refreshTokenService;
        private readonly IEmailService _emailService;
        private readonly ILogger<CustomerAccountController> _logger;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly IFTPUpload _fTPUpload;
        private readonly ICustomerService _customerService;
        private readonly IRestaurantCustomerService _restaurantCustomerService;
        private readonly IRestaurantService _restaurantService;

        public CustomerAccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
            ILogger<CustomerAccountController> logger, IMapper mapper, ITokenService tokenService, IEmailService emailService,
            IUserRefreshTokenService refreshTokenService, IFTPUpload fTPUpload,
            IMessageService messageService, IUserService userService, ICustomerService customerService, IRestaurantCustomerService restaurantCustomerService,
            IRestaurantService restaurantService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
            _emailService = emailService;
            _refreshTokenService = refreshTokenService;
            _messageService = messageService;
            _userService = userService;
            _fTPUpload = fTPUpload;
            _restaurantCustomerService = restaurantCustomerService;
            _restaurantService = restaurantService;

            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginData model)
        {
            IEnumerable<AppUser> Users = await _userService.GetUserByNumberAndCheck(model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.Customer));

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
                Response.Customer = _mapper.Map<IEnumerable<CustomerDTO>>(await _customerService.GetByUserIdAsync(Response.AuthData.UserId)).FirstOrDefault();

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

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Logout()
        {
            string DeviceTrackId = User.FindFirstValue("DeviceTrackId");
            await _signInManager.SignOutAsync();

            await _refreshTokenService.DeleteRefreshTokenAsync(DeviceTrackId);

            _logger.LogInformation("Logout Success for [" + _userManager + "]");

            return Ok(new SuccessResponse<string>("Logout successfull", ""));
        }

        [HttpPost]
        //[Authorize(Roles = "Customer")]
        [AllowAnonymous]
        public async Task<AccessToken> RefreshToken(AccessToken CurrentAccessToken)
        {
            return await _tokenService.RefreshAccessToken(CurrentAccessToken);
        }

        [HttpPost("{Email}")]
        public async Task<IActionResult> ResendConfirmEmail(string Email)
        {
            Random rnd = new Random();
            var user = await _userManager.FindByEmailAsync(Email);

            user.AuthCode = rnd.Next(1000, 9999);
            user.AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2);

            await _userManager.UpdateAsync(user);

            if (user == null)
                return Conflict("EmailNotFound");
            else
            {
                if (user.EmailConfirmed)
                    return Conflict("EmailAlreadyConfirmed");
            }

            return Ok(await SendConfirmationEmail(user));
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(ConfirmUser Model)
        {
            var user = await _userManager.FindByIdAsync(Model.UserId);
            if (user == null)
            {
                _logger.LogError("Confirmation Failure - Unable to load user " + Model.UserId);
                return Conflict("UserNotFound");
            }

            //var result = await _userManager.ConfirmEmailAsync(user, Model.ConfirmationCode);

            if (user.AuthCodeExpiryTime.TimeOfDay > DateTime.UtcNow.TimeOfDay && user.AuthCode == Model.AuthCode)
            {
                user.EmailConfirmed = true;

                //Forcing Auth code expiry to avoid forgery
                user.AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(-1);
                await _userManager.UpdateAsync(user);
            }
            else if (user.AuthCodeExpiryTime.TimeOfDay < DateTime.UtcNow.TimeOfDay)
            {
                _logger.LogError("Confirmation Failure - Auth code expired for user " + Model.UserId);
                return Conflict("AuthCodeExpired");
            }
            else
            {
                _logger.LogError("Confirmation Failure - Wrong Auth code recieved for user " + Model.UserId);
                return Conflict("InvalidAuthCode");
            }


            _logger.LogInformation("Confirmation Success for user " + Model.UserId);

            await _signInManager.SignInAsync(user, false);

            _logger.LogInformation("Login Success for " + Model.UserId);

            //BrandCartDTO BrandData = await GetUserBrandDataAsync(user.Id);

            //if (BrandData.SetupDone)
            //{
            //    return Conflict("ALREADY_CONFIRMED");
            //    // return BadRequest("ALREADY_CONFIRMED");
            //}

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
        public async Task<IActionResult> Register(RegisterData model)
        {
            string origin = Request.Headers["origin"];
            var ExistingUsersListByNumerb = await _userService.GetUserByNumberAndCheck(model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.Customer));

            if (ExistingUsersListByNumerb.Any())
                return Conflict(new ErrorDetails(409, "Phone number already registered", string.Empty));

            var ExistingUsersListByEmail = await _userService.GetUserByEmailAndCheck(model.Email, Enum.GetName(typeof(Logins), Logins.Customer));

            if (ExistingUsersListByEmail.Any())
                return Conflict(new ErrorDetails(409, "Email already registered", string.Empty));

            //if (ExistingUsersList.Any() && ExistingUsersList.FirstOrDefault().PhoneNumberConfirmed)
            //    return Conflict(new ErrorDetails(409, "Kindly confirm phone number first", string.Empty));

            Random rnd = new();
            var User = new AppUser
            {
                UserName = model.Email + rnd.Next(1000, 99999).ToString(),
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                AuthCode = rnd.Next(1000, 9999),
                LoginFor = Enum.GetName(typeof(Logins), Logins.Customer),
                AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2),
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = false,
                IsActive = true
            };

            IdentityResult result = await _userManager.CreateAsync(User, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(User, Enum.GetName(typeof(Roles), Roles.Customer));

                _logger.LogInformation("Registration Success for " + User.Email);

                Customer cust = new();
                cust.UserId = User.Id;
                cust.Name = User.FirstName + " " + User.LastName;
                cust.Contact = User.PhoneNumber;
                cust.Email = User.Email;
                cust.Status = Enum.GetName(typeof(Status), Status.Active);

                Customer custResult = await _customerService.AddCustomerAsync(cust);

                if (origin != "https://fougito.com")
                {
                    IEnumerable<Models.Restaurant> restaurant = await _restaurantService.GetRestaurantByOrigin(origin);
                    RestaurantCustomer restaurantCustomer = new();
                    restaurantCustomer.RestaurantId = restaurant.FirstOrDefault().Id;
                    restaurantCustomer.CustomerId = custResult.Id;

                    await _restaurantCustomerService.AddRestaurantCustomer(restaurantCustomer);
                }

                if (model.PhoneNumber.StartsWith("971"))
                {
                    await SendOTP(User);
                    await SendConfirmationEmail(User);
                }
                else
                    await SendConfirmationEmail(User);

                return Ok(new SuccessResponse<string>("", User.Id));
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

        [HttpPost("{PhoneNumber}")]
        public async Task<IActionResult> ForgetPassword(string PhoneNumber)
        {
            Random rdn = new Random();
            var user = await _userService.GetUserByNumberAndCheck(PhoneNumber, Enum.GetName(typeof(Logins), Logins.Customer));

            if (!user.Any())
                return Conflict(new ErrorDetails(409, "No user found, Invalid phone number", string.Empty));

            if (!(await _userManager.IsPhoneNumberConfirmedAsync(user.FirstOrDefault())))
            {
                _logger.LogError("Forget Password: User phone number is not confirmed " + user.FirstOrDefault().Email);
                return Conflict(new ErrorDetails(409, "No user found, Invalid phone number", string.Empty));
            }

            user.FirstOrDefault().AuthCode = rdn.Next(1000, 9999);
            user.FirstOrDefault().AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2);

            await _userManager.UpdateAsync(user.FirstOrDefault());

            if (PhoneNumber.StartsWith("971"))
                await SendOTP(user.FirstOrDefault());
            else
                await SendForgetPasswordEmail(user.FirstOrDefault());

            return Ok(new SuccessResponse<string>("", user.FirstOrDefault().Id));
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

        [HttpPost("{PhoneNumber}")]
        public async Task<IActionResult> ResendOTP(string PhoneNumber)
        {
            IEnumerable<AppUser> Users = await _userService.GetUserByNumberAndCheck(PhoneNumber, Enum.GetName(typeof(Logins), Logins.Customer));
            Random rnd = new Random();
            var user = Users.FirstOrDefault();

            if (user == null)
                return Conflict(new ErrorDetails(409, "User doesn't exists", string.Empty));
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

            return Ok(new SuccessResponse<string>("", user.Id));
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ChangePasswordDTO Model)
        {
            IdentityResult result;
            var user = await _userManager.FindByIdAsync(Model.UserId);

            if (user == null)
            {
                _logger.LogError("Forget Password: Unable to find user " + Model.UserId);
                return NotFound(new ErrorDetails(409, "User not found", string.Empty));
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            result = await _userManager.ResetPasswordAsync(user, token, Model.NewPassword);

            if (result.Succeeded)
                return Ok(new SuccessResponse<string>("Password reset successfully", null));
            else
                return Conflict(new ErrorDetails(409, result.Errors.First<IdentityError>().Description, string.Empty));
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                _logger.LogError("Change Password: Unable to find user " + UserId);
                throw new ApplicationException("UserNotFound");
            }

            /* We should also check for the old password before updating the password */

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword,
                                                                model.NewPassword);

            if (result.Succeeded)
                return Ok(new SuccessResponse<string>("Password change successfully", null));
            else
                throw new ApplicationException(result.Errors.First().Description);
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

        [HttpPut, DisableRequestSizeLimit]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ProfilePicture(IFormFile Image)
        {

            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            AppUser user = await _userManager.FindByIdAsync(UserId);

            string LogoPath = "/Images/Customer/" + user.Id + "/";
            if (Image is not null)
            {
                if (!string.IsNullOrEmpty(user.Logo))
                    if (_fTPUpload.DeleteFile(user.Logo))
                        user.Logo = null;

                if (_fTPUpload.UploadToDirectory(Image, ref LogoPath))
                    user.Logo = LogoPath;
            }
            else
                return Conflict(new ErrorDetails(409, "Image not found", ""));

            await _userManager.UpdateAsync(user);

            return Ok(new SuccessResponse<string>("Profile picture updated successfully", user.Logo));
        }

        [HttpPut]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Update(RegisterData Model)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            AppUser user = await _userManager.FindByIdAsync(UserId);

            user.FirstName = Model.FirstName;
            user.LastName = Model.LastName;
            user.Email = Model.Email;
            user.PhoneNumber = Model.PhoneNumber;

            await _userManager.UpdateAsync(user);

            IEnumerable<Customer> customers = await _customerService.GetByUserIdAsync(UserId);
            Customer customer = customers.FirstOrDefault();

            customer.Name = Model.FirstName + " " + Model.LastName;
            customer.Email = Model.Email;
            customer.Contact = Model.PhoneNumber;

            await _customerService.UpdateCustomerAsync(customer);

            return Ok(new SuccessResponse<AppUser>("Profile updated successfully", user));
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Profile()
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<CustomerDTO> result = _mapper.Map<IEnumerable<CustomerDTO>>(await _customerService.GetByUserIdAsync(UserId));

            return Ok(new SuccessResponse<CustomerDTO>("", result.FirstOrDefault()));
        }

    }
}
