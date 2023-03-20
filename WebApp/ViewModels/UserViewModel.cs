using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        //[Required]
       /* [DataType(DataType.Password)]*/
        public string Password { get; set; }

        public string PasswordHash { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        public string Status { get; set; }

        public string Role { get; set; }
        public int TotalRecords { get; set; }
        [Display(Name = "Creation Date")]
        public DateTime CreatedOn { get; set; }

        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
