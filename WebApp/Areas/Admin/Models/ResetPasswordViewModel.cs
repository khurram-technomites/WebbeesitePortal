using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.Admin.Models
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "The New Password is required")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "The Confirm Password is required")]
        [Compare("NewPassword", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }

        public string UserId { get; set; }
    }
}
