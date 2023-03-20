using HelperClasses.DTOs.Restaurant;
using System;

namespace WebApp.ViewModels
{
    public class RestaurantTaxSettingViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal TAXPercent { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }


        public long RestaurantId { get; set; }
        public long RestaurantBranchId { get; set; }

        public RestaurantViewModel Restaurant { get; set; }
        public RestaurantBranchViewModel RestaurantBranch { get; set; }
    }
}
