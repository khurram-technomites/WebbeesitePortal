using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HelperClasses.DTOs;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebAPI.ErrorHandling;
using System;
using HelperClasses.Classes;
using WebAPI.ResponseWrapper;
using WebAPI.Interfaces.IServices;
using WebAPI.Services.Domains;
using HelperClasses.DTOs.Authentication;

namespace WebAPI.Controllers.Vendors
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin ,GarageOwner, Automobile Manager,Vendor")]
    public class VendorsController : Controller
    {
        private readonly IVendorService _vendorService;
     
       
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFTPUpload _ftpUpload;
        private readonly IVendorDocumentService _vendorDocumentService;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;

        public VendorsController(IVendorService vendorService,
            IUserService userService,
        IMapper mapper, UserManager<AppUser> userManager,
        INotificationService notificationService,IFTPUpload ftpUpload, 
        IVendorDocumentService vendorDocumentService, IEmailService emailService)
        {
            _vendorService = vendorService;
            _mapper = mapper;
            _userManager = userManager;
            _userService = userService;
            _ftpUpload = ftpUpload;
            _vendorDocumentService = vendorDocumentService;
            _notificationService = notificationService;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<VendorDTO>>(await _vendorService.GetAllAsync()));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<VendorDTO> List = _mapper.Map<IEnumerable<VendorDTO>>(await _vendorService.GetVendorByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }
        [HttpPost]
        public async Task<IActionResult> Create(VendorDTO model)
        {
            ErrorDetails err;
            IEnumerable<AppUser> users = await _userService.GetUserByNumberAndCheck(model.User.PhoneNumber, Enum.GetName(typeof(Logins), Logins.Vendor));

            if (users.Count() > 0 )
                return Conflict("User already exists");

            Random rnd = new();
            var User = new AppUser
            {
                UserName = (model.NameAsPerTradeLicense + rnd.Next(1000, 9999).ToString()).Replace(" ", ""),
                Email = model.Email,
                FirstName = model.NameAsPerTradeLicense,
                LastName = model.NameAsPerTradeLicense,
                NormalizedEmail = model.Email,
                AuthCode = model.User.PhoneNumber.Equals("971507567600") ? 1234 : rnd.Next(1000, 9999),
                AuthCodeExpiryTime = DateTime.UtcNow.AddMinutes(2),
                LoginFor = Enum.GetName(typeof(Logins), Logins.Vendor),
                PhoneNumber = model.User.PhoneNumber,
                IsActive = true,
                PhoneNumberConfirmed = true,
            };

            IdentityResult result = await _userManager.CreateAsync(User, model.User.Password);
            Vendor vendor = new Vendor();
            if (result.Succeeded)
            {
                IdentityResult UserRoleResult = await _userManager.AddToRoleAsync(User, Enum.GetName(typeof(Roles), Roles.Vendor));

                if (UserRoleResult.Succeeded)
                {
                    model.UserId = User.Id;
                    model.User = null;
                    model.Status = Enum.GetName(typeof(Status), Status.Active);
                     vendor= await _vendorService.AddVendorAsync(_mapper.Map<Vendor>(model));
                }
                if (vendor != null)
                {
                    if (model.VendorDocuments != null)
                    {
                        foreach (var document in model.VendorDocuments)
                        {
                            if (document.Path is not null && document.Path.Contains("Draft"))
                            {
                                string LogoPath = "/Documents/Vendor/" + model.Id + "/";
                                if (_ftpUpload.MoveFile(document.Path, ref LogoPath))
                                {
                                    document.Path = LogoPath;
                                }
                                document.VendorId = vendor.Id;
                                await _vendorDocumentService.AddDocument(_mapper.Map<Models.VendorDocument>(document));
                            }
                        }
                    }
                }
               

                //if (model.RequirePhoneNumberConfirmation)
                //    if (model.PhoneNumber.StartsWith("971"))
                //        await SendOTP(User);
                //    else
                //        await SendConfirmationEmail(User);


                return Ok(new SuccessResponse<VendorDTO>("", model));
            }
            else
                return BadRequest(new ErrorDetails(400, result.Errors.First<IdentityError>().Description, string.Empty));



        }
        [HttpPut]
        public async Task<IActionResult> UpdateVendor(VendorDTO Model)
        {
            var Admin = await _userService.GetUserByEmailAndCheck("admin@webbeesite.com", "Admin");
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Vendor> vendorList = await _vendorService.GetVendorByIdAsync(Model.Id);
            AppUser user = await _userManager.FindByIdAsync(vendorList.FirstOrDefault().UserId);

            if (user != null && Model.User != null && Model.User.PhoneNumber != user.PhoneNumber)
            {
                IEnumerable<AppUser> userCheckByPhone = await _userService.GetUserByNumberAndCheck(Model.User.PhoneNumber, Enum.GetName(typeof(Logins), Logins.SparePartDealer));

                if (userCheckByPhone.Any())
                    return Conflict("This phone number is used by another account");
            }
            if (vendorList.Count() == 0)
                return Conflict(new ErrorDetails(409, "Invalid Id, No record found!", string.Empty));

           

            Vendor vendor = _mapper.Map(Model, vendorList.FirstOrDefault());

            vendor.User = null;
            VendorDTO result = _mapper.Map<VendorDTO>(await _vendorService.UpdateVendorAsync(vendor));

            AppUser AppUser = await _userManager.FindByIdAsync(user.Id);
            AppUser.FirstName = result.NameAsPerTradeLicense;
            AppUser.LastName = result.FullName;
            AppUser.NormalizedEmail = result.Email;
            AppUser.Email = result.Email;
            AppUser.PhoneNumber = Model.User.PhoneNumber;
            await _userManager.UpdateAsync(AppUser);
            if(Model.User.Password != null)
            {
                if (!user.PasswordHash.Equals(Model.User.Password))
                {
                    string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, Model.User.Password);
                }
            }
            if (result.Status == "Processing")
            {
                Notification notification = new()
                {
                    OriginatorId = vendor.UserId,
                    OriginatorName = vendor.Email,
                    Description = "New vendor listed for approval",
                    RecordId = result.Id,
                    OriginatorType = Enum.GetName(typeof(Logins), Logins.Vendor),
                    Url = "/Admin/Vendor/Index",
                    NotificationReceivers = new List<NotificationReceiver>()
                {
                    new NotificationReceiver
                    {
                        ReceiverId = Admin.FirstOrDefault().Id,
                        IsSeen = false,
                        IsDelivered = false,
                        IsRead = false,
                        ReceiverType = Enum.GetName(typeof(Logins), Logins.Admin),
                    }
                }
                };
                await _notificationService.AddNotification(notification);
            }



            return Ok(new SuccessResponse<VendorDTO>("", result));
        }
        [HttpGet("{Id}/ToggleActiveStatus/{flag}")]
        public async Task<IActionResult> ToggleActiveStatus(long Id, bool flag, string RejectionReason = "")
        {
            IEnumerable<Vendor> List = await _vendorService.GetVendorByIdAsync(Id);
            Vendor vendor = List.FirstOrDefault();
            AppUser user = await _userManager.FindByIdAsync(vendor.UserId);

            if (vendor.Status == Enum.GetName(typeof(Status), Status.Processing))
            {
                if (flag == false)
                {
                    vendor.Status = Enum.GetName(typeof(Status), Status.Rejected);
                    vendor.RejectionReason += "<br/>" + RejectionReason ;
                    Notification notification = new()
                    {
                        OriginatorId = user.Id,
                        OriginatorName = user.Email,
                        Description = "Your profile has been rejected.!",
                        RecordId = vendor.Id,
                        OriginatorType = Enum.GetName(typeof(Logins), Logins.Admin),
                        Url = "/Vendor/Dashboard/Index",
                        NotificationReceivers = new List<NotificationReceiver>()
                {
                    new NotificationReceiver
                    {
                        ReceiverId = vendor.UserId,
                        IsSeen = false,
                        IsDelivered = false,
                        IsRead = false,
                        ReceiverType = Enum.GetName(typeof(Logins), Logins.Vendor),
                    }
                }
                    };
                    await _notificationService.AddNotification(notification);
                }
                else if (flag == true)
                { 
                    vendor.Status = Enum.GetName(typeof(Status), Status.Active);
                    //ProfileApproval.cs
                    await _emailService.SendProfileApprovalEmail(new ConfirmEmailDTO()
                    {
                        Title = "Webbeesite - Vendor Account Approved",
                        Email = vendor.Email,
                        UserName = vendor.NameAsPerTradeLicense,
                    });
                    Notification notification = new()
                {
                    OriginatorId = user.Id,
                    OriginatorName = user.Email,
                    Description = "Your profile has been approved.!",
                    RecordId = vendor.Id,
                    OriginatorType = Enum.GetName(typeof(Logins), Logins.Admin),
                    Url = "/Vendor/Dashboard/Index",
                    NotificationReceivers = new List<NotificationReceiver>()
                {
                    new NotificationReceiver
                    {
                        ReceiverId = vendor.UserId,
                        IsSeen = false,
                        IsDelivered = false,
                        IsRead = false,
                        ReceiverType = Enum.GetName(typeof(Logins), Logins.Vendor),
                    }
                }
                };
                await _notificationService.AddNotification(notification);
                }
            }
            else if (vendor.Status == Enum.GetName(typeof(Status), Status.Active))
            {
                vendor.Status = Enum.GetName(typeof(Status), Status.Inactive);
                user.IsActive = false;
                await _userManager.UpdateAsync(user);
            }
            else if (vendor.Status == Enum.GetName(typeof(Status), Status.Inactive))
            {
                vendor.Status = Enum.GetName(typeof(Status), Status.Active);
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
            }

            vendor.User = null;

            vendor = await _vendorService.UpdateVendorAsync(vendor);

            return Ok(vendor);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            IEnumerable<VendorDTO> List = _mapper.Map<IEnumerable<VendorDTO>>(await _vendorService.GetVendorByIdAsync(Id));
            AppUser user = await _userManager.FindByIdAsync(List.FirstOrDefault().UserId);
            user.IsDeleted = true;
            await _userManager.UpdateAsync(user);
            return Ok(_mapper.Map<VendorDTO>(await _vendorService.ArchiveVendorAsync(Id)));
        }
    }
}
