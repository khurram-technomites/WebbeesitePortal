using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class RestaurantContentManagementDTO
    {
        public long Id { get; set; }
        public string PrivacyPolicy { get; set; }
        public string DeliveryPolicy { get; set; }
        public string ReturnPolicy { get; set; }
        public string TermsAndConditions { get; set; }
        public string AboutUs { get; set; }
        public long RestaurantId { get; set; }
        public string FooterImage { get; set; }

        public RestaurantDTO restaurant { get; set; }
        public RestaurantBranchDTO restaurantBrnach { get; set; }
    }
}
