using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.Garage.Filter;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

namespace WebAPI.Controllers.Garages
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin ,GarageOwner, Automobile Manager,Vendor")]
    public class GarageController : ControllerBase
    {
        private readonly IGarageService _garageService;
        private readonly IGarageSpecificationService _garageSpecificationService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly INumberRangeService _numberRangeService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<GarageController> _logger;

        public GarageController(IGarageService garageService,
            IUserService userService ,
        IMapper mapper, IFTPUpload ftpUpload, UserManager<AppUser> userManager, IGarageSpecificationService garageSpecificationService, INumberRangeService numberRangeService, ILogger<GarageController> logger)
        {
            _garageService = garageService;
            _fTPUpload = ftpUpload;
            _mapper = mapper;
            _userManager = userManager;
            _garageSpecificationService = garageSpecificationService;
            _userService = userService;
            _numberRangeService = numberRangeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<GarageDTO>>(await _garageService.GetAllGaragesAsync()));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<GarageDTO> List = _mapper.Map<IEnumerable<GarageDTO>>(await _garageService.GetGarageByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }
        [HttpGet("ForMobile/{Id}")]
        public async Task<IActionResult> GetByForMobileId(long Id)
        {
            IEnumerable<GarageDTO> List = _mapper.Map<IEnumerable<GarageDTO>>(await _garageService.GetGarageByIdAsync(Id));
            return Ok(new { Status = "Success", Data = List.FirstOrDefault() });
        }
        [HttpGet("ByUser")]
        public async Task<IActionResult> GetByUser()
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<GarageDTO> List = _mapper.Map<IEnumerable<GarageDTO>>(await _garageService.GetGarageByUserAsync(UserId));
            return Ok(List.FirstOrDefault());
        }
        [HttpGet("{VendorId}/ByVendor")]
        public async Task<IActionResult> GetByVendor(long VendorId)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<GarageDTO> List = _mapper.Map<IEnumerable<GarageDTO>>(await _garageService.GetGarageByVendorAsync(VendorId));
            return Ok(List);
        }
        [HttpPost]
        public async Task<IActionResult> AddGarage(GarageRegisterDTO Model)
        {
            IEnumerable<AppUser> users = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.Garage));
            Garage garage = new Garage();
            if (users.Any())
                throw new Exception("PhoneNumber already registered");
            
            var ExistingUsersListByEmail = await _userService.GetUserByEmailAndCheck(Model.Garage.ContactPersonEmail, Enum.GetName(typeof(Logins), Logins.Garage));
            if (ExistingUsersListByEmail.Any())
                throw new Exception("Email already registered");
            Random rnd = new();
            var User = new AppUser
            {
                UserName = (Model.Garage.NameAsPerTradeLicense + rnd.Next(1000, 9999).ToString()).Replace(" ", ""),
                Email = Model.Garage.ContactPersonEmail,
                FirstName = Model.Garage.NameAsPerTradeLicense,
                LastName = Model.Garage.NameAsPerTradeLicense,
                AuthCode = Model.PhoneNumber.Equals("971507567600") ? 1234 : rnd.Next(1000, 9999),
                AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2),
                LoginFor = Enum.GetName(typeof(Logins), Logins.Garage),
                PhoneNumber = Model.PhoneNumber,
                PhoneNumberConfirmed = true,
                IsActive = true
            };

            IdentityResult result = await _userManager.CreateAsync(User, Model.Password);

            if (result.Succeeded)
            {
                IdentityResult UserRoleResult = await _userManager.AddToRoleAsync(User, Enum.GetName(typeof(Roles), Roles.GarageOwner));

                if (UserRoleResult.Succeeded)
                {
                    Model.Garage.UserId = User.Id;
                    Model.Garage.ReferenceCode = await _numberRangeService.GetNumberRangeByName("Garage");
                    Model.Garage.Status = Enum.GetName(typeof(Status), Status.Draft);
                    Model.Garage.Slug = Slugify.GenerateSlug(Model.Garage.NameAsPerTradeLicense, Model.Garage.ReferenceCode);
                     garage = await _garageService.AddGarageAsync(_mapper.Map<Garage>(Model.Garage));
                }

                _logger.LogInformation("Registration Success for " + User.UserName);

                //if (Model.RequirePhoneNumberConfirmation)
                //    if (Model.PhoneNumber.StartsWith("971"))
                //        await SendOTP(User);
                //    else
                //        await SendConfirmationEmail(User);
                
                return Ok(new SuccessResponse<Garage>("", garage));
            }
            else
                return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, string.Empty));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateGarage(GarageDTO Model)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Garage> garageList = await _garageService.GetGarageByIdAsync(Model.Id);
            AppUser user = await _userManager.FindByIdAsync(garageList.FirstOrDefault().UserId);

            if (user != null && Model.User != null && Model.User.PhoneNumber != user.PhoneNumber)
            {
                IEnumerable<AppUser> userCheckByPhone = await _userService.GetUserByNumberAndCheck(Model.User.PhoneNumber, Enum.GetName(typeof(Logins), Logins.SparePartDealer));

                if (userCheckByPhone.Any())
                    return Conflict("This phone number is used by another account");
            }
            if (garageList.Count() == 0)
                return Conflict(new ErrorDetails(409, "Invalid Id, No record found!", string.Empty));

            if (!string.IsNullOrEmpty(Model.Logo))
                _fTPUpload.DeleteFile(garageList.FirstOrDefault().Logo);

            Garage garage = _mapper.Map(Model, garageList.FirstOrDefault());

            if (Model.Logo is not null && Model.Logo.Contains("Draft"))
            {
                string LogoPath = "/Images/Garage/" + garage.NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
                {
                    garage.Logo = LogoPath;
                }
            }

            garage.User = null;
            garage.Slug = Slugify.GenerateSlug(garage.NameAsPerTradeLicense, garage.ReferenceCode);
            GarageDTO result = _mapper.Map<GarageDTO>(await _garageService.UpdateGarageAsync(garage));

            AppUser AppUser = await _userManager.FindByIdAsync(UserId);
            AppUser.FirstName = result.NameAsPerTradeLicense;
            AppUser.LastName = result.NameAsPerTradeLicense;
            AppUser.Email = result.ContactPersonEmail;

            await _userManager.UpdateAsync(AppUser);

            return Ok(new SuccessResponse<GarageDTO>("", result));
        }
        [HttpPut("Vendor")]
        public async Task<IActionResult> UpdateVendorGarage(GarageDTO Model)
        {
            //string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Garage> garageList = await _garageService.GetGarageByIdAsync(Model.Id);
            AppUser user = await _userManager.FindByIdAsync(garageList.FirstOrDefault().UserId);

            if (user != null && Model.User != null && Model.User.PhoneNumber != user.PhoneNumber)
            {
                IEnumerable<AppUser> userCheckByPhone = await _userService.GetUserByNumberAndCheck(Model.User.PhoneNumber, Enum.GetName(typeof(Logins), Logins.SparePartDealer));

                if (userCheckByPhone.Any())
                    return Conflict("This phone number is used by another account");
            }
            if (garageList.Count() == 0)
                return Conflict(new ErrorDetails(409, "Invalid Id, No record found!", string.Empty));

            if (!string.IsNullOrEmpty(Model.Logo))
                _fTPUpload.DeleteFile(garageList.FirstOrDefault().Logo);

            Garage garage = _mapper.Map(Model, garageList.FirstOrDefault());

            if (Model.Logo is not null && Model.Logo.Contains("Draft"))
            {
                string LogoPath = "/Images/Garage/" + garage.NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
                {
                    garage.Logo = LogoPath;
                }
            }

            garage.User = null;
            garage.Slug = Slugify.GenerateSlug(garage.NameAsPerTradeLicense, garage.ReferenceCode);
            GarageDTO result = _mapper.Map<GarageDTO>(await _garageService.UpdateGarageAsync(garage));

            AppUser AppUser = await _userManager.FindByIdAsync(garageList.FirstOrDefault().UserId);
            AppUser.FirstName = result.NameAsPerTradeLicense;
            AppUser.LastName = result.NameAsPerTradeLicense;
            AppUser.Email = result.ContactPersonEmail;

            await _userManager.UpdateAsync(AppUser);

            return Ok(new SuccessResponse<GarageDTO>("", result));
        }
        [HttpGet("{Id}/ToggleActiveStatus/{flag}")]
        public async Task<IActionResult> ToggleActiveStatus(long Id, bool flag, string RejectionReason = "")
        {
            IEnumerable<Garage> List = await _garageService.GetGarageByIdAsync(Id);
            Garage garage = List.FirstOrDefault();

            if (garage.Status == Enum.GetName(typeof(Status), Status.Processing))
            {
                if (flag == false)
                {
                    garage.Status = Enum.GetName(typeof(Status), Status.Rejected);
                    garage.RejectionReason += RejectionReason + "<br />";
                }
                else if (flag == true)
                    garage.Status = Enum.GetName(typeof(Status), Status.Active);
            }
            else if (garage.Status == Enum.GetName(typeof(Status), Status.Active))
            {
                garage.Status = Enum.GetName(typeof(Status), Status.Inactive);
            }
            else if (garage.Status == Enum.GetName(typeof(Status), Status.Inactive))
            {
                garage.Status = Enum.GetName(typeof(Status), Status.Active);
            }

            garage.GarageRepairSpecifications = null;
            garage.GarageSchedules = null;
            garage.GarageImages = null;
            garage.GarageDocuments = null;
            garage.GarageRatings = null;

            garage = await _garageService.UpdateGarageAsync(garage);

            return Ok(garage);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GarageDTO>(await _garageService.ArchiveGarageAsync(Id)));
        }

        [HttpPost("{GarageId}/Specification/{CarMakeId}")]
        public async Task<IActionResult> AddRepairSpecification(long GarageId, long CarMakeId)
        {
            GarageRepairSpecification model = new()
            {
                GarageId = GarageId,
                CarMakeId = CarMakeId
            };

            return Ok(_mapper.Map<GarageRepairSpecificationDTO>(await _garageSpecificationService.Add(model)));
        }

        [HttpDelete("{GarageId}/Specification/{CarMakeId}")]
        public async Task<IActionResult> DeleteRepairSpecification(long GarageId, long CarMakeId)
        {
            IEnumerable<GarageRepairSpecification> result = await _garageSpecificationService.GetByIdandCarMake(GarageId, CarMakeId);
            await _garageSpecificationService.DeleteRepairSpecification(result.FirstOrDefault().Id);
            return Ok();
        }

        [HttpPut("{GarageId}/ProfilePicture")]
        public async Task<IActionResult> UpdateProfile(long GarageId, ThemeColorAndLogoDTO model)
        {
            IEnumerable<Garage> list = await _garageService.GetGarageByIdAsync(GarageId);

            if (model.Logo is not null && model.Logo.Contains("Draft"))
            {
                string LogoPath = "/Images/Garage/" + list.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(model.Logo, ref LogoPath))
                {
                    list.FirstOrDefault().Logo = LogoPath;
                }
            }

            list.FirstOrDefault().GarageRepairSpecifications = null;
            list.FirstOrDefault().GarageSchedules = null;

            Garage result = await _garageService.UpdateGarageAsync(list.FirstOrDefault());
            return Ok(result.Logo);
        }

        [HttpPut("{GarageId}/Theme")]
        public async Task<IActionResult> UpdateTheme(long GarageId, ThemeColorAndLogoDTO model)
        {
            IEnumerable<Garage> list = await _garageService.GetGarageByIdAsync(GarageId);

            list.FirstOrDefault().ThemeColor = model.ThemeColor;

            list.FirstOrDefault().GarageRepairSpecifications = null;
            list.FirstOrDefault().GarageSchedules = null;

            Garage result = await _garageService.UpdateGarageAsync(list.FirstOrDefault());
            return Ok(result.ThemeColor);
        }
        [HttpPut("{GarageId}/ThumbnailImage")]
        public async Task<IActionResult> ThumbnailImage(long GarageId, ThemeColorAndLogoDTO model)
        {
            IEnumerable<Garage> list = await _garageService.GetGarageByIdAsync(GarageId);

            if (!string.IsNullOrEmpty(model.Thumbnail))
            {
                string LogoPath = "/Images/Garage/" + list.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(model.Thumbnail, ref LogoPath))
                    list.FirstOrDefault().ThumbnailImage = LogoPath;
                else
                    list.FirstOrDefault().ThumbnailImage = model.Thumbnail;
            }

            list.FirstOrDefault().GarageRepairSpecifications = null;
            list.FirstOrDefault().GarageSchedules = null;

            Garage result = await _garageService.UpdateGarageAsync(list.FirstOrDefault());
            return Ok(result.ThumbnailImage);
        }

        [HttpPut("{GarageId}/SecondaryLogo")]
        public async Task<IActionResult> SecondaryLogo(long GarageId, ThemeColorAndLogoDTO model)
        {
            IEnumerable<Garage> list = await _garageService.GetGarageByIdAsync(GarageId);

            if (!string.IsNullOrEmpty(model.SecondaryLogo))
            {
                string LogoPath = "/Images/Garage/" + list.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(model.SecondaryLogo, ref LogoPath))
                    list.FirstOrDefault().SecondaryLogo = LogoPath;
                else
                    list.FirstOrDefault().SecondaryLogo = null;
            }

            list.FirstOrDefault().GarageRepairSpecifications = null;
            list.FirstOrDefault().GarageSchedules = null;

            Garage result = await _garageService.UpdateGarageAsync(list.FirstOrDefault());
            return Ok(result.SecondaryLogo);
        }
    }
}
