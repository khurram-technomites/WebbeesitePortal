using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISupplierAccountClient
    {
        Task<LoginResponse> LoginAsync(string Username, string Password, bool RememberMe);
        Task<bool> IsEmailConfirmedByName(string Name);
        Task<bool> ChangePasswordAsync(ChangePasswordViewModel changePasswordViewModel);
        Task<LoginResponse> RegisterAsync(RegisterData Data);
        Task<LoginResponse> ResendOTPAsync(string Contact);
        Task<string> VerifyOtp(string userId, int code);
        Task<string> ForgetPasswordAsync(string Email);
        Task<bool> ResetPasswordAsync(ChangePasswordDTO ResetData);
    }
}
