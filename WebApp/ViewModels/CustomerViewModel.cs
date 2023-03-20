using HelperClasses.DTOs;
using System;
using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class CustomerViewModel
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }

        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public AppUserDTO User { get; set; }
        public List<RestaurantCustomerViewModel> RestaurantCustomer { get; set; }
    }
}
