using HelperClasses.DTOs.Authentication;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Interfaces.TypedClients
{
    public interface IVendorAccountClient
    {
        Task<LoginResponse> LoginAsync(string Phonenumber, string Password, bool RememberMe);
        Task<string> ForgetPasswordAsync(string PhoneNumber, string For);
        Task<string> VerifyOtp(string userId, int code);
        Task<LoginResponse> ResendOTPAsync(string Contact ,string OTPFor);
        Task<bool> ResetPasswordAsync(ChangePasswordDTO ResetData);
        Task<bool> ChangePasswordAsync(ChangePasswordViewModel changePasswordViewModel);


    }
}
