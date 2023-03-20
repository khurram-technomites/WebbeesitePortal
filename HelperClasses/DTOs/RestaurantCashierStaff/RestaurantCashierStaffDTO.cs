using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.RestaurantCashierStaff
{
    public class RestaurantCashierStaffDTO
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
        public decimal TaxPercentage { get; set; }
        public decimal DeliveryCharges { get; set; }
        public string UserId { get; set; }
        public DateTime CreationDate { get; set; }

        public long RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public long RestaurantBranchId { get; set; }
        public string RestaurantBranchName { get; set; }
        public string RestaurantBranchContact { get; set; }
        public long RestaurantBalanceSheetId { get; set; }

        public bool IsClose { get; set; } // branch close
        public bool IsShiftClose { get; set; } = true;
        public bool IsPrinterAllowed { get; set; } // toggle added in mobile app

        public string RestaurantWebsite { get; set; }
        public string RestaurantVATRegistrationNumber { get; set; }

        /*Relation*/
        public AppUserDTO User { get; set; }
        public RestaurantBalanceSheetDTO RestaurantBalanceSheet { get; set; }
		public RestaurantBranchDTO RestaurantBranch { get; set; }
		public RestaurantDTO Restaurant { get; set; }

	}
}
