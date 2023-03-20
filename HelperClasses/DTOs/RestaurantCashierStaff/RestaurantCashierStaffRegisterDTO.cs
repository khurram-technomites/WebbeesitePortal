using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.RestaurantCashierStaff
{
    public class RestaurantCashierStaffRegisterDTO
	{

        public RestaurantCashierStaffRegisterDTO()
        {
            RestaurantCashierStaff = new RestaurantCashierStaffDTO();
        }

        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public bool RequirePhoneNumberConfirmation { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreationDate { get; set; }

        public RestaurantCashierStaffDTO RestaurantCashierStaff { get; set; }
    }
}
