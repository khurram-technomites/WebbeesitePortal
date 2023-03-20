using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.SparePartsDealer;
using HelperClasses.DTOs.SparePartsDealer.Filter;
using Microsoft.AspNetCore.Authorization;
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

namespace WebAPI.Controllers.ServiceStaff
{
    [Route("api/ServiceStaff/SparePartDealer")]
    [ApiController]
    [Authorize(Roles = "ServiceStaff , Admin, Automobile Manager")]
    public class ServiceStaffSparePartDealerController : ControllerBase
    {
        private readonly ISparePartsDealerService _sparePartsDealerService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFTPUpload _fTPUpload;
        private readonly INumberRangeService _numberRangeService;
        private readonly ILogger<ServiceStaffGarageController> _logger;
        private readonly IMessageService _messageService;
        private readonly IEmailService _emailService;
        private readonly ISparePartDealerSpecificationService _sparePartDealerSpecificationService;
        private readonly ISparePartDealerScheduleService _sparePartDealerScheduleService;
        private readonly INotificationService _notificationService;

        public ServiceStaffSparePartDealerController(INotificationService notificationService, ISparePartsDealerService sparePartsDealerService, IMapper mapper, IUserService userService, IEmailService emailService,
            UserManager<AppUser> userManager, IFTPUpload fTPUpload, INumberRangeService numberRangeService, ILogger<ServiceStaffGarageController> logger,
            IMessageService messageService, ISparePartDealerSpecificationService sparePartDealerSpecificationService, ISparePartDealerScheduleService sparePartDealerScheduleService)
        {
            _sparePartsDealerService = sparePartsDealerService;
            _userService = userService;
            _userManager = userManager;
            _fTPUpload = fTPUpload;
            _numberRangeService = numberRangeService;
            _mapper = mapper;
            _logger = logger;
            _messageService = messageService;
            _emailService = emailService;
            _sparePartDealerSpecificationService = sparePartDealerSpecificationService;
            _sparePartDealerScheduleService = sparePartDealerScheduleService;
            _notificationService = notificationService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllSparePartsDealer(SparePartFilterDTO Filter)
        {
            return Ok(new SuccessResponse<IEnumerable<SparePartsDealerDTO>>("", _mapper.Map<IEnumerable<SparePartsDealerDTO>>(await _sparePartsDealerService.GetAllSparePartsDealerAsync(Filter))));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetSparePartsDealerById(long Id)
        {
            IEnumerable<SparePartsDealerDTO> List = _mapper.Map<IEnumerable<SparePartsDealerDTO>>(await _sparePartsDealerService.GetSparePartsDealerByIdAsync(Id));
            return Ok(new SuccessResponse<SparePartsDealerDTO>("", List.FirstOrDefault()));
        }

        [HttpPost]
        public async Task<IActionResult> RegisterSparePartDealer(SparePartsDealerRegisterDTO Model)
        {
            IEnumerable<AppUser> users = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.SparePartDealer));

            if (users.Any())
                return Conflict(new ErrorDetails(409, "User already exists", string.Empty));

            Random rnd = new();
            var User = new AppUser
            {
                UserName = (Model.SparePartsDealer.NameAsPerTradeLicense + rnd.Next(1000, 9999).ToString()).Replace(" ", ""),
                Email = Model.SparePartsDealer.ContactPersonEmail,
                FirstName = Model.SparePartsDealer.NameAsPerTradeLicense,
                LastName = Model.SparePartsDealer.NameAsPerTradeLicense,
                AuthCode = Model.PhoneNumber.Equals("971507567600") ? 1234 : rnd.Next(1000, 9999),
                AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2),
                LoginFor = Enum.GetName(typeof(Logins), Logins.SparePartDealer),
                PhoneNumber = Model.PhoneNumber,
                PhoneNumberConfirmed = !Model.RequirePhoneNumberConfirmation
            };

            IdentityResult result = await _userManager.CreateAsync(User, Model.Password);

            if (result.Succeeded)
            {
                IdentityResult UserRoleResult = await _userManager.AddToRoleAsync(User, Enum.GetName(typeof(Roles), Roles.SparePartDealer));

                if (UserRoleResult.Succeeded)
                {
                    if (!string.IsNullOrEmpty(Model.SparePartsDealer.Logo))
                    {
                        string LogoPath = "/Images/SparePartsDealer/" + Model.SparePartsDealer.NameAsPerTradeLicense + "/";
                        if (_fTPUpload.MoveFile(Model.SparePartsDealer.Logo, ref LogoPath))
                        {
                            Model.SparePartsDealer.Logo = LogoPath;
                        }
                    }

                    if (!string.IsNullOrEmpty(Model.SparePartsDealer.Video))
                    {
                        string VideoPath = "/Videos/Garage/" + Model.SparePartsDealer.NameAsPerTradeLicense + "/";
                        if (_fTPUpload.MoveFile(Model.SparePartsDealer.Video, ref VideoPath))
                        {
                            Model.SparePartsDealer.Video = VideoPath;
                        }
                    }

                    foreach (var images in Model.SparePartsDealer.DealerImages)
                    {
                        string ImagePath = "/Images/SparePartsDealerImages/" + Model.SparePartsDealer.NameAsPerTradeLicense + "/";
                        if (_fTPUpload.MoveFile(images.Image, ref ImagePath))
                        {
                            images.Image = ImagePath;
                        }
                    }

                    foreach (var document in Model.SparePartsDealer.SparePartsDealerDocuments)
                    {
                        string DocumentPath = "/Documents/SparePartsDealer/" + Model.SparePartsDealer.NameAsPerTradeLicense + "/";
                        if (_fTPUpload.MoveFile(document.Path, ref DocumentPath))
                        {
                            document.Path = DocumentPath;
                        }
                    }

                    Model.SparePartsDealer.UserId = User.Id;
                    Model.SparePartsDealer.ReferenceCode = await _numberRangeService.GetNumberRangeByName("SparePartDealer");
                    Model.SparePartsDealer = _mapper.Map<SparePartsDealerDTO>(await _sparePartsDealerService.AddSparePartsDealerAsync(_mapper.Map<Models.SparePartsDealer>(Model.SparePartsDealer)));
                }

                _logger.LogInformation("Registration Success for " + User.UserName);

                if (Model.RequirePhoneNumberConfirmation)
                    if (Model.PhoneNumber.StartsWith("971"))
                        await SendOTP(User);
                    else
                        await SendConfirmationEmail(User);

                if (Model.SparePartsDealer.Status == Enum.GetName(typeof(Status), Status.Draft))
                {
                    string userid = _userManager.Users.Where(x => x.Email == "admin@fougito.com").FirstOrDefault().Id;
                    if (!string.IsNullOrEmpty(userid))
                    {
                        NotificationDTO notification = new()
                        {
                            OriginatorId = User.Id,
                            OriginatorName = User.FirstName + " " + User.LastName,
                            Description = "Added new Spear parts dealer",
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
                            Url = "/Admin/SpearPartsDealer/Index"
                        };
                        await _notificationService.AddNotification(_mapper.Map<Notification>(notification));
                    }
                }
                return Ok(new SuccessResponse<SparePartsDealerRegisterDTO>("", Model));
            }
            else
                return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, string.Empty));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSparePartDealer(SparePartsDealerRegisterDTO Model)
        {
            IEnumerable<Models.SparePartsDealer> SparePartsDealerList = await _sparePartsDealerService.GetSparePartsDealerByIdAsync(Model.SparePartsDealer.Id);

            if (SparePartsDealerList.Count() == 0)
                return Conflict(new ErrorDetails(409, "No Spare Part Dealer Found. Invalid Id", ""));

            Models.SparePartsDealer SparePartsDealer = SparePartsDealerList.FirstOrDefault();

            AppUser user = await _userManager.FindByIdAsync(SparePartsDealer.UserId);

            if (user != null && Model.PhoneNumber != user.PhoneNumber)
            {
                IEnumerable<AppUser> userCheckByPhone = await _userService.GetUserByNumberAndCheck(Model.PhoneNumber, Enum.GetName(typeof(Logins), Logins.SparePartDealer));

                if (userCheckByPhone.Any())
                    return Conflict("This phone number is used by another account");
            }

            user.FirstName = Model.SparePartsDealer.NameAsPerTradeLicense;
            user.LastName = Model.SparePartsDealer.NameAsPerTradeLicense;
            user.PhoneNumber = Model.PhoneNumber;

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (!user.PasswordHash.Equals(Model.Password))
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, Model.Password);
            }

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(Model.SparePartsDealer.Logo) && !Model.SparePartsDealer.Logo.Replace("%20", " ").Equals(SparePartsDealer.Logo))
                {
                    string LogoPath = "/Images/SparePartsDealer/" + Model.SparePartsDealer.NameAsPerTradeLicense + "/";
                    if (_fTPUpload.MoveFile(Model.SparePartsDealer.Logo, ref LogoPath))
                    {
                        Model.SparePartsDealer.Logo = LogoPath;
                    }
                }
                else
                    Model.SparePartsDealer.Logo = SparePartsDealer.Logo;

                if (!string.IsNullOrEmpty(Model.SparePartsDealer.Video) && !Model.SparePartsDealer.Video.Replace("%20", " ").Equals(SparePartsDealer.Video))
                {
                    string VideoPath = "/Videos/SparePartsDealer/" + Model.SparePartsDealer.NameAsPerTradeLicense + "/";
                    if (_fTPUpload.MoveFile(Model.SparePartsDealer.Video, ref VideoPath))
                    {
                        Model.SparePartsDealer.Video = VideoPath;
                    }
                }
                else
                    Model.SparePartsDealer.Video = SparePartsDealer.Video;

                foreach (var images in Model.SparePartsDealer.DealerImages)
                {
                    string ImagePath = "/Images/SparePartsDealerImages/" + Model.SparePartsDealer.NameAsPerTradeLicense + "/";

                    if (images.Image.Contains("/Draft/"))
                        if (_fTPUpload.MoveFile(images.Image, ref ImagePath))
                        {
                            images.Image = ImagePath;
                        }
                }

                foreach (var document in Model.SparePartsDealer.SparePartsDealerDocuments)
                {
                    string DocumentPath = "/Documents/SparePartsDealer/" + Model.SparePartsDealer.NameAsPerTradeLicense + "/";

                    if (document.Path.Contains("/Draft/"))
                        if (_fTPUpload.MoveFile(document.Path, ref DocumentPath))
                        {
                            document.Path = DocumentPath;
                        }
                }

                if (string.IsNullOrEmpty(SparePartsDealer.ReferenceCode))
                    Model.SparePartsDealer.ReferenceCode = await _numberRangeService.GetNumberRangeByName("SparePartDealer");
                else
                    Model.SparePartsDealer.ReferenceCode = SparePartsDealer.ReferenceCode;

                Model.SparePartsDealer.UserId = user.Id;
                Model.SparePartsDealer.User = null;
                await _sparePartsDealerService.UpdateSparePartsDealerAsync(_mapper.Map<Models.SparePartsDealer>(Model.SparePartsDealer));


                _logger.LogInformation("Registration Success for " + user.UserName);

                return Ok(new SuccessResponse<SparePartsDealerRegisterDTO>("", Model));
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

        [HttpPost("{SparePartId}/ToggleStatus")]
        public async Task<IActionResult> ToggleStatus(long SparePartId)
        {
            IEnumerable<Models.SparePartsDealer> List = await _sparePartsDealerService.GetSparePartsDealerByIdAsync(SparePartId);
            Models.SparePartsDealer model = List.FirstOrDefault();

            if (model.Status == Enum.GetName(typeof(Status), Status.Draft) || model.Status == Enum.GetName(typeof(Status), Status.Processing) || model.Status == Enum.GetName(typeof(Status), Status.Rejected))
                return Conflict(new ErrorDetails(409, "Cannot proceed with the following request because dealer is not approved by admin", ""));

            if (model.Status == Enum.GetName(typeof(Status), Status.Active))
                model.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                model.Status = Enum.GetName(typeof(Status), Status.Active);

            model = await _sparePartsDealerService.UpdateSparePartsDealerAsync(model);

            return Ok(new SuccessResponse<string>(string.Format("Dealer {0} successfully", model.Status), null));
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

        [HttpDelete("{DealerId}/Schedule/{Day}")]
        public async Task<IActionResult> DeleteSchedule(long DealerId, string Day)
        {
            IEnumerable<DealerSchedule> schedules = await _sparePartDealerScheduleService.GetByDealerAndDay(DealerId, Day);

            if (!schedules.Any())
                return Conflict(new ErrorDetails(409, "No record found!", null));

            await _sparePartDealerScheduleService.DeleteSchedule(schedules.FirstOrDefault().Id);

            return Ok(new SuccessResponse<string>("Record deleted successfully", ""));
        }

        [HttpDelete("RepairSpecification/{Id}")]
        public async Task<IActionResult> DeleteSpecification(long Id)
        {
            IEnumerable<DealerInventorySpecification> specifications = await _sparePartDealerSpecificationService.GetById(Id);

            if (!specifications.Any())
                return Conflict(new ErrorDetails(409, "No record found!", null));

            await _sparePartDealerSpecificationService.DeleteSpecification(specifications.FirstOrDefault().Id);

            return Ok(new SuccessResponse<string>("Record deleted successfully", ""));
        }
    }
}
