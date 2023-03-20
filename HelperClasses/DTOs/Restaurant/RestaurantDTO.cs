using HelperClasses.DTOs.RestaurantCashierStaff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantDTO
    {
        public RestaurantDTO()
        {
            RestaurantBranches = new List<RestaurantBranchDTO>();
            restaurantRatings = new List<RestaurantRatingDTO>();
            restaurantDeliveryStaffs = new List<RestaurantDeliveryStaffDTO>();
            categories = new List<CategoryDTO>();
            RestaurantImages = new List<RestaurantImagesDTO>();
            RestaurantBannerSettings = new List<RestaurantBannerSettingDTO>();
            RestaurantDocuments = new List<RestaurantDocumentDTO>();
            RestaurantCashierStaffs = new List<RestaurantCashierStaffDTO>();
        }
        public long Id { get; set; }
        public string NameAsPerTradeLicense { get; set; }
        public string NameArAsPerTradeLicense { get; set; }
        public string Logo { get; set; }
        public string SecondaryLogo { get; set; }
        public string ThumbnailImage { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string WhatsappNumber { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Linkedin { get; set; }
        public string Status { get; set; }
        public decimal TaxPercent { get; set; }
        public string UserId { get; set; }
        public string Slug { get; set; }
        public string ReferenceCode { get; set; }
        public string SupplierCode { get; set; }
        public string Description { get; set; }
        public string Bank { get; set; }
        public string Favicon { get; set; }
        public string BankAccountHolderName { get; set; }
        public string BankAccountNumber { get; set; }
        public string IBAN { get; set; }
        public string ThemeColor { get; set; }
        public string Origin { get; set; }
        public string DescriptionImage { get; set; }
        public DateTime CreationDate { get; set; }
        public string VATRegistrationNumber { get; set; }
        public bool IsCashierAllowed { get; set; }
        public bool IsPartnerAllowed { get; set; }
        public bool IsKitchenManagerAllowed { get; set; }
        public bool IsWaiterAllowed { get; set; }
        public string OrderPaymentType { get; set; }

        public AppUserDTO User { get; set; }
        public string DateDifference
        {
            get
            {
                DateTime current = DateTime.UtcNow;
                double difference = (current - CreationDate).TotalDays;

                if (difference < 30)
                    return $"Since {Math.Floor(difference)} days";

                if (difference > 30 && difference < 365)
                    return $"Since {Math.Floor(difference / 30)} months";

                else
                    return $"Since {Math.Floor(difference / 365)} years";

            }
            set { }
        }
        public List<RestaurantBranchDTO> RestaurantBranches { get; set; }
        public List<RestaurantRatingDTO> restaurantRatings { get; set; }
        public List<RestaurantDeliveryStaffDTO> restaurantDeliveryStaffs { get; set; }
        public List<RestaurantImagesDTO> RestaurantImages { get; set; }
        public List<CategoryDTO> categories { get; set; }
        public List<RestaurantBannerSettingDTO> RestaurantBannerSettings { get; set; }
        public List<RestaurantDocumentDTO> RestaurantDocuments { get; set; }
        public List<RestaurantCashierStaffDTO> RestaurantCashierStaffs { get; set; }

    }
}
