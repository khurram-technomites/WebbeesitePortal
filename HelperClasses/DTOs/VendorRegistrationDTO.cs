using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class VendorRegistrationDTO
    {
      

        public string NameAsPerTradeLicense { get; set; }

        public string FirstName { get; set; }
        public string Email { get; set; }
        public bool RequirePhoneNumberConfirmation { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
       
    }
}
