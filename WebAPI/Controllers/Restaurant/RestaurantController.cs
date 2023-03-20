using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.Order;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ErrorHandling;
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantImageService _Service;
        private readonly IRestaurantService _restaurantService;
		private readonly IOrderService _orderService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RestaurantController> _logger;
        public readonly INumberRangeService _numberRangeService;
        private readonly IMessageService _messageService;
        private readonly IEmailService _emailService;

        public RestaurantController(IRestaurantService resturantService, IOrderService orderService, UserManager<AppUser> userManager, INumberRangeService numberRangeService, IMessageService messageService, IEmailService emailService, IMapper mapper, IFTPUpload fTPUpload, ILogger<RestaurantController> logger, IRestaurantImageService service)
        {
			_orderService = orderService;
            _mapper = mapper;
            _fTPUpload = fTPUpload;
            _restaurantService = resturantService;
            _userManager = userManager;
            _numberRangeService = numberRangeService;
            _logger = logger;
            _messageService = messageService;
            _emailService = emailService;
            _Service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantDTO>>(await _restaurantService.GetAllRestaurantsAsync()));
        }

        [HttpPost("GetUnAssignedSupplierCodeRestaurant")]
        public async Task<IActionResult> GetAllDropDownAsync()
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantDTO>>(await _restaurantService.GetRestaurantForDropDwonAsync()));
        }
        [HttpPost("GetAssignedSupplierCodeRestaurant")]
        public async Task<IActionResult> GetRestaurantForDropDwonAssignAsync()
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantDTO>>(await _restaurantService.GetRestaurantForDropDwonAssignAsync()));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetByIdAsync(long Id)
        {
            IEnumerable<RestaurantDTO> cities = _mapper.Map<IEnumerable<RestaurantDTO>>(await _restaurantService.GetRestaurantByIdAsync(Id));
            return Ok(cities.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Post(RestaurantDTO Model)
        {
            Random rnd = new Random();
            var User = new AppUser
            {
                UserName = Model.User.Email,
                Email = Model.User.Email,
                FirstName = Model.NameAsPerTradeLicense,
                LastName = Model.NameAsPerTradeLicense,
                AuthCode = Model.PhoneNumber.Equals("971507567600") ? 1234 : rnd.Next(1000, 9999),
                AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2),
                LoginFor = Enum.GetName(typeof(Logins), Logins.Restaurant),
                PhoneNumber = Model.PhoneNumber,
                PhoneNumberConfirmed = !Model.User.RequirePhoneNumberConfirmation,
                EmailConfirmed = true,
                IsActive = true
            };

            IdentityResult result = await _userManager.CreateAsync(User, Model.User.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(User, Enum.GetName(typeof(Roles), Roles.RestaurantOwner));

                _logger.LogInformation("Registration Success for " + User.UserName);


                Model.UserId = User.Id;
                Model.ReferenceCode = await _numberRangeService.GetNextRange("R-");
                Model.Slug = Slugify.GenerateSlug(Model.NameAsPerTradeLicense, Model.ReferenceCode);
                Model.RestaurantBannerSettings = null;
                Model.restaurantDeliveryStaffs = null;
                Model.restaurantRatings = null;
                Model.RestaurantBranches = null;
                Model.RestaurantImages = null;
                //Model.User = null;
                Model.categories = null;
                Model.User = null;
                var entity = await _restaurantService.AddRestaurantAsync(_mapper.Map<Models.Restaurant>(Model));
                Model.Id = entity.Id;
                //if (Model.User.RequirePhoneNumberConfirmation)
                //    if (Model.PhoneNumber.StartsWith("971"))
                //        await SendOTP(User);
                //    else
                //        await SendConfirmationEmail(User);

                return Ok(Model);
            }

            return Ok(Model);
        }

        [HttpPut]
        public async Task<IActionResult> Put(RestaurantDTO Model)
        {
            IEnumerable<Models.Restaurant> List = await _restaurantService.GetRestaurantByIdAsync(Model.Id);
            AppUser user = await _userManager.FindByIdAsync(List.FirstOrDefault().UserId);
            if (List.Count() == 0)
                return Conflict(new ErrorDetails(409, "Invalid Id, No record found!", string.Empty));


            if (!string.IsNullOrEmpty(Model.Logo) && Model.Logo.Contains("Draft"))
            {
                string LogoPath = "/Images/Restaurant/" + Model.NameAsPerTradeLicense + "/";
                if (!string.IsNullOrEmpty(List.FirstOrDefault().Logo))
                    if (_fTPUpload.DeleteFile(List.FirstOrDefault().Logo))
                        List.FirstOrDefault().Logo = null;

                if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
                {
                    Model.Logo = LogoPath;
                }
                else
                    Model.Logo = null;
            }

            if (!string.IsNullOrEmpty(Model.SecondaryLogo) && Model.SecondaryLogo.Contains("Draft"))
            {
                string LogoPath = "/Images/Restaurant/" + Model.NameAsPerTradeLicense + "/";
                if (!string.IsNullOrEmpty(List.FirstOrDefault().SecondaryLogo))
                    if (_fTPUpload.DeleteFile(List.FirstOrDefault().SecondaryLogo))
                        List.FirstOrDefault().SecondaryLogo = null;

                if (_fTPUpload.MoveFile(Model.SecondaryLogo, ref LogoPath))
                {
                    Model.SecondaryLogo = LogoPath;
                }
                else
                    Model.SecondaryLogo = null;
            }

            if (!string.IsNullOrEmpty(Model.ThumbnailImage) && Model.ThumbnailImage.Contains("Draft"))
            {
                string LogoPath = "/Images/Restaurant/" + Model.NameAsPerTradeLicense + "/";
                if (!string.IsNullOrEmpty(List.FirstOrDefault().ThumbnailImage))
                    if (_fTPUpload.DeleteFile(List.FirstOrDefault().ThumbnailImage))
                        List.FirstOrDefault().ThumbnailImage = null;

                if (_fTPUpload.MoveFile(Model.ThumbnailImage, ref LogoPath))
                {
                    Model.ThumbnailImage = LogoPath;
                }
                else
                    Model.ThumbnailImage = null;
            }

            foreach (var images in Model.RestaurantImages)
            {
                string ImagePath = "/Images/RestaurantImages/" + Model.NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(images.Image, ref ImagePath))
                {
                    images.Image = ImagePath;
                }
                else
                    images.Image = null;
            }
            Models.Restaurant restaurant = List.FirstOrDefault();

            restaurant = _mapper.Map(Model, restaurant);

            if (Model.User != null && !user.PasswordHash.Equals(Model.User.PasswordHash))
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, Model.User.PasswordHash);
            }
            restaurant.User = null;

            if (restaurant.RestaurantBranches.Any())
                restaurant.RestaurantBranches.FirstOrDefault().City = null;


            return Ok(_mapper.Map<RestaurantDTO>(await _restaurantService.UpdateRestaurantAsync(restaurant)));
        }

        [HttpPut("EditAboutUsImage/{Id}")]
        public async Task<IActionResult> EditAboutUsImage(long Id)
        {
            IEnumerable<Models.Restaurant> List = await _restaurantService.GetRestaurantByIdAsync(Id);
            AppUser user = await _userManager.FindByIdAsync(List.FirstOrDefault().UserId);
            if (List.Count() == 0)
                return Conflict(new ErrorDetails(409, "Invalid Id, No record found!", string.Empty));

            Models.Restaurant restaurant = List.FirstOrDefault();
            restaurant.User = null;
            restaurant.DescriptionImage = null;
            return Ok(_mapper.Map<RestaurantDTO>(await _restaurantService.UpdateRestaurantAsync(restaurant)));
        }

        [HttpPut("UnAssignSupplierCode")]
        public async Task<IActionResult> UnAssignSupplierCode(RestaurantDTO Model)
        {
            IEnumerable<Models.Restaurant> List = await _restaurantService.GetRestaurantByIdAsync(Model.Id);
            Models.Restaurant restaurant = List.FirstOrDefault();

            restaurant = _mapper.Map(Model, restaurant);
            restaurant.SupplierCode = null;

            return Ok(_mapper.Map<RestaurantDTO>(await _restaurantService.UpdateRestaurantAsync(_mapper.Map<Models.Restaurant>(restaurant))));
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<RestaurantDTO>(await _restaurantService.ArchiveRestaurantAsync(Id)));
        }

        [HttpGet("{restaruantId}/DashboardStats/{branchId}")]
        public async Task<IActionResult> DashboardStats(long restaruantId, long branchId = 0)
        {
            IEnumerable<Order> orderList;

            if (branchId == 0)
                orderList = await _orderService.GetAllOrdersByRestaurant(restaruantId);
            else
                orderList = await _orderService.GetAllOrdersByBranch(branchId);

            return Ok(new SuccessResponse<DashboardStatsDTO>("Data received successfully", _mapper.Map<DashboardStatsDTO>(orderList.ToList())));
        }

        /* [HttpDelete("RestaurantImage/{Id}")]
         public async Task<IActionResult> DeleteRestaurantImage(long Id)
         {
             return Ok(_mapper.Map<RestaurantImagesDTO>(await _Service.ArchiveRestaurantImageAsync(Id)));
         }*/
        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<Models.Restaurant> restaurantList = await _restaurantService.GetRestaurantByIdAsync(Id);
            Models.Restaurant restaurant = restaurantList.FirstOrDefault();

            if (restaurant.Status == Enum.GetName(typeof(Status), Status.Active))
                restaurant.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                restaurant.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<RestaurantDTO>(await _restaurantService.UpdateRestaurantAsync(restaurant)));
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
    
    }
}
