using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.RestaurantDeliveryStaff
{
    public class RestaurantDeliveryStaffDTO
	{
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Logo { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserId { get; set; }
        public AppUserDTO User { get; set; }
    }
}
