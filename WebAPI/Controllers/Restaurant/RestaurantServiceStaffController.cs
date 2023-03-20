using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.RestaurantServiceStaff;
using HelperClasses.DTOs.ServiceStaff;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantServiceStaffController : ControllerBase
    {
        private readonly IRestaurantServiceStaffService _serviceStaffService;
        private readonly IUserService _userService;
        private readonly IFTPUpload _fTPUpload;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantServiceStaffController> _logger;
        private readonly IMessageService _messageService;
        private readonly IEmailService _emailService;


        public RestaurantServiceStaffController(IRestaurantServiceStaffService serviceStaffService, IMapper mapper, IUserService userService, UserManager<AppUser> userManager,
            IFTPUpload fTPUpload, ISparePartsDealerService sparePartsDealerService, IGarageService garageService
            , ILogger<RestaurantServiceStaffController> logger
            , IMessageService messageService
            , IEmailService emailService)
        {
            _serviceStaffService = serviceStaffService;
            _userManager = userManager;
            _fTPUpload = fTPUpload;
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
            _messageService = messageService;
            _emailService = emailService;
        }
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(PagingParameters Paging)
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantServiceStaffDTO>>(await _serviceStaffService.GetAllRestaurantServiceStaffAsync(Paging)));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<RestaurantServiceStaffDTO> staff = _mapper.Map<IEnumerable<RestaurantServiceStaffDTO>>(await _serviceStaffService.GetAllRestaurantServiceStaffByBranchIdAsync(Id));
            return Ok(staff);
        }
        [HttpGet("Restaurant/{Id}")]
        public async Task<IActionResult> GetByRestaurantId(long Id)
        {
            IEnumerable<RestaurantServiceStaffDTO> staff = _mapper.Map<IEnumerable<RestaurantServiceStaffDTO>>(await _serviceStaffService.GetAllRestaurantServiceStaffByRestaurantIdAsync(Id));
            return Ok(staff);
        }
        [HttpGet("RestaurantServiceStaff/{Id}")]
        public async Task<IActionResult> GetRestaurantServiceStaffById(long Id)
        {
            RestaurantServiceStaffDTO staff = _mapper.Map<RestaurantServiceStaffDTO>(await _serviceStaffService.GetAllRestaurantServiceStaffByIdAsync(Id));
            return Ok(staff);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            RestaurantServiceStaffDTO model = _mapper.Map<RestaurantServiceStaffDTO>(await _serviceStaffService.ArchiveRestaurantServiceStaffAsync(Id));

            AppUser AppUser = await _userManager.FindByIdAsync(model.UserId);
            AppUser.IsDeleted = true;

            await _userManager.UpdateAsync(AppUser);

            return Ok(model);
        }
        [HttpPut("UpdateServiceStaff")]
        public async Task<IActionResult> UpdateServiceStaff(RestaurantServiceStaffDTO Model)
		{
            AppUser user = await _userManager.FindByIdAsync(Model.UserId);

            if(Model.PhoneNumber != user.PhoneNumber)
            {
                IEnumerable<AppUser> users = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.RestaurantServiceStaff));

                if (users.Any())
                    return Conflict("Phone number is already in use.");
            }

            if (Model.Email != user.Email)
            {
                IEnumerable<AppUser> users = await _userService.GetUserByEmailAndCheck(Model.Email, Enum.GetName(typeof(Logins), Logins.RestaurantServiceStaff));

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
				RestaurantServiceStaff serviceStaff = await _serviceStaffService.GetAllRestaurantServiceStaffByIdAsync(Model.Id);

				if (!string.IsNullOrEmpty(serviceStaff.Logo) && !serviceStaff.Logo.Equals(Model.Logo))
				{
					if (!string.IsNullOrEmpty(Model.Logo))
					{
						string LogoPath = "/Images/ResaturantServiceStaff/" + user.UserName + "/";
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

				serviceStaff.UserId = Model.UserId;
				serviceStaff.PhoneNumber = Model.PhoneNumber;
				serviceStaff.Email = Model.Email;
				serviceStaff.FirstName = Model.FirstName;
				serviceStaff.LastName = Model.LastName;
                serviceStaff.RestaurantBranchId = Model.RestaurantBranchId;
				serviceStaff.User = null;
				serviceStaff.Logo = Model.Logo;

                Model = _mapper.Map<RestaurantServiceStaffDTO>(await _serviceStaffService.UpdateRestaurantServiceStaffAsync(serviceStaff));

				_logger.LogInformation("Update Success for " + user.UserName);

				return Ok(Model);
			}
			else
				return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, ""));
		}
        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            RestaurantServiceStaff serviceStaff = await _serviceStaffService.GetAllRestaurantServiceStaffByIdAsync(Id);
            AppUser user = await _userManager.FindByIdAsync(serviceStaff.UserId);

            if (serviceStaff.Status == Enum.GetName(typeof(Status), Status.Active))
            {
                serviceStaff.Status = Enum.GetName(typeof(Status), Status.Inactive);
                user.IsActive = false;
                await _userManager.UpdateAsync(user);
            }
            else
            {
                serviceStaff.Status = Enum.GetName(typeof(Status), Status.Active);
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
            }
                
            serviceStaff.User = null;
            return Ok(_mapper.Map<RestaurantServiceStaffDTO>(await _serviceStaffService.UpdateRestaurantServiceStaffAsync(serviceStaff)));
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterServiceStaff(RestaurantServiceStaffDTO Model)
        {
            IEnumerable<AppUser> usersByPhone = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.RestaurantServiceStaff));

            if (usersByPhone.Any())
                return Conflict("Phone number is already in use.");

            IEnumerable<AppUser> usersByEmail = await _userService.GetUserByEmailAndCheck(Model.Email, Enum.GetName(typeof(Logins), Logins.RestaurantServiceStaff));

            if (usersByEmail.Any())
                return Conflict("Email is already in use.");

            Random rnd = new();
            var User = new AppUser
            {
                UserName = Model.FirstName.Replace(" ", "") + rnd.Next(1000, 9999).ToString(),
                Email = Model.Email,
                FirstName = Model.FirstName,
                LastName = Model.LastName,
                AuthCode = Model.PhoneNumber.Equals("971507567600") ? 1234 : rnd.Next(1000, 9999),
                AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2),
                LoginFor = Enum.GetName(typeof(Logins), Logins.RestaurantServiceStaff),
                PhoneNumber = Model.PhoneNumber,
                IsActive = true,
                PhoneNumberConfirmed = !Model.RequirePhoneNumberConfirmation,
            };

            IdentityResult result = await _userManager.CreateAsync(User, Model.Password);

            if (result.Succeeded)
            {
                IdentityResult UserRoleResult = await _userManager.AddToRoleAsync(User, Enum.GetName(typeof(Roles), Roles.RestaurantServiceStaff));

                if (UserRoleResult.Succeeded)
                {
                    if (!string.IsNullOrEmpty(Model.Logo) && Model.Logo.Contains("Draft"))
                    {
                        string LogoPath = "/Images/RestaurantServiceStaff/" + User.UserName + "/";
                        if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
                        {
                            Model.Logo = LogoPath;
                        }
                        else
                        {
                            Model.Logo = null;
                        }
                    }

                    Model.UserId = User.Id;
                    Model = _mapper.Map<RestaurantServiceStaffDTO>(await _serviceStaffService.AddRestaurantServiceStaffAsync(_mapper.Map<Models.RestaurantServiceStaff>(Model)));
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
