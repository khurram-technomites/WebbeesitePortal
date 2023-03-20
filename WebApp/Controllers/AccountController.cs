using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    //[DisplayName("Accounts")]
    public class AccountController : Controller
    {
        string ErrorMessage = string.Empty;
        private readonly IAdminAccountClient _authService;
        private readonly IUserSessionManager _sessionManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserClient _userService;
        private readonly IMapper _mapper;
        private readonly ITokenManager _tokenManager;
        //private readonly IUser

        //[BindProperty]
        //public LoginViewModel LoginInfo { get; set; }

        public AccountController(IAdminAccountClient authenticationClient, IUserSessionManager sessionManager,
                           IAuthorizationService authorizationService, IMapper mapper, IUserClient userService, ITokenManager tokenManager)
        {
            _authService = authenticationClient;
            _sessionManager = sessionManager;
            _authorizationService = authorizationService;
            _userService = userService;
            _mapper = mapper;
            _tokenManager = tokenManager;
        }


        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                if ((await _authorizationService.AuthorizeAsync(User, "Admin")).Succeeded)
                {
                    return RedirectPermanent("/Admin/Dashboard/Index");
                }
                else
                {
                    string role = User.FindFirstValue(ClaimTypes.Role);

                    if (string.IsNullOrEmpty(role))
                    {
                        _sessionManager.ClearSession();
                        return View();
                    }
                    else
                    {
                        if (role.Contains("Admin"))
                            return RedirectPermanent("/Admin/Dashboard/Index");
                        else if (role == Enum.GetName(Roles.RestaurantOwner))
                            return RedirectPermanent("/Restaurant/Dashboard/Index");
                        else if (role == Enum.GetName(Roles.Supplier))
                            return RedirectPermanent("/Supplier/Dashboard/Index");
                        else
                        {
                            _sessionManager.ClearSession();
                            return View();
                        }                            
                    }
                }
            }
            else
            {
                _sessionManager.ClearSession();
                return View();
            }

        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(new AuthenticationProperties() { RedirectUri = "/Account/Login" });
            _sessionManager.ClearSession();

            return RedirectPermanent("/Account/Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, [FromQuery(Name = "ReturnUrl")] string ReturnUrl)
        {
            try
            {
                UserAuthData authData = await _authService.LoginAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe);

                if (authData != null)
                {

                    if (!authData.EmailConfirmed && authData.UserRole.Name != Permissions.Admin.ToString())
                        return RedirectToPage("/ResendEmail");

                    if (!String.IsNullOrEmpty(ReturnUrl) && (ReturnUrl != "/"))
                        return LocalRedirect(ReturnUrl); // if order to prevent malicious redirect


                    if (authData.UserRole.Name == "Admin" || authData.UserRole.Name == "Automobile Manager" || authData.UserRole.Name == "Restaurant Manager" || authData.UserRole.Name == "B2B Manager")
                    {
                        string url = "/Admin/Dashboard/Index";
                        return Json(new { success = true, url = url, message = "Login successfull" });
                    }

                    else if (authData.UserRole.Name == Enum.GetName(Roles.RestaurantOwner))
                    {
                        string url = "/Restaurant/Dashboard/Index";
                        return Json(new { success = true, url = url, message = "Login successfull" });
                    }
                }
            }
            catch (ApiException ex)
            {
                if (ex.Message.Contains("Account Suspended!"))
                {
                    return Json(new { success = false, url = string.Empty, message = "Account Suspended!" });
                }
                if (ex.Message.Contains("No User Found!"))
                {
                    return Json(new { success = false, url = string.Empty, message = "No User Found!" });
                }
                else
                {
                    return Json(new { success = false, url = string.Empty, message = "Something went wrong!" });
                }


            }
            catch (Exception)
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
                        return Json(new { success = true, url = "/Account/VerifyOTp?UserId=" + response + "&Email=" + forgotPasswordViewModel.Email, message = "Forget Password Link Successfully Sent!" });
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
            catch (Exception ex)
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
        public async Task<IActionResult> VerifyOTP(string UserId, string OTP)
        {
            try
            {
                int parse = Convert.ToInt16(OTP);
                string Otp = await _authService.VerifyOtp(UserId, parse);
                return Json(new { success = true, message = "OTP Verified", url = "/Account/ResetPassword?UserId=" + UserId });
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
                    return Json(new { success = true, message = "Password reset successfully", url = "/Account/Login" });
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

        public async Task<ActionResult> ChangePassword()
        {

            string userId = _sessionManager.GetUser().UserId;
            string image = _sessionManager.GetUserStore().Logo;
            ViewBag.RestaurantImage = image;
            var user = _mapper.Map<UserViewModel>(await _userService.GetUserByIdAsync(userId));
            return View(user);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        /*[Authorize]*/
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

        [HttpPost]
        public IActionResult RefreshToken()
        {
            if (_tokenManager.ForcefullyRefreshAccessToken())
                return Json(new { success = true, message = "Session extended successfully" });
            else
                return Json(new { success = false, message = "" });
        }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            await _authService.test();

            return Ok();
        }
    }
}