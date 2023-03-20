using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class UserDTO
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }

        public string Password{ get; set; }
        public bool IsActive { get; set; }
        
        public string Status { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
