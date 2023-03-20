using Microsoft.AspNetCore.Identity;
using System;

namespace WebAPI.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int AuthCode { get; set; }
        public DateTime AuthCodeExpiryTime { get; set; }
        public string LoginFor { get; set; }
        public string Logo { get; set; }
    }
}
