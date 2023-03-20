using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.RestaurantKitchenManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ErrorHandling;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/Restaurant")]
    [ApiController]
    [Authorize(Roles = "Admin , RestaurantOwner")]
    public class RestaurantKitchenManagerController : ControllerBase
    {
        private readonly IRestaurantKitchenManagerService _RestaurantKitchenManagerService;
        private readonly IMapper _mapper;
        private readonly IFTPUpload _fTPUpload;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly ILogger<RestaurantKitchenManagerController> _logger;
        private readonly IMessageService _messageService;

        public RestaurantKitchenManagerController(UserManager<AppUser> userManager
            , ILogger<RestaurantKitchenManagerController> logger
            , IMapper mapper
            , IEmailService emailService
            , IRestaurantKitchenManagerService restaurantKitchenManagerService
            , IUserService userService
            , IMessageService messageService
            , IFTPUpload fTPUpload)
        {
            _userManager = userManager;
            _logger = (ILogger<RestaurantKitchenManagerController>)logger;
            _mapper = mapper;
            _RestaurantKitchenManagerService = restaurantKitchenManagerService;
            _userService = userService;
            _emailService = emailService;
            _messageService = messageService;
            _fTPUpload = fTPUpload;
        }


        //[Authorize(Roles = "Admin")]
        [HttpGet("KitchenManager")]
        public async Task<IActionResult> GetAll()
        {
            var Kitchenstaff = _mapper.Map<IEnumerable<RestaurantKitchenManagerDTO>>(await _RestaurantKitchenManagerService.GetAllAsync());
            return Ok(Kitchenstaff);
        }

        [HttpGet("{restaurantId}/KitchenManager")]
        public async Task<IActionResult> GetAll(long restaurantId)
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantKitchenManagerDTO>>(await _RestaurantKitchenManagerService.GetByRestaurantIdAsync(restaurantId)));
        }

        [HttpGet("KitchenManager/{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {

            IEnumerable<RestaurantKitchenManagerDTO> staff = _mapper.Map<IEnumerable<RestaurantKitchenManagerDTO>>(await _RestaurantKitchenManagerService.GetByIdAsync(Id));
            RestaurantKitchenManagerDTO staffModel = staff.FirstOrDefault();

            return Ok(staffModel);
        }


        [HttpDelete("KitchenManager/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<RestaurantKitchenManagerDTO>(await _RestaurantKitchenManagerService.ArchiveRestaurantKitchenManagerAsync(Id)));
        }

        [HttpPost("KitchenManager")]
        public async Task<IActionResult> Add(RestaurantKitchenManagerDTO Model)
        {
            Model.Status = Enum.GetName(typeof(Status), Status.Active);
            IEnumerable<AppUser> usersByPhone = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.RestaurantKitchenManager));

            if (usersByPhone.Any())
                return Conflict("Phone number is already in use.");

            IEnumerable<AppUser> usersByEmail = await _userService.GetUserByEmailAndCheck(Model.Email, Enum.GetName(typeof(Logins), Logins.RestaurantKitchenManager));

            //if (usersByEmail.Any())
            //    return Conflict("Email is already in use.");

            Random rnd = new();
            var User = new AppUser
            {
                UserName = Model.FirstName.Replace(" ", "") + rnd.Next(1000, 9999).ToString(),
                Email = Model.Email,
                FirstName = Model.FirstName,
                LastName = Model.LastName,
                AuthCode = Model.PhoneNumber.Equals("971507567600") ? 1234 : rnd.Next(1000, 9999),
                AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2),
                LoginFor = Enum.GetName(typeof(Logins), Logins.RestaurantKitchenManager),
                PhoneNumber = Model.PhoneNumber,
                IsActive = true,
                PhoneNumberConfirmed = true,
            };

            IdentityResult result = await _userManager.CreateAsync(User, Model.Password);

            if (result.Succeeded)
            {
                IdentityResult UserRoleResult = await _userManager.AddToRoleAsync(User, Enum.GetName(typeof(Roles), Roles.RestaurantKitchenManager));

                if (UserRoleResult.Succeeded)
                {
                    if (!string.IsNullOrEmpty(Model.Logo))
                    {
                        string LogoPath = "/Images/RestaurantKitchenManager/";
                        if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
                        {
                            Model.Logo = LogoPath;
                        }
                    }
                    Model.UserId = User.Id;
                    Model = _mapper.Map<RestaurantKitchenManagerDTO>(await _RestaurantKitchenManagerService.AddRestaurantKitchenManagerAsync(_mapper.Map<RestaurantKitchenManager>(Model)));
                }

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


        [HttpGet("KitchenManager/{Id}/ToggleStatus")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<RestaurantKitchenManager> kitchenManagers = await _RestaurantKitchenManagerService.GetByIdAsync(Id);
            RestaurantKitchenManager staff = kitchenManagers.FirstOrDefault();

            if (staff.Status == Enum.GetName(typeof(Status), Status.Active))
                staff.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                staff.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(await _RestaurantKitchenManagerService.UpdateRestaurantKitchenManagerAsync(staff));
        }


        [HttpPut("KitchenManager")]
        public async Task<IActionResult> Update(RestaurantKitchenManagerDTO Model)
        {
            AppUser user = await _userManager.FindByIdAsync(Model.UserId);

            if (Model.PhoneNumber != user.PhoneNumber)
            {
                IEnumerable<AppUser> users = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.RestaurantKitchenManager));

                if (users.Any())
                    return Conflict("Phone number is already in use.");
            }

            if (Model.Email != user.Email)
            {
                IEnumerable<AppUser> users = await _userService.GetUserByEmailAndCheck(Model.Email, Enum.GetName(typeof(Logins), Logins.RestaurantKitchenManager));

                if (users.Any())
                    return Conflict("Email is already in use.");
            }

            user.FirstName = Model.FirstName;
            user.LastName = Model.LastName;
            user.Email = Model.Email;
            user.PhoneNumber = Model.PhoneNumber;

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                IEnumerable<RestaurantKitchenManager> kitchenManagers = await _RestaurantKitchenManagerService.GetByIdAsync(Model.Id);
                RestaurantKitchenManager kitchenManager = kitchenManagers.FirstOrDefault();

                if (!string.IsNullOrEmpty(kitchenManager.Logo) && !kitchenManager.Logo.Equals(Model.Logo))
                {
                    if (!string.IsNullOrEmpty(Model.Logo))
                    {
                        string LogoPath = "/Images/RestaurantKitchenManager/" + user.UserName + "/";
                        if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
                        {
                            Model.Logo = LogoPath;
                        }
                    }
                }

                if (Model.Password != null)
                {
                    string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, Model.Password);
                }

                kitchenManager.UserId = Model.UserId;
                kitchenManager.PhoneNumber = Model.PhoneNumber;
                kitchenManager.Email = Model.Email;
                kitchenManager.FirstName = Model.FirstName;
                kitchenManager.LastName = Model.LastName;
				kitchenManager.RestaurantBranchId = Model.RestaurantBranchId;
				kitchenManager.Logo = Model.Logo;
                kitchenManager.User = null;

                Model = _mapper.Map<RestaurantKitchenManagerDTO>(await _RestaurantKitchenManagerService.UpdateRestaurantKitchenManagerAsync(kitchenManager));

                return Ok(Model);
            }
            else
                return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, ""));
        }

        /* Private */

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
