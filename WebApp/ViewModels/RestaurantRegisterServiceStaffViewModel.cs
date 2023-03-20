using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class RestaurantRegisterServiceStaffViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public bool RequirePhoneNumberConfirmation { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Logo { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public RestaurantServiceStaffViewModel ServiceStaff { get; set; }
    }
}
