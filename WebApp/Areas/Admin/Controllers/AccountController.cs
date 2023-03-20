using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin, Automobile Manager, Restaurant Manager,  B2B Manager")]
    public class AccountController : Controller
    {
        string ErrorMessage = string.Empty;
        private readonly IAdminAccountClient _authService;
        private readonly IUserSessionManager _sessionManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserClient _userService;
        private readonly IMapper _mapper;
        //private readonly IUser

        //[BindProperty]
        //public LoginViewModel LoginInfo { get; set; }

        public AccountController(IAdminAccountClient authenticationClient, IUserSessionManager sessionManager,
                           IAuthorizationService authorizationService, IUserClient userService, IMapper mapper)
        {
            _authService = authenticationClient;
            _sessionManager = sessionManager;
            _authorizationService = authorizationService;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                bool IsEmailConfirmed = await _authService.IsEmailConfirmedByName(User.Identity.Name);

                if (!IsEmailConfirmed)
                    return View();

                if ((await _authorizationService.AuthorizeAsync(User, "Admin")).Succeeded)
                {
                    //BrandDTO UserBrand = _sessionManager.GetUserBrand();

                    //// Check if brand information is present in session cookie
                    //if (UserBrand.BrandId > 0)
                    return RedirectToAction("Index", "Dashboard");
                    //else
                    //    return Page();
                }
                else
                {
                    string UserStoreInfo = User.Claims.Where(x => x.Type == "UserStoreInfo").Select(x => x.Value).FirstOrDefault();

                    if (string.IsNullOrEmpty(UserStoreInfo))
                        return View();
                    else
                    {
                        //BrandDTO BrandInfo = JsonConvert.DeserializeObject<BrandDTO>(UserStoreInfo);

                        //if (BrandInfo.SetupDone)
                        return RedirectToAction("Privacy", "Home");
                        //else
                        //{
                        //    return RedirectToPage("/InitialSetup");
                        //}
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
            try
            {
                string ReturnUrl = HttpContext.Request.Query["ReturnUrl"];
                UserAuthData authData = await _authService.LoginAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe);

                if (authData != null)
                {
                    if (!authData.EmailConfirmed && authData.UserRole.Name != Permissions.Admin.ToString())
                        return RedirectToPage("/ResendEmail");
                    // Cookie is not updated yet so pick data from the authdata claims rather than context claims or authentication
                    // service
                    //else if (authData.UserRole.Name == Permissions.Admin.ToString())
                    //{
                    if (!String.IsNullOrEmpty(ReturnUrl) && (ReturnUrl != "/"))
                        return LocalRedirect(ReturnUrl); // if order to prevent malicious redirect

                    string url = "/Admin/Dashboard/Index";
                    return Json(new { success = true, url = url, message = "Login successfull" });

                }
            }
            catch (ApiException ex)
            {
                return Json(new { success = false, url = string.Empty, message = ex.Message });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, url = string.Empty, message = "Something went wrong!" });
            }

            return Json(new { success = false, message = "Something went wrong!" });
        }


        //[HttpGet]
        //public async Task<IActionResult> ForgetPassword()
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {

            string Message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    string response = await _authService.ForgetPasswordAsync(forgotPasswordViewModel.Email);


                    if (response != null)
                    {
                        return Json(new { success = true, url = "/Admin/Account/VerifyOTp?UserId=" + response + "&Email=" + forgotPasswordViewModel.Email, message = "success" });
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

        public ActionResult VerifyOTp(string UserId, string Email)
        {
            ViewBag.UserId = UserId;
            ViewBag.Email = Email;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResendOTP(string Email)
        {
            if (Email == null)
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

                if (ModelState.IsValid)
                {
                    string response = await _authService.ForgetPasswordAsync(Email);
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
        [HttpPost]
        public async Task<IActionResult> VerifyOTP(string UserId, string OTP)
        {
            try
            {
                int parse = Convert.ToInt16(OTP);
                string Otp = await _authService.VerifyOtp(UserId, parse);
                return Json(new { success = true, message = "OTP Verified", url = "/Admin/Account/ResetPassword?UserId=" + UserId });
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
                    return Json(new { success = true, message = "Password reset successfully", url = "/Admin/Account/Login" });
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

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(new AuthenticationProperties() { RedirectUri = "/Account/Login" });

            return RedirectPermanent("/Account/Login");
        }

        public async Task<ActionResult> ChangePassword()
        {
            string userId = _sessionManager.GetUser().UserId;
            var user = _mapper.Map<UserViewModel>(await _userService.GetUserByIdAsync(userId));
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            string Message = "Invalid Data!";
            if (ModelState.IsValid)
            {
                string UserId = changePasswordViewModel.UserId;
                if (await _userService.ChangePasswordAsync(UserId, changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword))
                {
                    return Json(new
                    {
                        success = true,
                        message = "Password Changed Successfully"
                    });

                }
                else
                {
                    ErrorMessage = Message;
                }
            }
            return Json(new { success = false, message = Message });

        }
    }
}
