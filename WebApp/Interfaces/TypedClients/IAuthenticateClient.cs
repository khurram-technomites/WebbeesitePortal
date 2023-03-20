using HelperClasses.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IAuthenticateClient
    {
        Task<UserAuthData> LoginAsync(string Username, string Password, bool RememberMe);
        Task<LoginResponse> RegisterAsync(RegisterData Data);
        Task<LoginResponse> ResendConfirmEmailAsync(string Email);
        Task<LoginResponse> ConfirmEmailAsync(string UserId, int Code);
        Task<LoginResponse> ForgetPasswordAsync(string Email);
        Task ResetPasswordAsync(ResetPasswordDTO ResetData);
        Task<bool> IsEmailConfirmedByName(string Name);
    }
}
