using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.RestaurantCashierStaff;
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
using WebAPI.ResponseWrapper;
using WebAPI.Services.Domains;

namespace WebAPI.Controllers.Restaurant
{
	[Route("api/Restaurant")]
	[ApiController]
	[Authorize(Roles = "Admin, RestaurantOwner, RestaurantCashierStaff")]
	public class RestaurantCashierStaffController : ControllerBase
	{
		private readonly IRestaurantCashierStaffService _RestaurantCashierStaffService;
		private readonly IMapper _mapper;
		private readonly IFTPUpload _fTPUpload;
		private readonly IUserService _userService;
		private readonly UserManager<AppUser> _userManager;
		private readonly IEmailService _emailService;
		private readonly ILogger<RestaurantCashierStaffController> _logger;
		private readonly IMessageService _messageService;

		public RestaurantCashierStaffController(UserManager<AppUser> userManager
			, ILogger<RestaurantCashierStaffController> logger
			, IMapper mapper
			, IEmailService emailService
			, IRestaurantCashierStaffService restaurantCashierStaffService
			, IUserService userService
			, IMessageService messageService
			, IFTPUpload fTPUpload)
		{
			_userManager = userManager;
			_logger = (ILogger<RestaurantCashierStaffController>)logger;
			_mapper = mapper;
			_RestaurantCashierStaffService = restaurantCashierStaffService;
			_userService = userService;
			_emailService = emailService;
			_messageService = messageService;
			_fTPUpload = fTPUpload;
		}

		[HttpGet("{restaurantId}/CashierStaff")]
		public async Task<IActionResult> GetAll(long restaurantId)
		{
			return Ok(_mapper.Map<IEnumerable<RestaurantCashierStaffDTO>>(await _RestaurantCashierStaffService.GetAllRestaurantCashierStaffsAsync(restaurantId)));
		}


		[HttpGet("CashierStaff/{Id}")]
		public async Task<IActionResult> GetById(long Id)
		{

			IEnumerable<RestaurantCashierStaffDTO> staff = _mapper.Map<IEnumerable<RestaurantCashierStaffDTO>>(await _RestaurantCashierStaffService.GetRestaurantCashierStaffByIdAsync(Id));
			RestaurantCashierStaffDTO staffModel = staff.FirstOrDefault();

			return Ok(staffModel);
		}


		[HttpDelete("CashierStaff/{Id}")]
		public async Task<IActionResult> Archive(long Id)
		{
			return Ok(_mapper.Map<RestaurantCashierStaffDTO>(await _RestaurantCashierStaffService.ArchiveRestaurantCashierStaffAsync(Id)));
		}

		[HttpPost("CashierStaff")]
		public async Task<IActionResult> Add(RestaurantCashierStaffDTO Model)
		{
			IEnumerable<AppUser> usersByPhone = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.RestaurantCashierStaff));

			if (usersByPhone.Any())
				return Conflict("Phone number is already in use.");

			IEnumerable<AppUser> usersByEmail = await _userService.GetUserByEmailAndCheck(Model.Email, Enum.GetName(typeof(Logins), Logins.RestaurantCashierStaff));

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
				LoginFor = Enum.GetName(typeof(Logins), Logins.RestaurantCashierStaff),
				PhoneNumber = Model.PhoneNumber,
				IsActive = true,
				PhoneNumberConfirmed = true,
			};

			IdentityResult result = await _userManager.CreateAsync(User, Model.Password);

			if (result.Succeeded)
			{
				IdentityResult UserRoleResult = await _userManager.AddToRoleAsync(User, Enum.GetName(typeof(Roles), Roles.RestaurantCashierStaff));

				if (UserRoleResult.Succeeded)
				{
					if (!string.IsNullOrEmpty(Model.Logo))
					{
						string LogoPath = "/Images/RestaurantCashierStaff/";
						if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
						{
							Model.Logo = LogoPath;
						}
					}
					Model.UserId = User.Id;
					Model = _mapper.Map<RestaurantCashierStaffDTO>(await _RestaurantCashierStaffService.AddRestaurantCashierStaffAsync(_mapper.Map<RestaurantCashierStaff>(Model)));
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


		[HttpGet("CashierStaff/{Id}/ToggleStatus")]
		public async Task<IActionResult> ToggleStatus(long Id)
		{
			IEnumerable<RestaurantCashierStaff> deliveryStaffs = await _RestaurantCashierStaffService.GetRestaurantCashierStaffByIdAsync(Id);
			RestaurantCashierStaff staff = deliveryStaffs.FirstOrDefault();

			if (staff.Status == Enum.GetName(typeof(Status), Status.Active))
				staff.Status = Enum.GetName(typeof(Status), Status.Inactive);
			else
				staff.Status = Enum.GetName(typeof(Status), Status.Active);

			return Ok(await _RestaurantCashierStaffService.UpdateRestaurantCashierStaffAsync(staff));
		}

		[HttpPut("CashierStaff")]
		public async Task<IActionResult> Update(RestaurantCashierStaffDTO Model)
		{
			AppUser user = await _userManager.FindByIdAsync(Model.UserId);

			if (Model.PhoneNumber != user.PhoneNumber)
			{
				IEnumerable<AppUser> users = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.RestaurantCashierStaff));

				if (users.Any())
					return Conflict("Phone number is already in use.");
			}

			if (Model.Email != user.Email)
			{
				IEnumerable<AppUser> users = await _userService.GetUserByEmailAndCheck(Model.Email, Enum.GetName(typeof(Logins), Logins.RestaurantCashierStaff));

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
				IEnumerable<RestaurantCashierStaff> deliveryStaffs = await _RestaurantCashierStaffService.GetRestaurantCashierStaffByIdAsync(Model.Id);
				RestaurantCashierStaff deliveryStaff = deliveryStaffs.FirstOrDefault();

				if (!string.IsNullOrEmpty(deliveryStaff.Logo) && !deliveryStaff.Logo.Equals(Model.Logo))
				{
					if (!string.IsNullOrEmpty(Model.Logo))
					{
						string LogoPath = "/Images/RestaurantCashierStaff/" + user.UserName + "/";
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

				deliveryStaff.UserId = Model.UserId;
				deliveryStaff.PhoneNumber = Model.PhoneNumber;
				deliveryStaff.Email = Model.Email;
				deliveryStaff.FirstName = Model.FirstName;
				deliveryStaff.LastName = Model.LastName;
                deliveryStaff.RestaurantBranchId = Model.RestaurantBranchId;
                deliveryStaff.User = null;
				deliveryStaff.Logo = Model.Logo;
				deliveryStaff.Password = Model.Password;
				var test = _mapper.Map<RestaurantCashierStaffDTO>(deliveryStaff);

				deliveryStaff.RestaurantBranch = null;
				Model = _mapper.Map<RestaurantCashierStaffDTO>(await _RestaurantCashierStaffService.UpdateRestaurantCashierStaffAsync(deliveryStaff));

				return Ok(Model);
			}
			else
				return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, ""));
		}

		[HttpPut("CashierStaff/AllowPrinter")]
		public async Task<IActionResult> AllowPrinter(RestaurantCashierStaffDTO Model)
		{
			IEnumerable<RestaurantCashierStaff> deliveryStaffs = await _RestaurantCashierStaffService.GetRestaurantCashierStaffByIdAsync(Model.Id);
			RestaurantCashierStaff deliveryStaff = deliveryStaffs.FirstOrDefault();

			string msg = "";

			if (deliveryStaff.IsPrinterAllowed == true)
			{
				deliveryStaff.IsPrinterAllowed = false;
				msg = "Printer disabled successfully";
			}
			else
			{
				deliveryStaff.IsPrinterAllowed = true;
				msg = "Printer enabled successfully";
			}
			_mapper.Map<RestaurantCashierStaffDTO>(await _RestaurantCashierStaffService.UpdateRestaurantCashierStaffAsync(deliveryStaff));

			return Ok(new SuccessResponse<RestaurantCashierStaff>(msg, null));
		}

		[HttpGet("CashierStaff/{Id}/IsPrinterAllowed")]
		public async Task<IActionResult> IsPrinterAllowed(long Id)
		{
			IEnumerable<RestaurantCashierStaffDTO> staff = _mapper.Map<IEnumerable<RestaurantCashierStaffDTO>>(await _RestaurantCashierStaffService.GetRestaurantCashierStaffByIdAsync(Id));
			RestaurantCashierStaffDTO staffModel = staff.FirstOrDefault();

			object Printer = new
			{
				IsPrinterAllowed = staffModel.IsPrinterAllowed
			};

			return Ok(new SuccessResponse<object>("", Printer));
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
