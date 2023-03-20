using HelperClasses.DTOs.RestaurantCashierStaff;
using System;

namespace WebApp.ViewModels.Restaurant
{
    public class RestaurantCashierStaffViewModel
    {
          public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
		public bool RequirePhoneNumberConfirmation { get; set; }
        public string Logo { get; set; }
        public string UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public long RestaurantId { get; set; }
        public long RestaurantBranchId { get; set; }
        public string RestaurantBranchName { get; set; }
        public bool IsPrinterAllowed { get; set; } // toggle added in mobile app

        public RestaurantBranchViewModel RestaurantBranch { get; set; }
    }
}
