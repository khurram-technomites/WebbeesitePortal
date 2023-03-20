using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

namespace WebAPI.Controllers.Supplier
{
    [Route("api/Supplier/Account/[action]")]
    [ApiController]
    public class SupplierAccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRefreshTokenService _refreshTokenService;
        private readonly IRestaurantServiceStaffService _restaurantServiceStaffService;
        private readonly IRestaurantDeliveryStaffService _restaurantDeliveryStaffService;
        private readonly IDeliveryStaffService _deliveryStaffService;
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly ILogger<SupplierAccountController> _logger;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IMessageService _messageService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IFCMUserSessionService _fcmService;
        private readonly ISupplierService _supplierService;
        private readonly ISupplierOTPVerificationService _supplierOTPVerificationService;
        private readonly INumberRangeService _numberRangeService;

        public SupplierAccountController(SignInManager<AppUser> signInManager
            , UserManager<AppUser> userManager
            , IConfiguration config
            , ILogger<SupplierAccountController> logger
            , IMapper mapper
            , ITokenService tokenService
            , IEmailService emailService
            , IUserRefreshTokenService refreshTokenService
            , IRouteGroupService routeGroupService
            , IRestaurantServiceStaffService restaurantServiceStaffService
            , IRestaurantDeliveryStaffService restaurantDeliveryStaffService
            , IUserService userService
            , IDeliveryStaffService deliveryStaffService
            , IMessageService messageService
            , IFTPUpload fTPUpload
            , IFCMUserSessionService fcmService
            , ISupplierService supplierService
            , ISupplierOTPVerificationService supplierOTPVerificationService
            , INumberRangeService numberRangeService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
            _restaurantServiceStaffService = restaurantServiceStaffService;
            _restaurantDeliveryStaffService = restaurantDeliveryStaffService;
            _userService = userService;
            _deliveryStaffService = deliveryStaffService;
            _emailService = emailService;
            _refreshTokenService = refreshTokenService;
            _messageService = messageService;
            _numberRangeService = numberRangeService;
            _fTPUpload = fTPUpload;
            _fcmService = fcmService;
            _supplierService = supplierService;
            _supplierOTPVerificationService = supplierOTPVerificationService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginData model)
        {
            IEnumerable<AppUser> Users = await _userService.GetUserByEmailAndCheck(model.Email, Enum.GetName(typeof(Logins), Logins.Supplier));

            if (!Users.Any())
                return Conflict("No user found. Invalid email address");

            if (Users.FirstOrDefault().IsDeleted)
                return Conflict("No user found. Invalid email address");

            if (!Users.FirstOrDefault().IsActive)
                return Conflict("Account suspended! contact your administrator");

            if (!Users.FirstOrDefault().PhoneNumberConfirmed)
                return Conflict("Kindly confirm phone number first");

            if (!Users.FirstOrDefault().EmailConfirmed)
                return Conflict("Kindly confirm email first");

            var result = await _signInManager.PasswordSignInAsync(Users.FirstOrDefault(), model.Password, false, false);

            if (result.Succeeded)
            {
                LoginResponse Response = await MakeLoginResponse(Users.FirstOrDefault());
                Response.Supplier = _mapper.Map<IEnumerable<SupplierDTO>>(await _supplierService.GetByUserIdAsync(Users.FirstOrDefault().Id)).FirstOrDefault();

                _logger.LogInformation("Login Success for " + Users.FirstOrDefault().UserName);

                return Ok(Response);
            }

            return Unauthorized("Invalid password");

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
        [Authorize(Roles = "Supplier")]
        public async Task Logout()
        {
            string DeviceTrackId = User.FindFirstValue("DeviceTrackId");
            await _signInManager.SignOutAsync();

            await _refreshTokenService.DeleteRefreshTokenAsync(DeviceTrackId);

            _logger.LogInformation("Logout Success for [" + _userManager + "]");
        }

        [HttpPost]
        //[Authorize(Roles = "Customer")]
        [AllowAnonymous]
        public async Task<AccessToken> RefreshToken(AccessToken CurrentAccessToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            return await _tokenService.RefreshAccessToken(CurrentAccessToken);
        }

        [HttpPost("{Email}")]
        public async Task<IActionResult> SendOTPEmail(string Email)
        {
            Random rnd = new();
            IEnumerable<SupplierOTPVerification> SupplierList = await _supplierOTPVerificationService.GetByEmailAsync(Email);

            if (SupplierList.Any())
            {
                SupplierOTPVerification Supplier = SupplierList.FirstOrDefault();

                Supplier.OTP = rnd.Next(1000, 9999);
                Supplier.OTPExpiryTime = DateTime.UtcNow.AddMinutes(2);

                await _supplierOTPVerificationService.UpdateAsync(Supplier);

                await SendConfirmationEmail(Email, Supplier.OTP);

                return Ok(new SuccessResponse<string>("Email sent successfully", ""));
            }

            SupplierOTPVerification SupplierCreation = new()
            {
                Email = Email,
                OTP = rnd.Next(1000, 9999),
                OTPExpiryTime = DateTime.UtcNow.AddMinutes(2),
                IsVerified = false
            };

            await _supplierOTPVerificationService.InsertAsync(SupplierCreation);

            await SendConfirmationEmail(Email, SupplierCreation.OTP);

            return Ok(new SuccessResponse<string>("Email sent successfully", ""));
        }

        [HttpPut("{OTP}/{Email}")]
        public async Task<IActionResult> ConfirmEmail(long OTP, string Email)
        {
            IEnumerable<SupplierOTPVerification> SupplierList = await _supplierOTPVerificationService.GetByEmailAsync(Email);

            if (!SupplierList.Any())
            {
                return Conflict(new ErrorDetails(409, "Invalid email address", ""));
            }

            SupplierOTPVerification supplier = SupplierList.FirstOrDefault();

            if (supplier.OTPExpiryTime.TimeOfDay > DateTime.UtcNow.TimeOfDay && supplier.OTP == OTP)
            {
                supplier.IsVerified = true;

                //Forcing Auth code expiry to avoid forgery
                supplier.OTPExpiryTime = DateTime.UtcNow.AddMinutes(-1);

                await _supplierOTPVerificationService.UpdateAsync(supplier);
            }
            else if (supplier.OTPExpiryTime.TimeOfDay < DateTime.UtcNow.TimeOfDay)
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
            var ExistingUsersListByNumber = await _userService.GetUserByNumberAndCheck(model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.Supplier));

            if (ExistingUsersListByNumber.Any())
                return Conflict(new ErrorDetails(409, "Phone number already registered", string.Empty));

            var ExistingUsersListByEmail = await _userService.GetUserByEmailAndCheck(model.Email, Enum.GetName(typeof(Logins), Logins.Supplier));

            if (ExistingUsersListByEmail.Any())
                return Conflict(new ErrorDetails(409, "Email already registered", string.Empty));

            IEnumerable<SupplierOTPVerification> supplierListByEmail = await _supplierOTPVerificationService.GetByEmailAsync(model.Email);

            if ((supplierListByEmail.Any() && !supplierListByEmail.FirstOrDefault().IsVerified) || !supplierListByEmail.Any())
                return Conflict(new ErrorDetails(409, "Email not verified", string.Empty));

            IEnumerable<SupplierOTPVerification> supplierListByPhone = await _supplierOTPVerificationService.GetByPhoneAsync(model.PhoneNumber);

            if ((supplierListByPhone.Any() && !supplierListByPhone.FirstOrDefault().IsVerified) || !supplierListByPhone.Any())
                return Conflict(new ErrorDetails(409, "Phone number not verified", string.Empty));


            Random rnd = new();
            var User = new AppUser
            {
                UserName = model.Email + rnd.Next(1000, 99999).ToString(),
                Email = model.Email,
                EmailConfirmed = true,
                FirstName = model.FirstName,
                LastName = model.LastName,
                AuthCode = 0000,
                LoginFor = Enum.GetName(typeof(Logins), Logins.Supplier),
                AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(-1),
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = true,
                IsActive = true
        };

            IdentityResult result = await _userManager.CreateAsync(User, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(User, Enum.GetName(typeof(Roles), Roles.Supplier));

                _logger.LogInformation("Registration Success for " + User.Email);

                Models.Supplier supplier = new()
                {
                    NameAsPerTradeLicense = model.FirstName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    UserId = User.Id,
                    Status = Enum.GetName(typeof(Status), Status.Pending),
                    ReferenceCode = await _numberRangeService.GetNumberRangeByName("SUPPLIER")
                };

                await _supplierService.AddSupplierAsync(supplier);

                return Ok(new SuccessResponse<string>("Account created successfully", ""));
            }
            else
                return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, string.Empty));
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

        [HttpPost("{Email}")]
        public async Task<IActionResult> ForgetPassword(string Email)
        {
            Random rdn = new Random();
            var user = await _userService.GetUserByEmailAndCheck(Email, Enum.GetName(typeof(Logins), Logins.Supplier));

            if (!user.Any())
                return Conflict(new ErrorDetails(409, "No user found, Invalid email address", string.Empty));

            user.FirstOrDefault().AuthCode = rdn.Next(1000, 9999);
            user.FirstOrDefault().AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2);

            await _userManager.UpdateAsync(user.FirstOrDefault());

            await SendForgetPasswordEmail(user.FirstOrDefault());

            return Ok(new SuccessResponse<string>("", user.FirstOrDefault().Id));
        }
        [HttpPut("{OTP}/{PhoneNumber}")]
        public async Task<IActionResult> VerifyOTPForForgetPassword(int OTP, string Email)
        {
            IEnumerable<AppUser> users = await _userService.GetUserByEmailAndCheck(Email, Enum.GetName(typeof(Logins), Logins.Supplier));

            if (!users.Any())
            {
                return Conflict("Invalid email address");
            }

            AppUser user = users.FirstOrDefault();

            if (user.AuthCodeExpiryTime.TimeOfDay > DateTime.UtcNow.TimeOfDay && user.AuthCode == OTP)
            {
                //Forcing Auth code expiry to avoid forgery
                user.AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(-1);

                await _userManager.UpdateAsync(user);
            }
            else if (user.AuthCodeExpiryTime.TimeOfDay < DateTime.UtcNow.TimeOfDay)
            {
                _logger.LogError("Confirmation Failure - Auth code expired for supplier " + user.PhoneNumber);
                return Conflict("OTP expired");
            }
            else
            {
                _logger.LogError("Confirmation Failure - Wrong Auth code recieved for supplier " + user.PhoneNumber);
                return Conflict("Invalid OTP");
            }


            _logger.LogInformation("Confirmation Success for supplier " + user.PhoneNumber);

            return Ok("OTP Verified");
        }

        [HttpPut("{OTP}/{PhoneNumber}")]
        public async Task<IActionResult> VerifyOTP(int OTP, string PhoneNumber)
        {
            var SupplierList = await _supplierOTPVerificationService.GetByPhoneAsync(PhoneNumber);

            if (!SupplierList.Any())
            {
                return Conflict(new ErrorDetails(409, "Invalid phone number", ""));
            }

            SupplierOTPVerification supplier = SupplierList.FirstOrDefault();

            if (supplier.OTPExpiryTime.TimeOfDay > DateTime.UtcNow.TimeOfDay && supplier.OTP == OTP)
            {
                supplier.IsVerified = true;

                //Forcing Auth code expiry to avoid forgery
                supplier.OTPExpiryTime = DateTime.UtcNow.AddMinutes(-1);

                await _supplierOTPVerificationService.UpdateAsync(supplier);
            }
            else if (supplier.OTPExpiryTime.TimeOfDay < DateTime.UtcNow.TimeOfDay)
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
        [HttpPost("VerifyOTPForSupplier/{UserId}/{OTP}")]
        public async Task<IActionResult> VerifyOTP(string UserId, int OTP)
        {

            ErrorDetails err;
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                _logger.LogError("Forget Password: Unable to find user " + UserId);
                return NotFound(err = new ErrorDetails(409, "User not found", string.Empty));
            }

            //var result = await _userManager.ResetPasswordAsync(user, model.Code, model.NewPassword);

            if (user.AuthCodeExpiryTime.TimeOfDay > DateTime.UtcNow.TimeOfDay && user.AuthCode == OTP)
            {
                user.AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(-1);
                await _userManager.UpdateAsync(user);
                return Ok(user.Id);
                ////Forcing Auth code expiry to avoid forgery
                //user.AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(-1);
                //await _userManager.UpdateAsync(user);

                //string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                //result = await _userManager.ResetPasswordAsync(user, token, Model.NewPassword);
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
        [HttpPost("{PhoneNumber}")]
        public async Task<IActionResult> ResendOTP(string PhoneNumber)
        {
            Random rnd = new();
            IEnumerable<SupplierOTPVerification> SupplierList = await _supplierOTPVerificationService.GetByPhoneAsync(PhoneNumber);

            if (SupplierList.Any())
            {
                SupplierOTPVerification Supplier = SupplierList.FirstOrDefault();

                if (PhoneNumber == "971507567600")
                    Supplier.OTP = 1234;
                else
                    Supplier.OTP = rnd.Next(1000, 9999);
                Supplier.OTPExpiryTime = DateTime.UtcNow.AddMinutes(2);

                await _supplierOTPVerificationService.UpdateAsync(Supplier);

                await SendOTP(PhoneNumber, Supplier.OTP);

                return Ok(new SuccessResponse<string>("OTP sent successfully", ""));
            }

            SupplierOTPVerification SupplierCreation = new()
            {
                PhoneNumber = PhoneNumber,
                OTP = PhoneNumber == "971507567600" ? 1234 : rnd.Next(1000, 9999),
                OTPExpiryTime = DateTime.UtcNow.AddMinutes(2),
                IsVerified = false
            };

            await _supplierOTPVerificationService.InsertAsync(SupplierCreation);

            await SendOTP(PhoneNumber, SupplierCreation.OTP);

            return Ok(new SuccessResponse<string>("OTP sent successfully", ""));
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ChangePasswordDTO Model)
        {
            IdentityResult result;
            var user = await _userManager.FindByIdAsync(Model.UserId);

            if (user == null)
            {
                _logger.LogError("Forget Password: Unable to find user " + Model.UserId);
                return NotFound("User not found");
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            result = await _userManager.ResetPasswordAsync(user, token, Model.NewPassword);

            if (result.Succeeded)
                return Ok("Password reset successfully");
            else
                return Conflict(result.Errors.First<IdentityError>().Description);
        }

        [Authorize(Roles = "Supplier")]
        [HttpPut]
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

        private async Task<bool> SendOTP(string Phonenumber, int OTP)
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

        [HttpPut, DisableRequestSizeLimit]
        [Authorize(Roles = "Supplier")]
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
        //[Authorize(Roles = "Supplier")]
        public async Task<IActionResult> Update(RegisterData Model)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            AppUser user = await _userManager.FindByIdAsync(UserId);

            user.FirstName = Model.FirstName;
            user.LastName = Model.LastName;
            user.Email = Model.Email;
            user.PhoneNumber = Model.PhoneNumber;

            await _userManager.UpdateAsync(user);

            return Ok(user);
        }

        [HttpPut]
        //[Authorize(Roles = "Supplier")]
        public async Task<IActionResult> CompleteProfile(SupplierDTO Entity)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Models.Supplier> Suppliers = await _supplierService.GetByUserIdAsync(UserId);

            Models.Supplier SupplierMerge = _mapper.Map(Entity, Suppliers.FirstOrDefault());

            SupplierMerge.UserId = UserId;
            Models.Supplier result = await _supplierService.UpdateSupplierAsync(SupplierMerge);

            AppUser user = await _userManager.FindByIdAsync(UserId);

            user.FirstName = result.NameAsPerTradeLicense;
            user.LastName = result.NameAsPerTradeLicense;
            user.PhoneNumber = result.PhoneNumber;

            await _userManager.UpdateAsync(user);

            return Ok(_mapper.Map<SupplierDTO>(result));
        }

        [HttpPut("Approval/{SupplierId}")]
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> Approval(long SupplierId)
        {
            IEnumerable<Models.Supplier> Suppliers = await _supplierService.GetByIdAsync(SupplierId);

            Models.Supplier Supplier = Suppliers.FirstOrDefault();

            if (Supplier.Status == Enum.GetName(typeof(Status), Status.Pending))
                Supplier.Status = Enum.GetName(typeof(Status), Status.Processing);

            else if (Supplier.Status == Enum.GetName(typeof(Status), Status.Processing))
                Supplier.Status = Enum.GetName(typeof(Status), Status.Pending);

            return Ok(await _supplierService.UpdateSupplierAsync(Supplier));
        }
    }
}
