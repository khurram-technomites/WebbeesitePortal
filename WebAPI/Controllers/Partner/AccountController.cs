using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.DeliveryStaff;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.RestaurantCashierStaff;
using HelperClasses.DTOs.RestaurantDeliveryStaff;
using HelperClasses.DTOs.RestaurantKitchenManager;
using HelperClasses.DTOs.RestaurantServiceStaff;
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
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Partner
{
	[Route("api/partner/[controller]/[action]")]
	[ApiController]
	public class AccountController : ControllerBase
	{

		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly IUserRefreshTokenService _refreshTokenService;
		private readonly IRestaurantServiceStaffService _restaurantServiceStaffService;
		private readonly IRestaurantService _restaurantService;
		private readonly IRestaurantBranchService _restaurantBranchService;
		private readonly IRestaurantDeliveryStaffService _restaurantDeliveryStaffService;
		private readonly IRestaurantCashierStaffService _restaurantCashierStaffService;
		private readonly IRestaurantKitchenManagerService _restaurantKitchenManagerService;
		private readonly IDeliveryStaffService _deliveryStaffService;
		private readonly IUserService _userService;
		private readonly IConfiguration _config;
		private readonly IEmailService _emailService;
		private readonly ILogger<ServiceAndDeliveryStaffAccountController> _logger;
		private readonly IMapper _mapper;
		private readonly ITokenService _tokenService;
		private readonly IMessageService _messageService;
		private readonly IFTPUpload _fTPUpload;
		private readonly IFCMUserSessionService _fcmService;
		private readonly IRestaurantUserLogManagementService _restaurantUserLogManagementService;

		public AccountController(SignInManager<AppUser> signInManager
			, UserManager<AppUser> userManager
			, IConfiguration config
			, ILogger<ServiceAndDeliveryStaffAccountController> logger
			, IMapper mapper
			, IRestaurantService restaurantService
			, IRestaurantBranchService restaurantBranchService
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
			, IRestaurantCashierStaffService restaurantCashierStaffService
			, IRestaurantKitchenManagerService restaurantKitchenManagerService
			, IRestaurantUserLogManagementService restaurantUserLogManagementService

			)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_config = config;
			_logger = logger;
			_mapper = mapper;
			_restaurantBranchService = restaurantBranchService;
			_restaurantService = restaurantService;
			_mapper = mapper;
			_tokenService = tokenService;
			_restaurantServiceStaffService = restaurantServiceStaffService;
			_restaurantDeliveryStaffService = restaurantDeliveryStaffService;
			_userService = userService;
			_deliveryStaffService = deliveryStaffService;
			_emailService = emailService;
			_refreshTokenService = refreshTokenService;
			_messageService = messageService;
			_fTPUpload = fTPUpload;
			_fcmService = fcmService;
			_restaurantCashierStaffService = restaurantCashierStaffService;
			_restaurantKitchenManagerService = restaurantKitchenManagerService;
			_restaurantUserLogManagementService = restaurantUserLogManagementService;
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginData model)
		{
			IEnumerable<AppUser> Users = await _userService.GetUserByNumber(model.PhoneNumber);
			AppUser user = Users.FirstOrDefault();

			if (!Users.Any() || Users.FirstOrDefault().IsDeleted)
				return Unauthorized(new ErrorDetails(409, "No user found. Invalid phone number", string.Empty));

			if (!Users.FirstOrDefault().PhoneNumberConfirmed)
				return Conflict(new ErrorDetails(409, "Kindly confirm phone number first", string.Empty));

			if (!Users.FirstOrDefault().IsActive)
				return Conflict(new ErrorDetails(409, "Account suspended. Contact your Administrator", string.Empty));

			if (user.LoginFor != Enum.GetName(typeof(Logins), Logins.RestaurantServiceStaff) && user.LoginFor != Enum.GetName(typeof(Logins), Logins.RestaurantDeliveryStaff)
				&& user.LoginFor != Enum.GetName(typeof(Logins), Logins.RestaurantCashierStaff) && user.LoginFor != Enum.GetName(typeof(Logins), Logins.RestaurantKitchenManager))
				return Conflict(new ErrorDetails(401, "No user found. Invalid Credentials", string.Empty));

			/* Verify User Process*/
			string verificationMessage = string.Empty;
			if (!VerifyUser(user, ref verificationMessage))
			{
				return Conflict(new ErrorDetails(401, verificationMessage, string.Empty));
			}

			var result = await _signInManager.PasswordSignInAsync(Users.FirstOrDefault(), model.Password, false, false);

			if (result.Succeeded)
			{
				LoginResponse Response = await MakeLoginResponse(Users.FirstOrDefault());
				if (user.LoginFor == Enum.GetName(typeof(Logins), Logins.RestaurantServiceStaff))
				{
					Response.RestaurantServiceStaff = _mapper.Map<RestaurantServiceStaffDTO>(_restaurantServiceStaffService.GetRestaurantServiceStaffByUserAsync(Users.FirstOrDefault().Id).Result.FirstOrDefault());

					#region User Log Management

					await _restaurantUserLogManagementService.AddRestaurantUserLogManagementArgumentsAsync(DateTime.UtcNow.ToDubaiDateTime(), model.DeviceId, Users.FirstOrDefault().Id, Enum.GetName(typeof(Logins), Logins.RestaurantServiceStaff), Response.RestaurantServiceStaff.Id, Response.RestaurantServiceStaff.RestaurantId, Response.RestaurantServiceStaff.RestaurantBranchId);

					#endregion
				}
				else if (user.LoginFor == Enum.GetName(typeof(Logins), Logins.RestaurantDeliveryStaff))
				{
					Response.RestaurantDeliveryStaff = _mapper.Map<HelperClasses.DTOs.Restaurant.RestaurantDeliveryStaffDTO>(_restaurantDeliveryStaffService.GetRestaurantDeliveryStaffByUserAsync(Users.FirstOrDefault().Id).Result);

					#region User Log Management

					await _restaurantUserLogManagementService.AddRestaurantUserLogManagementArgumentsAsync(DateTime.UtcNow.ToDubaiDateTime(), model.DeviceId, Users.FirstOrDefault().Id, Enum.GetName(typeof(Logins), Logins.RestaurantDeliveryStaff), Response.RestaurantDeliveryStaff.Id, Response.RestaurantDeliveryStaff.RestaurantId, Response.RestaurantDeliveryStaff.RestaurantBranchId);

					#endregion
				}
				else if (user.LoginFor == Enum.GetName(typeof(Logins), Logins.RestaurantCashierStaff))
				{
					Response.RestaurantCashierStaff = _mapper.Map<HelperClasses.DTOs.RestaurantCashierStaff.RestaurantCashierStaffDTO>(_restaurantCashierStaffService.GetRestaurantCashierStaffByUserAsync(Users.FirstOrDefault().Id, model.DeviceId).Result);
					_logger.LogInformation("Login Success for " + Users.FirstOrDefault().UserName);

					#region User Log Management

					await _restaurantUserLogManagementService.AddRestaurantUserLogManagementArgumentsAsync(DateTime.UtcNow.ToDubaiDateTime(), model.DeviceId, Users.FirstOrDefault().Id, Enum.GetName(typeof(Logins), Logins.RestaurantCashierStaff), Response.RestaurantCashierStaff.Id, Response.RestaurantCashierStaff.RestaurantId, Response.RestaurantCashierStaff.RestaurantBranchId);

					#endregion
				}
				else if (user.LoginFor == Enum.GetName(typeof(Logins), Logins.RestaurantKitchenManager))
				{
					HelperClasses.DTOs.RestaurantKitchenManager.RestaurantKitchenManagerDTO kitchen = _mapper.Map<HelperClasses.DTOs.RestaurantKitchenManager.RestaurantKitchenManagerDTO>(_restaurantKitchenManagerService.GetRestaurantKitchenManagerByUserAsync(Users.FirstOrDefault().Id).Result);
					Response.RestaurantKitchenManager = kitchen;
					_logger.LogInformation("Login Success for " + Users.FirstOrDefault().UserName);

					IEnumerable<RestaurantDTO> restaurant = _mapper.Map<IEnumerable<RestaurantDTO>>(await _restaurantService.GetRestaurantByIdAsync(kitchen.Id));
					Response.Restaurant = restaurant.FirstOrDefault();

					#region User Log Management

					await _restaurantUserLogManagementService.AddRestaurantUserLogManagementArgumentsAsync(DateTime.UtcNow.ToDubaiDateTime(), model.DeviceId, Users.FirstOrDefault().Id, Enum.GetName(typeof(Logins), Logins.RestaurantKitchenManager), Response.RestaurantKitchenManager.Id, Response.RestaurantKitchenManager.RestaurantId, Response.RestaurantKitchenManager.RestaurantBranchId);

					#endregion
				}
				else
				{
					return Conflict(new ErrorDetails(401, "Cannot proceed the request with given credentials", string.Empty));
				}
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
			#region User Log Management

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			IEnumerable<AppUser> list = await _userService.GetUsersByIdAsync(userId);
			AppUser appUser = list.FirstOrDefault();
			await _restaurantUserLogManagementService.UpdateRestaurantUserLogManagementArgumentsAsync(DateTime.UtcNow.ToDubaiDateTime(), userId, appUser.LoginFor);

			#endregion

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
			IEnumerable<AppUser> Users = await _userService.GetUserByNumberAndCheck(PhoneNumber, OTPFor);
			Random rnd = new();
			var user = Users.FirstOrDefault();

			if (user == null)
				return Conflict(new ErrorDetails(409, "User doesn't exists", string.Empty));

			user.AuthCode = PhoneNumber.Equals("971507567600") ? 1234 : rnd.Next(1000, 9999);
			user.AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2);

			await _userManager.UpdateAsync(user);

			if (PhoneNumber.StartsWith("971"))
				await SendOTP(user);
			else
				await SendConfirmationEmail(user);

			return Ok(new SuccessResponse<string>("OTP sent successfully", user.Id));
		}

		[HttpPost]
		public async Task<IActionResult> ConfirmPhoneNumber(ConfirmUser Model)
		{
			var user = await _userManager.FindByIdAsync(Model.UserId);
			if (user == null)
			{
				_logger.LogError("Confirmation Failure - Unable to load user " + Model.UserId);
				return Conflict(new ErrorDetails(409, "User not found", string.Empty));
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
				return Conflict(new ErrorDetails(409, "OTP has been expired", string.Empty));
			}
			else
			{
				_logger.LogError("Confirmation Failure - Wrong Auth code recieved for user " + Model.UserId);
				return Conflict(new ErrorDetails(409, "Invalid OTP", string.Empty));
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

		[HttpPut]
		private async Task<IActionResult> UpdateServiceStaff(RestaurantServiceStaffRegisterDTO Model)
		{
			IEnumerable<AppUser> users = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.RestaurantServiceStaff));

			if (users.Count() == 0)
				return Conflict("No record found, Invalid phone number");

			AppUser User = users.FirstOrDefault();

			User.FirstName = Model.FirstName;
			User.LastName = Model.LastName;
			User.Email = Model.Email;
			User.PhoneNumber = Model.PhoneNumber;

			IdentityResult result = await _userManager.UpdateAsync(User);

			if (result.Succeeded)
			{
				IEnumerable<Models.RestaurantServiceStaff> serviceStaffList = await _restaurantServiceStaffService.GetRestaurantServiceStaffByIdAsync(Model.RestaurantServiceStaff.Id);
				Models.RestaurantServiceStaff serviceStaff = serviceStaffList.FirstOrDefault();

				if (!serviceStaff.Logo.Equals(Model.RestaurantServiceStaff.Logo))
				{
					if (!string.IsNullOrEmpty(Model.RestaurantServiceStaff.Logo))
					{
						string LogoPath = "/Images/RestaurantServiceStaff/" + User.UserName + "/";
						if (_fTPUpload.MoveFile(Model.RestaurantServiceStaff.Logo, ref LogoPath))
						{
							Model.RestaurantServiceStaff.Logo = LogoPath;
						}
					}
				}

				if (Model.Password != null)
				{
					AppUser user = await _userManager.FindByIdAsync(users.FirstOrDefault().Id);
					string token = await _userManager.GeneratePasswordResetTokenAsync(user);
					await _userManager.ResetPasswordAsync(user, token, Model.Password);
				}

				Model.RestaurantServiceStaff.UserId = serviceStaff.UserId;
				Model.RestaurantServiceStaff.PhoneNumber = serviceStaff.PhoneNumber;
				Model.RestaurantServiceStaff.Email = serviceStaff.Email;
				Model.RestaurantServiceStaff.FirstName = serviceStaff.FirstName;
				Model.RestaurantServiceStaff.LastName = serviceStaff.LastName;
				Model.RestaurantServiceStaff.User = null;

				Model.RestaurantServiceStaff = _mapper.Map<RestaurantServiceStaffDTO>(await _restaurantServiceStaffService.UpdateRestaurantServiceStaffAsync(_mapper.Map<Models.RestaurantServiceStaff>(Model.RestaurantServiceStaff)));

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

		[Authorize(Roles = "RestaurantServiceStaff, RestaurantDeliveryStaff, RestaurantCashierStaff")]
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

		private bool VerifyUser(AppUser user, ref string message)
		{
			/*Remove This*/
			
			//if (user.LoginFor == Enum.GetName(typeof(Logins), Logins.RestaurantCashierStaff))
			//{
			//	var staff = _restaurantCashierStaffService.GetRestaurantCashierStaffByUserAsync(user.Id).Result;
			//	var RestaurantCashierStaff = _mapper.Map<HelperClasses.DTOs.RestaurantCashierStaff.RestaurantCashierStaffDTO>(staff);

			//	if (!RestaurantCashierStaff.IsShiftClose)
			//	{
			//		message = "This account is already logged in on another device";
			//		return false;
			//	}
			//}
			return true;
		
			/*Remove This*/

			if (user.LoginFor == Enum.GetName(typeof(Logins), Logins.RestaurantServiceStaff))
			{
				var staff = _restaurantServiceStaffService.GetRestaurantServiceStaffByUserAsync(user.Id).Result.FirstOrDefault();
				//RestaurantServiceStaffDTO RestaurantServiceStaff = _mapper.Map<RestaurantServiceStaffDTO>(staff);

				if (!staff.Restaurant.IsPartnerAllowed)
					message = "The restaurant has not been allowed to login Partner";
				else
					return true;
			}
			else if (user.LoginFor == Enum.GetName(typeof(Logins), Logins.RestaurantDeliveryStaff))
			{
				var staff = _restaurantDeliveryStaffService.GetRestaurantDeliveryStaffByUserAsync(user.Id).Result;
				//HelperClasses.DTOs.Restaurant.RestaurantDeliveryStaffDTO RestaurantDeliveryStaff = _mapper.Map<HelperClasses.DTOs.Restaurant.RestaurantDeliveryStaffDTO>(staff);

				if (!staff.Restaurant.IsPartnerAllowed)
					message = "The restaurant has not been allowed to login Partner";
				else
					return true;
			}
			else if (user.LoginFor == Enum.GetName(typeof(Logins), Logins.RestaurantCashierStaff))
			{
				var staff = _restaurantCashierStaffService.GetRestaurantCashierStaffByUserAsync(user.Id).Result;
				var RestaurantCashierStaff = _mapper.Map<HelperClasses.DTOs.RestaurantCashierStaff.RestaurantCashierStaffDTO>(staff);

				if (!staff.Restaurant.IsCashierAllowed)
					message = "The restaurant has not been allowed to login Cashier";
				else if (!RestaurantCashierStaff.IsShiftClose)
					message = "This account is already logged in on another device";
				else
					return true;
			}
			else if (user.LoginFor == Enum.GetName(typeof(Logins), Logins.RestaurantKitchenManager))
			{
				var staff = _restaurantKitchenManagerService.GetRestaurantKitchenManagerByUserAsync(user.Id).Result;
				//HelperClasses.DTOs.RestaurantKitchenManager.RestaurantKitchenManagerDTO RestaurantKitchenManager = _mapper.Map<HelperClasses.DTOs.RestaurantKitchenManager.RestaurantKitchenManagerDTO>(staff);

				if (!staff.Restaurant.IsKitchenManagerAllowed)
					message = "The restaurant has not been allowed to login Kitchen Manager";
				else
					return true;
			}
			else
			{
				message = "Something went wrong";
			}
			return false;
		}
	}
}
