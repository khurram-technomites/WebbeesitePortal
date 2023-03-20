﻿using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Supplier.Controllers
{
    [Area("Supplier")]

    public class AccountController : Controller
    {
        string ErrorMessage = string.Empty;
        private readonly ISupplierAccountClient _authService;
        private readonly IUserSessionManager _sessionManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserClient _userService;
        private readonly ICountryClient _countryClient;
        private readonly ICityClient _cityClient;
        private readonly ISupplierClient _supplierClient;
        private readonly ISupplierPackageClient _supplierPackageClient;
        private readonly IMapper _mapper;
        public AccountController(ISupplierAccountClient authenticationClient, IUserSessionManager sessionManager,
                           IAuthorizationService authorizationService, IUserClient userService, IMapper mapper, ICountryClient countryClient, ICityClient cityClient, ISupplierClient supplierClient, ISupplierPackageClient supplierPackageClient)
        {
            _authService = authenticationClient;
            _sessionManager = sessionManager;
            _authorizationService = authorizationService;
            _userService = userService;
            _mapper = mapper;
            _countryClient = countryClient;
            _cityClient = cityClient;
            _supplierClient = supplierClient;
            _supplierPackageClient = supplierPackageClient;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login()
        {

            if (User.Identity.IsAuthenticated)
            {
                if ((await _authorizationService.AuthorizeAsync(User, "Supplier")).Succeeded)
                {
                    return RedirectPermanent("/Supplier/Dashboard/Index");
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
                        if (role == Enum.GetName(Roles.Supplier))
                            return RedirectPermanent("/Supplier/Dashboard/Index");
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
                    if (!authData.AuthData.EmailConfirmed && authData.AuthData.UserRole.Name != Permissions.Admin.ToString())
                        return RedirectToPage("/ResendEmail");
                    // Cookie is not updated yet so pick data from the authdata claims rather than context claims or authentication
                    // service
                    //else if (authData.UserRole.Name == Permissions.Admin.ToString())
                    //{
                    if (!String.IsNullOrEmpty(ReturnUrl) && (ReturnUrl != "/"))
                        return LocalRedirect(ReturnUrl); // if order to prevent malicious redirect

                    if (authData.Supplier.Status == Enum.GetName(typeof(Status), Status.Pending) ||
                        authData.Supplier.Status == Enum.GetName(typeof(Status), Status.Processing) ||
                        authData.Supplier.Status == Enum.GetName(typeof(Status), Status.Rejected))
                        url = "/Supplier/Account/ProfileManagement";
                    else
                        url = "/Supplier/Dashboard/Index";
                    return Json(new { success = true, url = url, message = "Login successfull" });

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

        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(new AuthenticationProperties() { RedirectUri = "Supplier/Account/" });
            _sessionManager.ClearSession();
            return RedirectPermanent("Login");
        }
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> ProfileManagement()
        {

            IEnumerable<SupplierDTO> store = await _supplierClient.GetSupplierByIDAsync(_sessionManager.GetSupplierStore().Id);
            ViewBag.CountryId = _mapper.Map<IEnumerable<CountryViewModel>>(await _countryClient.GetCountriesByMaster()).OrderBy(x => x.Name);
            ViewBag.CityId = _mapper.Map<IEnumerable<CityViewModel>>(await _cityClient.GetCitiesMaster());
            ViewBag.PackageId = _mapper.Map<IEnumerable<SupplierPackageViewModel>>(await _supplierPackageClient.GetAllAsync());
            return View(_mapper.Map<SupplierViewModel>(store.FirstOrDefault()));
        }

        [HttpPost]
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> ProfileManagement(SupplierViewModel Model, string logoPath, string BankName, string ReferenceCode)
        {
            string Message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    Model.ReferenceCode = ReferenceCode;
                    Model.Bank = BankName;
                    Model.Logo = logoPath;
                    ViewBag.CountryId = _mapper.Map<IEnumerable<CountryViewModel>>(await _countryClient.GetCountriesByMaster()).OrderBy(x => x.Name);
                    ViewBag.CityId = _mapper.Map<IEnumerable<CityViewModel>>(await _cityClient.GetCitiesMaster());
                    ViewBag.PackageId = _mapper.Map<IEnumerable<SupplierPackageViewModel>>(await _supplierPackageClient.GetAllAsync());
                    var result = await _supplierClient.UpdateSupplierAsync(_mapper.Map<SupplierDTO>(Model));
                    return RedirectPermanent("/Supplier/Account/ProfileManagement");
                }
                else
                {
                    Message = "Please fill the form properly ...";
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Something went wrong", Url = "/Supplier/Account/ProfileManagement" });
            }
            return Json(new { success = false, message = Message });
        }
        //[Authorize(Roles = "Supplier")]
        public async Task<IActionResult> ToggleApprovalStatus(long Id)
        {
            SupplierDTO supplierDTO = await _supplierClient.ToggleApproveAsync(Id);

            return Json(new
            {
                success = true,
                message = string.Format("Profile {0} successfully", supplierDTO.Status == Enum.GetName(typeof(Status), Status.Processing) ? "sent for approval" : "approval canceled"),
                data = supplierDTO.Status
            });
        }
        [HttpPost]

        public async Task<IActionResult> ForgetPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (forgotPasswordViewModel.Email == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Email is required."
                });
            }
            string Message = string.Empty;
            try
            {
                //if (forgotPasswordViewModel)
                //{

                //}
                if (ModelState.IsValid)
                {
                    string response = await _authService.ForgetPasswordAsync(forgotPasswordViewModel.Email);
                    dynamic UserId = JObject.Parse(response);
                    if (response != null)
                    {
                        return Json(new
                        {
                            success = true,
                            url = "Supplier/Account/VerifyOTp?UserId=" + UserId.Result,
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
        public ActionResult VerifyOTp(string UserId)
        {
            ViewBag.UserId = UserId;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> VerifyOTP(string UserId, string OTP)
        {
            try
            {
                int parse = Convert.ToInt16(OTP);
                string Otp = await _authService.VerifyOtp(UserId, parse);
                return Json(new { success = true, message = "OTP Verified", url = "/Supplier/Account/ResetPassword?UserId=" + UserId });
            }

            catch (ApiException ex)
            {
                return Json(new { success = false, message = "Something went wrong" });
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
                    return Json(new { success = true, message = "Password reset successfully", url = "/Supplier/Account/Login" });
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
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> ChangePassword()
        {
            string userId = _sessionManager.GetSupplierStore().UserId;
            var user = _mapper.Map<UserViewModel>(await _userService.GetUserByIdAsync(userId));
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            string Message = "Invalid Data!";
            if (ModelState.IsValid)
            {
                string UserId = changePasswordViewModel.UserId;
                var password = await _authService.ChangePasswordAsync(changePasswordViewModel);
                if (password != null)
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
        [Authorize(Roles = "Supplier")]
        public IActionResult DocumentModel(long SupplierId)
        {
            return View(SupplierId);
        }
    }
}
