using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Display(Name = "Email address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
