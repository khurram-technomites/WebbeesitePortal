using HelperClasses.DTOs.Restaurant;
using System;

namespace WebApp.ViewModels
{
    public class RestaurantUserLogManagementViewModel
    {
        public long Id { get; set; }

        public DateTime? LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public string DeviceID { get; set; }
        public string Status { get; set; }


        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }

        public string UserId { get; set; }
        public string UserType { get; set; }
        public long? UserDetailId { get; set; }


        public long RestaurantId { get; set; }
        public long RestaurantBranchId { get; set; }
        public string BranchName { get; set; }
        //public long ServiceStaffId { get; set; }

        public RestaurantViewModel Restaurant { get; set; }
        //public RestaurantBranchViewModel RestaurantBranch { get; set; }
        //public AppUserDTO User { get; set; }
        //public ServiceStaffDTO ServiceStaff { get; set; }
    }
}
