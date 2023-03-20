using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HelperClasses.Classes;
using HelperClasses.DTOs.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Pages
{
    public class SignInModel : PageModel
    {
        private readonly IAuthenticateClient _authService;
        private readonly IUserSessionManager _sessionManager;
        private readonly IAuthorizationService _authorizationService;

        [BindProperty]
        public LoginViewModel LoginInfo { get; set; }

        public SignInModel(IAuthenticateClient authenticationClient, IUserSessionManager sessionManager,
                           IAuthorizationService authorizationService)
        {
            _authService = authenticationClient;
            _sessionManager = sessionManager;
            _authorizationService = authorizationService;
        }
        public async Task<IActionResult> OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                bool IsEmailConfirmed = await _authService.IsEmailConfirmedByName(User.Identity.Name);

                if (!IsEmailConfirmed)
                    return Page();

                if ((await _authorizationService.AuthorizeAsync(User, "Admin")).Succeeded)
                {
                    //BrandDTO UserBrand = _sessionManager.GetUserBrand();

                    //// Check if brand information is present in session cookie
                    //if (UserBrand.BrandId > 0)
                    return RedirectToAction("Index", "Home");
                    //else
                    //    return Page();
                }
                else
                {
                    string UserStoreInfo = User.Claims.Where(x => x.Type == "UserStoreInfo").Select(x => x.Value).FirstOrDefault();

                    if (string.IsNullOrEmpty(UserStoreInfo))
                        return Page();
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
                return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                //return RedirectPermanent("Admin/Dashboard/Index");
                string ReturnUrl = HttpContext.Request.Query["ReturnUrl"];
                UserAuthData authData = await _authService.LoginAsync(LoginInfo.Email, LoginInfo.Password, LoginInfo.RememberMe);

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

                        return RedirectToAction("Index", "Home");
                    //}
                        
                    //else
                    //{
                    //    //BrandDTO BrandInfo = _sessionManager.GetUserBrand();

                    //    //if (BrandInfo.SetupDone)
                    //    //{
                        

                    //    if (!String.IsNullOrEmpty(ReturnUrl) && (ReturnUrl != "/"))
                    //        return LocalRedirect(ReturnUrl); // if order to prevent malicious redirect
                                                             
                    //    return RedirectToAction("Index", "Home");
                    //    //}
                    //    //else
                    //    //{
                    //    //    return RedirectToPage("/InitialSetup");
                    //    //}
                    //}
                }
            }
            catch (Exception ex)
            {
                InjectErrorOnPage(ex.Message);
            }

            return Page();
        }

        private void InjectErrorOnPage(string ErrorMessage)
        {
            ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ErrorMessage);

            switch (err.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    {
                        ModelState.AddModelError("SignIn", "There was an error with your E-Mail/Password combination. " +
                                                 "Please try again.");
                        break;
                    }

                default:
                    {
                        ModelState.AddModelError("SignIn", "An unexpected error during SignIn. " +
                                                           "Please contact the site administrator");
                        break;
                    }
            }
        }

    }
}
