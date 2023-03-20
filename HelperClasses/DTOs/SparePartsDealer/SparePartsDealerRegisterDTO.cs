using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.SparePartsDealer
{
    public class SparePartsDealerRegisterDTO
    {
        public SparePartsDealerRegisterDTO()
        {
            SparePartsDealer = new SparePartsDealerDTO();
        }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public bool RequirePhoneNumberConfirmation { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

        public SparePartsDealerDTO SparePartsDealer { get; set; }
    }
}
