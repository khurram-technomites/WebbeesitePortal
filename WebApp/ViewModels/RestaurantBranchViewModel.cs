using System;
using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class RestaurantBranchViewModel
    {
        public long Id { get; set; }
        public long RestaurantId { get; set; }
        public string NameAsPerTradeLicense { get; set; }
        public string NameArAsPerTradeLicense { get; set; }
        public string Email { get; set; }
        public string OrderingPhoneNumber { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public long? CityId { get; set; }
        public long? CountryId { get; set; }
        public bool IsClose { get; set; }
        public decimal ServiceDistance { get; set; }
        public string DeliveryType { get; set; }
        public decimal DeliveryCharges { get; set; }
        public decimal AvgRating { get; set; }
        public int RatingCount { get; set; }
        public int DeliveryMinutes { get; set; }
        public string Slug { get; set; }
        public string Status { get; set; }
        public bool IsMainBranch { get; set; }
        public string UniqueKey { get; set; }
        public string Distance { get; set; }
        public DateTime CreationDate { get; set; }
        public TimeSpan? ClosingTimeSpan { get; set; }
        public decimal MinOrderPrice { get; set; }

        public RestaurantViewModel Restaurant { get; set; }
        public CityViewModel City { get; set; }
        public CountryViewModel Country { get; set; }
        public List<RestaurantBranchScheduleViewModel> BranchSchedules { get; set; }
    }
}
