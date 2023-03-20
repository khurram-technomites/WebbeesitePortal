using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class RestaurantKitchenManagerViewModel
    {
        public long Id { get; set; }

        public string UserId { get; set; }
        public long RestaurantId { get; set; }
        public long RestaurantBranchId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
        public string Password { get; set; }
        public string Logo { get; set; }
        public DateTime CreationDate { get; set; }
        public RestaurantViewModel Restaurant { get; set; }
        public RestaurantBranchViewModel RestaurantBranch { get; set; }
        public string RestaurantBranchName { get; set; }
    }
}
