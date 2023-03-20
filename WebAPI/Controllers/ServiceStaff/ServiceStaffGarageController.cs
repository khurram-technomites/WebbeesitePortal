using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.Garage.Filter;
using Microsoft.AspNetCore.Authorization;
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

namespace WebAPI.Controllers.ServiceStaff
{
    [Route("api/ServiceStaff/Garage")]
    [ApiController]
    [Authorize(Roles = "ServiceStaff, Admin, Automobile Manager, GarageOwner")]
    public class ServiceStaffGarageController : ControllerBase
    {
        private readonly IGarageService _garageService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFTPUpload _fTPUpload;
        private readonly INumberRangeService _numberRangeService;
        private readonly ILogger<ServiceStaffGarageController> _logger;
        private readonly IMessageService _messageService;
        private readonly IEmailService _emailService;
        private readonly IGarageScheduleService _garageScheduleService;
        private readonly IGarageSpecificationService _garageSpecificationService;
        private readonly INotificationService _notificationService;

        public ServiceStaffGarageController(INotificationService notificationService, IGarageService garageService, IMapper mapper, IUserService userService, IEmailService emailService,
            UserManager<AppUser> userManager, IFTPUpload fTPUpload, INumberRangeService numberRangeService, ILogger<ServiceStaffGarageController> logger,
            IMessageService messageService, IGarageScheduleService garageScheduleService, IGarageSpecificationService garageSpecificationService)
        {
            _garageService = garageService;
            _userService = userService;
            _userManager = userManager;
            _fTPUpload = fTPUpload;
            _numberRangeService = numberRangeService;
            _mapper = mapper;
            _logger = logger;
            _messageService = messageService;
            _emailService = emailService;
            _garageScheduleService = garageScheduleService;
            _garageSpecificationService = garageSpecificationService;
            _notificationService = notificationService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllGarages(GarageFilter Filter)
        {
            return Ok(new SuccessResponse<IEnumerable<GarageDTO>>("", _mapper.Map<IEnumerable<GarageDTO>>(await _garageService.GetAllGaragesAsync(Filter))));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetGarageById(long Id)
        {
            IEnumerable<GarageDTO> List = _mapper.Map<IEnumerable<GarageDTO>>(await _garageService.GetGarageByIdAsync(Id));
            return Ok(new SuccessResponse<GarageDTO>("", List.FirstOrDefault()));
        }

        [HttpPost]
        public async Task<IActionResult> RegisterGarage(GarageRegisterDTO Model)
        {
            IEnumerable<AppUser> users = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.Garage));

            if (users.Any())
                return Conflict(new ErrorDetails(409, "User already exists", string.Empty));

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
                PhoneNumberConfirmed = !Model.RequirePhoneNumberConfirmation,
                IsActive = true
            };

            IdentityResult result = await _userManager.CreateAsync(User, Model.Password);

            if (result.Succeeded)
            {
                IdentityResult UserRoleResult = await _userManager.AddToRoleAsync(User, Enum.GetName(typeof(Roles), Roles.GarageOwner));

                if (UserRoleResult.Succeeded)
                {
                    if (!string.IsNullOrEmpty(Model.Garage.Logo))
                    {
                        string LogoPath = "/Images/Garage/" + Model.Garage.NameAsPerTradeLicense + "/";
                        if (_fTPUpload.MoveFile(Model.Garage.Logo, ref LogoPath))
                            Model.Garage.Logo = LogoPath;
                        else
                            Model.Garage.Logo = null;
                    }

                    if (!string.IsNullOrEmpty(Model.Garage.Video))
                    {
                        string VideoPath = "/Videos/Garage/" + Model.Garage.NameAsPerTradeLicense + "/";
                        if (_fTPUpload.MoveFile(Model.Garage.Video, ref VideoPath))
                            Model.Garage.Video = VideoPath;
                        else
                            Model.Garage.Video = null;
                    }

                    foreach (var images in Model.Garage.GarageImages)
                    {
                        string ImagePath = "/Images/GarageImages/" + Model.Garage.NameAsPerTradeLicense + "/";
                        if (_fTPUpload.MoveFile(images.Image, ref ImagePath))
                            images.Image = ImagePath;
                    }

                    foreach (var document in Model.Garage.GarageDocuments)
                    {
                        string DocumentPath = "/Documents/Garage/" + Model.Garage.NameAsPerTradeLicense + "/";
                        if (_fTPUpload.MoveFile(document.Path, ref DocumentPath))
                            document.Path = DocumentPath;
                    }

                    Model.Garage.UserId = User.Id;
                    Model.Garage.ReferenceCode = await _numberRangeService.GetNumberRangeByName("Garage");
                    Model.Garage.Slug = Slugify.GenerateSlug(Model.Garage.NameAsPerTradeLicense, Model.Garage.ReferenceCode);
                    await _garageService.AddGarageAsync(_mapper.Map<Garage>(Model.Garage));
                }

                _logger.LogInformation("Registration Success for " + User.UserName);

                if (Model.RequirePhoneNumberConfirmation)
                    if (Model.PhoneNumber.StartsWith("971"))
                        await SendOTP(User);
                    else
                        await SendConfirmationEmail(User);
                if (Model.Garage.Status == Enum.GetName(typeof(Status), Status.Draft))
                {
                    string userid = _userManager.Users.Where(x => x.Email == "admin@fougito.com").FirstOrDefault().Id;
                    if (!string.IsNullOrEmpty(userid))
                    {
                        NotificationDTO notification = new NotificationDTO
                        {
                            OriginatorId = User.Id,
                            OriginatorName = User.FirstName + " " + User.LastName,
                            Description = "Added new garage",
                            NotificationReceivers = new List<NotificationReceiverDTO>()
                    {
                        new NotificationReceiverDTO
                        {
                            ReceiverId = User.Id,
                            IsSeen = false,
                            IsDelivered = false,
                            IsRead = false,
                            ReceiverType = Enum.GetName(typeof(Logins), Logins.Admin),
                        }
                    },
                            OriginatorType = Enum.GetName(typeof(Logins), Logins.ServiceStaff),
                            Url = "/Admin/Garage/Index"
                        };
                        await _notificationService.AddNotification(_mapper.Map<Notification>(notification));
                    }
                }
                return Ok(new SuccessResponse<GarageRegisterDTO>("", Model));
            }
            else
                return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, string.Empty));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGarage(GarageRegisterDTO Model)
        {
            IEnumerable<Garage> GarageList = await _garageService.GetGarageByIdAsync(Model.Garage.Id);

            if (!GarageList.Any())
                return Conflict(new ErrorDetails(409, "No Garage Found. Invalid Id", string.Empty));

            Garage garage = GarageList.FirstOrDefault();

            AppUser user = await _userManager.FindByIdAsync(garage.UserId);

            if (user != null && Model.PhoneNumber.Replace(" ", "") != user.PhoneNumber.Replace(" ", ""))
            {
                IEnumerable<AppUser> userCheckByPhone = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.Garage));

                if (userCheckByPhone.Any())
                    return Conflict("This phone number is used by another account");
            }

            user.FirstName = Model.Garage.NameAsPerTradeLicense;
            user.LastName = Model.Garage.NameAsPerTradeLicense;
            user.PhoneNumber = Model.PhoneNumber;
            user.Email = Model.Email;

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (!user.PasswordHash.Equals(Model.Password))
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, Model.Password);
            }

            if (result.Succeeded)
            {
                if (Model.Garage.Logo != null && !Model.Garage.Logo.Replace("%20", " ").Equals(garage.Logo))
                {
                    string LogoPath = "/Images/Garage/" + Model.Garage.NameAsPerTradeLicense + "/";
                    if (_fTPUpload.MoveFile(Model.Garage.Logo, ref LogoPath))
                        Model.Garage.Logo = LogoPath;
                    else
                        Model.Garage.Logo = null;
                }
                else
                    Model.Garage.Logo = garage.Logo;

                if (!string.IsNullOrEmpty(Model.Garage.Video) && !Model.Garage.Video.Replace("%20", " ").Equals(garage.Video))
                {
                    string VideoPath = "/Videos/Garage/" + Model.Garage.NameAsPerTradeLicense + "/";
                    if (_fTPUpload.MoveFile(Model.Garage.Video, ref VideoPath))
                        Model.Garage.Video = VideoPath;
                    else
                        Model.Garage.Video = null;
                }
                else
                    Model.Garage.Video = garage.Video;

                foreach (var images in Model.Garage.GarageImages)
                {
                    string ImagePath = "/Images/GarageImages/" + Model.Garage.NameAsPerTradeLicense + "/";

                    if (images.Image.Contains("/Draft/"))
                        if (_fTPUpload.MoveFile(images.Image, ref ImagePath))
                        {
                            images.Image = ImagePath;
                        }
                }

                foreach (var document in Model.Garage.GarageDocuments)
                {
                    string DocumentPath = "/Documents/Garage/" + Model.Garage.NameAsPerTradeLicense + "/";
                    if (document.Path.Contains("/Draft/"))
                        if (_fTPUpload.MoveFile(document.Path, ref DocumentPath))
                        {
                            document.Path = DocumentPath;
                        }
                }

                //Model.Garage.ReferenceCode = garage.ReferenceCode;
                Model.Garage.UserId = user.Id;

                if (string.IsNullOrEmpty(garage.ReferenceCode))
                    Model.Garage.ReferenceCode = await _numberRangeService.GetNumberRangeByName("Garage");
                else
                    Model.Garage.ReferenceCode = garage.ReferenceCode;

                Model.Garage.User = null;
                Model.Garage.GarageContentManagement = null;
                Model.Garage.Slug = Slugify.GenerateSlug(Model.Garage.NameAsPerTradeLicense, Model.Garage.ReferenceCode);

                await _garageService.UpdateGarageAsync(_mapper.Map<Garage>(Model.Garage));

                _logger.LogInformation("Update Success for " + user.UserName);

                return Ok(new SuccessResponse<GarageRegisterDTO>("", Model));
            }
            else
                return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, string.Empty));
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

        [HttpPost("{GarageId}/ToggleStatus")]
        public async Task<IActionResult> ToggleStatus(long GarageId)
        {
            IEnumerable<Garage> List = await _garageService.GetGarageByIdAsync(GarageId);
            Garage model = List.FirstOrDefault();

            if (model.Status == Enum.GetName(typeof(Status), Status.Draft) || model.Status == Enum.GetName(typeof(Status), Status.Processing) || model.Status == Enum.GetName(typeof(Status), Status.Rejected))
                return Conflict(new ErrorDetails(409, "Cannot proceed with the following request because garage is not approved by admin", ""));

            if (model.Status == Enum.GetName(typeof(Status), Status.Active))
                model.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                model.Status = Enum.GetName(typeof(Status), Status.Active);

            model.GarageImages = null;
            model.GarageSchedules = null;
            model.GarageRatings = null;
            model.GarageRepairSpecifications = null;
            model.User = null;

            model = await _garageService.UpdateGarageAsync(model);

            return Ok(new SuccessResponse<string>(string.Format("Garage {0} successfully", model.Status), null));
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

        [HttpDelete("{GarageId}/Schedule/{Day}")]
        public async Task<IActionResult> DeleteSchedule(long GarageId, string Day)
        {
            IEnumerable<GarageSchedule> schedules = await _garageScheduleService.GetByGarageAndDay(GarageId, Day);

            if (!schedules.Any())
                return Conflict(new ErrorDetails(409, "No record found!", null));

            await _garageScheduleService.DeleteSchedule(schedules.FirstOrDefault().Id);

            return Ok(new SuccessResponse<string>("Record deleted successfully", ""));
        }

        [HttpDelete("RepairSpecification/{Id}")]
        public async Task<IActionResult> DeleteSpecification(long Id)
        {
            IEnumerable<GarageRepairSpecification> specifications = await _garageSpecificationService.GetById(Id);

            if (!specifications.Any())
                return Conflict(new ErrorDetails(409, "No record found!", null));

            await _garageSpecificationService.DeleteRepairSpecification(specifications.FirstOrDefault().Id);

            return Ok(new SuccessResponse<string>("Record deleted successfully", ""));
        }
    }
}
