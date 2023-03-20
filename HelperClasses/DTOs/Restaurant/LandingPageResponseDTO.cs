using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class LandingPageResponseDTO
    {
        public LandingPageResponseDTO()
        {
            RestaurantRatings = new List<RestaurantRatingDTO>();
        }
        public long RestaurantId { get; set; }
        public long RestaurantBranchId { get; set; }
        public string BranchDeliveryType { get; set; }
        public decimal BranchDeliveryCharges { get; set; }
        public string RestaurantName { get; set; }
        public string BranchName { get; set; }
        public string BannerImage { get; set; }
        public string BannerUrl { get; set; }
        public string MenuBannerImage { get; set; }
        public string Logo { get; set; }
        public string SecondaryLogo { get; set; }
        public string AboutUsImage { get; set; }
        public string AboutUs { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Linkedin { get; set; }
        public double AvgRating { get; set; }
        public double RatingCount { get; set; }
        public string ThemeColor { get; set; }
        public decimal VAT { get; set; }
        public string Footer { get; set; }
        public bool IsClose { get; set; }
        public TimeSpan? ClosingTimeSpan { get; set; }
        public decimal DeliveryCharges { get; set; }
        public decimal TaxPercent { get; set; }
        public string RestaurantDeliveryType { get; set; }
        public bool Isfavourite { get; set; }
        public string Origin { get; set; }
        public string Favicon { get; set; }
        public string OpeningTime { get; set; }
        public string ClosingTime { get; set; }
        public string Distance { get; set; }
        public string OrderPaymentType { get; set; }

        public List<RestaurantRatingDTO> RestaurantRatings { get; set; }
        public List<LandingPromotionBannerDTO> PromotionBanners { get; set; }
        public IEnumerable<TrendingItemsDTO> TrendingItems { get; set; }
       
    }

    public class LandingPromotionBannerDTO
    {
        public string ImagePath { get; set; }
        public string Url { get; set; }
    }

    public class TrendingItemsDTO
    {
        public string Name { get; set; }
    }
}
