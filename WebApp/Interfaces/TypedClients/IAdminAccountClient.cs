using HelperClasses.DTOs.Authentication;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IAdminAccountClient
    {
        Task<UserAuthData> LoginAsync(string Username, string Password, bool RememberMe);
        Task<LoginResponse> RegisterAsync(RegisterData Data);
        Task<LoginResponse> ResendOTPAsync(string Contact);
        Task<string> VerifyOtp(string userId, int code);
        Task<string> ForgetPasswordAsync(string Email);
        Task<bool> ResetPasswordAsync(ChangePasswordDTO ResetData);
        Task<bool> IsEmailConfirmedByName(string Name);
        Task test();
    }
}
