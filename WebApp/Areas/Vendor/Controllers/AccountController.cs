using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;
using System.Threading.Tasks;
using System.Security.Claims;
using System;
using WebApp.ErrorHandling;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using HelperClasses.DTOs;
using WebAPI.Models;
using WebAPI.Interfaces.IServices.Domains;
using DocumentFormat.OpenXml.Office2010.Excel;
using Newtonsoft.Json;

namespace WebApp.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    public class AccountController : Controller
    {
        private readonly IVendorAccountClient _authService;

        private readonly IUserSessionManager _sessionManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserClient _userService;
        private readonly ICountryClient _countryClient;
        private readonly ICityClient _cityClient;
        private readonly ISupplierClient _supplierClient;
        private readonly ISupplierPackageClient _supplierPackageClient;
        private readonly IMapper _mapper;
        private readonly IVendorClient _vendorClient;
        private readonly IVendorDocumentClient _vendorDocumentClient;
        public AccountController(IVendorAccountClient authenticationClient, IUserSessionManager sessionManager,IAuthorizationService authorizationService,
            IUserClient userService, IMapper mapper, 
            ICountryClient countryClient, ICityClient cityClient, 
            ISupplierClient supplierClient, IVendorClient vendorClient,
            ISupplierPackageClient supplierPackageClient, 
            IVendorDocumentClient vendorDocumentClient)
        {
            _authService = authenticationClient;
            _sessionManager = sessionManager;
            _authorizationService = authorizationService;
            _userService = userService;
            _mapper = mapper;
            _countryClient = countryClient;
            _cityClient = cityClient;
            _supplierClient = supplierClient;
            _vendorClient = vendorClient;
            _vendorDocumentClient = vendorDocumentClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("Vendor")]
        [HttpGet]
        public async Task<IActionResult> Login()
        {

            if (User.Identity.IsAuthenticated)
            {
                if ((await _authorizationService.AuthorizeAsync(User, "Vendor")).Succeeded)
                {
                    return RedirectPermanent("/Vendor/Dashboard/Index");
                }
                else
                {
                    string role = User.FindFirstValue(ClaimTypes.Role);

                    if (string.IsNullOrEmpty(role))
                    {
                        return View();
                    }
                    else
                    {
                        if (role == Enum.GetName(Roles.Vendor))
                            return RedirectPermanent("/Vendor/Dashboard/Index");
                        else
                            return View();
                    }
                }
            }
            else
                return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            string url = "";
            try
            {
                string ReturnUrl = HttpContext.Request.Query["ReturnUrl"];
                LoginResponse authData = await _authService.LoginAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe);

                if (authData != null)
                {
                    if (authData.Vendor.Status != "Active")
                    {
                        url = "/Vendor/Account/CompleteYourProfile";
                        return Json(new { success = true, url = url, message = "Login successfull" });
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(ReturnUrl) && (ReturnUrl != "/"))
                            return LocalRedirect(ReturnUrl); // if order to prevent malicious redirect

                        url = "/Vendor/Dashboard/Index";
                        return Json(new { success = true, url = url, message = "Login successfull" });
                    }
                    //if (!authData.AuthData.EmailConfirmed && authData.AuthData.UserRole.Name != Permissions.Admin.ToString())
                    //    return RedirectToPage("/ResendEmail");
                    // Cookie is not updated yet so pick data from the authdata claims rather than context claims or authentication
                    // service

                }
            }
            catch (ApiException ex)
            {
                return Json(new { success = false, url = string.Empty, message = "No User Found!" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, url = string.Empty, message = "Something went wrong!" });
            }

            return Json(new { success = false, message = "Something went wrong!" });
        }

        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(new AuthenticationProperties() { RedirectUri = "Vendor" });
            _sessionManager.ClearSession();
            return RedirectPermanent("/Vendor");
        }

        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> CompleteYourProfile()
        {
            long Id = _sessionManager.GetVendorStore().Id;
            var countries = _mapper.Map<IEnumerable<CountryViewModel>>(await _countryClient.GetCountries());
            ViewBag.Country = countries.OrderBy(x => x.Name);
            VendorViewModel vendor = _mapper.Map<VendorViewModel>(await _vendorClient.GetVendorByID(Id));
            vendor.VendorDocuments = (List<VendorDocumentViewModel>)await _vendorDocumentClient.GetAllByVendor(vendor.Id);
            return View(vendor);
        }
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> GetCityBycountryId(long countryId)
        {
            var cities = await _cityClient.GetCityByCountryId(countryId);

            return Json(new
            {
                success = true,
                data = cities
            });
        }
        [Authorize(Roles = "Vendor")]
        [HttpPost]
        public async Task<IActionResult> CompleteYourProfile(VendorViewModel model)
        {

            VendorViewModel vendor = _mapper.Map<VendorViewModel>(await _vendorClient.GetVendorByID(model.Id));
            model.Email = vendor.Email;
            model.NameArAsPerTradeLicense = vendor.NameArAsPerTradeLicense;
            model.UserId = vendor.UserId;
            model.RejectionReason = vendor.RejectionReason;
            if (vendor.Status == Enum.GetName(typeof(Status), Status.Active))
            {
                model.Status = Enum.GetName(typeof(Status), Status.Processing);
            }
            model.ContactNumber1 = model.ContactNumber1.Replace("-", "").Replace(" ", "");
            model.ContactNumber2 = model.ContactNumber2.Replace("-", "").Replace(" ", "");
            model.User.PhoneNumber = model.User.PhoneNumber.Replace("-", "").Replace(" ", "");
            var Result = await _vendorClient.Edit(model);
           
                return Json(new
            {
                success = true,
                message = "Record Updated Successfully",
                data = Result
            });
        }
        [Authorize(Roles = "Vendor")]
        [HttpPost]
        public async Task<IActionResult> Edit(VendorViewModel model)
        {
            VendorViewModel vendor = _mapper.Map<VendorViewModel>(await _vendorClient.GetVendorByID(model.Id));
            model.Email = vendor.Email;
            model.NameArAsPerTradeLicense = vendor.NameArAsPerTradeLicense;
            model.UserId = vendor.UserId;
            model.RejectionReason = vendor.RejectionReason;
            model.ContactNumber1 = model.ContactNumber1.Replace("-", "").Replace(" ", "");
            model.ContactNumber2 = model.ContactNumber2.Replace("-", "").Replace(" ", "");
            model.User.PhoneNumber = model.User.PhoneNumber.Replace("-", "").Replace(" ", "");
            object Result = await _vendorClient.Edit(model);

            return Json(new
            {
                success = true,
                message = "Record Updated Successfully",
                data = Result
            });
        }
        public IActionResult DocumentModel(long GarageId)
        {
            ViewBag.ID = GarageId;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(long VendorId)
        {
            VendorViewModel vendor = _mapper.Map<VendorViewModel>(await _vendorClient.GetVendorByID(VendorId));
            if(vendor.Status== Enum.GetName(typeof(Status), Status.Processing))
            {
                vendor.Status = Enum.GetName(typeof(Status), Status.Pending);
            }
            else
            {
                vendor.Status = Enum.GetName(typeof(Status), Status.Processing);
            }
            
            object Result = await _vendorClient.Edit(vendor);
            return Json(new
            {
                success = true,
                message = "Record Updated Successfully",
            });
        }
        public ActionResult VerifyOTp(string UserId, string PhoneNumber)
        {
            ViewBag.UserId = UserId;
            ViewBag.Contact = PhoneNumber;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> VerifyOTP(string UserId, string OTP)
        {
            try
            {
                int parse = Convert.ToInt16(OTP);
                string Otp = await _authService.VerifyOtp(UserId, parse);
                return Json(new { success = true, message = "OTP Verified", url = "/Vendor/Account/ResetPassword?UserId=" + UserId });
            }

            catch (ApiException ex)
            {
                ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                return Json(new { success = false, message = err.Message });
            }

            catch (Exception x)
            {
                return Json(new { success = false, message = x.Message });
            }


        }
        public ActionResult ResetPassword(string UserId)
        {
            ViewBag.UserId = UserId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ChangePasswordDTO resetPasswordViewModel)
        {
            try
            {

                bool Otp = await _authService.ResetPasswordAsync(resetPasswordViewModel);
                if (Otp)
                {
                    return Json(new { success = true, message = "Password reset successfully", url = "/Vendor" });
                }
                else
                {
                    return Json(new { success = false, message = "Something went wrong" });
                }

            }

            catch (ApiException)
            {
                return Json(new { success = false, message = "Something went wrong" });
            }


        }
        [HttpPost]
        public async Task<IActionResult> ResendOTP(string PhoneNumber)
        {
            if (PhoneNumber == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Phone number is required."
                });
            }
            string Message = string.Empty;
            try
            {

                if (ModelState.IsValid)
                {
                    string response = await _authService.ForgetPasswordAsync(PhoneNumber, Enum.GetName(typeof(Logins), Logins.Vendor));
                    if (response != null)
                    {
                        return Json(new
                        {
                            success = true,
                            message = "OTP Send Successfully"
                        });
                    }
                    else
                    {
                        Message = "Something went wrong";
                    }
                }
                else
                {
                    Message = "Please fill the form properly ...";
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Something went wrong" });
            }
            return Json(new { success = false, message = Message });
        }
        [HttpPost]

        public async Task<IActionResult> ForgetPassword(string PhoneNumber)
        {
            if (PhoneNumber == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Phone number is required."
                });
            }
            string Message = string.Empty;
            try
            {

                if (ModelState.IsValid)
                {
                    string response = await _authService.ForgetPasswordAsync(PhoneNumber, Enum.GetName(typeof(Logins), Logins.Vendor));
                    if (response != null)
                    {
                        return Json(new
                        {
                            success = true,
                            url = "Vendor/Account/VerifyOTp?UserId=" + response+ "&PhoneNumber=" + PhoneNumber,
                            message = "success"
                        });
                    }
                    else
                    {
                        Message = "Something went wrong";
                    }
                }
                else
                {
                    Message = "Please fill the form properly ...";
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Something went wrong" });
            }
            return Json(new { success = false, message = Message });
        }
        [Authorize(Roles = "Vendor")]
        public async Task<ActionResult> ChangePassword()
        {
            string userId = _sessionManager.GetVendorStore().UserId;
            //ViewBag.ProfilePicture = _sessionManager.GetVendorStore().Logo;
            var user = _mapper.Map<UserViewModel>(await _userService.GetUserByIdAsync(userId));
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Vendor")]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            string Message = "Invalid Data!";
            if (ModelState.IsValid)
            {
                string UserId = changePasswordViewModel.UserId;
                var password = await _authService.ChangePasswordAsync(changePasswordViewModel);
                if (password)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Password Changed Successfully"
                    });

                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Please Insert Correct Password"
                    });
                }
            }
            return Json(new { success = false, message = Message });

        }
    }
}
