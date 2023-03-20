using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class RestaurantContentManagementViewModel
    {
        public long Id { get; set; }
        public string PrivacyPolicy { get; set; }
        public string DeliveryPolicy { get; set; }
        public string ReturnPolicy { get; set; }
        public string TermsAndConditions { get; set; }
        public string AboutUs { get; set; }
        public long RestaurantId { get; set; }
        public string FooterImage { get; set; }
        public RestaurantViewModel restaurant { get; set; }
        public RestaurantBranchViewModel restaurantBranch { get; set; }
    }
}
