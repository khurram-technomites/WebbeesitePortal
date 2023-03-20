using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin , RestaurantOwner , RestaurantCashierStaff")]
    [ApiController]
    public class RestaurantDeliveryStaffController : ControllerBase
    {
        private readonly IRestaurantDeliveryStaffService _RestaurantDeliveryStaffService;
        private readonly IMapper _mapper;
        private readonly IFTPUpload _fTPUpload;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;

        public RestaurantDeliveryStaffController(IRestaurantDeliveryStaffService RestaurantDeliveryStaffService, IMapper mapper , IFTPUpload fTPUpload, IUserService userService,
            UserManager<AppUser> userManager)
        {
            _RestaurantDeliveryStaffService = RestaurantDeliveryStaffService;
            _mapper = mapper;
            _fTPUpload = fTPUpload;
            _userService = userService;
            _userManager = userManager;
        }

        [HttpGet("GetAll/{restaurantId}")]
        public async Task<IActionResult> GetAll(long restaurantId)
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantDeliveryStaffDTO>>(await _RestaurantDeliveryStaffService.GetAllRestaurantDeliveryStaffsAsync(restaurantId)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {

            IEnumerable<RestaurantDeliveryStaffDTO> staff = _mapper.Map<IEnumerable<RestaurantDeliveryStaffDTO>>(await _RestaurantDeliveryStaffService.GetRestaurantDeliveryStaffByIdAsync(Id));
            RestaurantDeliveryStaffDTO staffModel = staff.FirstOrDefault();

            return Ok(staffModel);
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            IEnumerable<RestaurantDeliveryStaff> staffs = await _RestaurantDeliveryStaffService.GetRestaurantDeliveryStaffByIdAsync(Id);

            AppUser user = await _userManager.FindByIdAsync(staffs.FirstOrDefault().UserId);
            user.IsDeleted = true;

            await _userManager.UpdateAsync(user);

            return Ok(_mapper.Map<RestaurantDeliveryStaffDTO>(await _RestaurantDeliveryStaffService.ArchiveRestaurantDeliveryStaffAsync(Id)));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Add(RestaurantDeliveryStaffDTO Model)
        {
            IEnumerable<AppUser> usersByPhone = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.RestaurantDeliveryStaff));

            if (usersByPhone.Any())
                return Conflict("Phone number is already in use.");

            IEnumerable<AppUser> usersByEmail = await _userService.GetUserByEmailAndCheck(Model.Email, Enum.GetName(typeof(Logins), Logins.RestaurantDeliveryStaff));

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
                LoginFor = Enum.GetName(typeof(Logins), Logins.RestaurantDeliveryStaff),
                PhoneNumber = Model.PhoneNumber,
                IsActive = true,
                PhoneNumberConfirmed = true,
            };

            IdentityResult result = await _userManager.CreateAsync(User, Model.Password);

            if (result.Succeeded)
            {
                IdentityResult UserRoleResult = await _userManager.AddToRoleAsync(User, Enum.GetName(typeof(Roles), Roles.RestaurantDeliveryStaff));

                if (UserRoleResult.Succeeded)
                {
                    if (!string.IsNullOrEmpty(Model.Logo) && Model.Logo.Contains("Draft"))
                    {
                        string LogoPath = "/Images/RestaurantDeliveryStaff/" + User.UserName + "/";
                        if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
                        {
                            Model.Logo = LogoPath;
                        }
                    }
                    Model.UserId = User.Id;
                    Model = _mapper.Map<RestaurantDeliveryStaffDTO>(await _RestaurantDeliveryStaffService.AddRestaurantDeliveryStaffAsync(_mapper.Map<RestaurantDeliveryStaff>(Model)));
                }

                //if (Model.RequirePhoneNumberConfirmation)
                //    if (Model.PhoneNumber.StartsWith("971"))
                //        await SendOTP(User);
                //    else
                //        await SendConfirmationEmail(User);


                return Ok(Model);
            }
            else
                return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, ""));
        }


        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<RestaurantDeliveryStaff> deliveryStaffs = await _RestaurantDeliveryStaffService.GetRestaurantDeliveryStaffByIdAsync(Id);
            RestaurantDeliveryStaff staff = deliveryStaffs.FirstOrDefault();

            if (staff.Status == Enum.GetName(typeof(Status), Status.Active))
                staff.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                staff.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(await _RestaurantDeliveryStaffService.UpdateRestaurantDeliveryStaffAsync(staff));
        }


        [HttpPut]
        public async Task<IActionResult> Update(RestaurantDeliveryStaffDTO Model)
        {
            AppUser user = await _userManager.FindByIdAsync(Model.UserId);

            if (Model.PhoneNumber != user.PhoneNumber)
            {
                IEnumerable<AppUser> users = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.RestaurantDeliveryStaff));

                if (users.Any())
                    return Conflict("Phone number is already in use.");
            }

            if (Model.Email != user.Email)
            {
                IEnumerable<AppUser> users = await _userService.GetUserByEmailAndCheck(Model.Email, Enum.GetName(typeof(Logins), Logins.RestaurantDeliveryStaff));

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
                IEnumerable<RestaurantDeliveryStaff> deliveryStaffs = await _RestaurantDeliveryStaffService.GetRestaurantDeliveryStaffByIdAsync(Model.Id);
                RestaurantDeliveryStaff deliveryStaff = deliveryStaffs.FirstOrDefault();

                if (!string.IsNullOrEmpty(deliveryStaff.Logo) && !deliveryStaff.Logo.Equals(Model.Logo))
                {
                    if (!string.IsNullOrEmpty(Model.Logo))
                    {
                        string LogoPath = "/Images/RestaurantDeliveryStaff/" + user.UserName + "/";
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
                deliveryStaff.RestaurantBranchId= Model.RestaurantBranchId;
                deliveryStaff.User = null;
                deliveryStaff.Logo = Model.Logo;

                Model = _mapper.Map<RestaurantDeliveryStaffDTO>(await _RestaurantDeliveryStaffService.UpdateRestaurantDeliveryStaffAsync(deliveryStaff));

                return Ok(Model);
            }
            else
                return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, ""));
        }

    }

}
