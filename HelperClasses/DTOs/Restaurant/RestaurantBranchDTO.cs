using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantBranchDTO
    {
        public RestaurantBranchDTO()
        {
            BranchSchedules = new List<RestaurantBranchScheduleDTO>();
        }
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
        public decimal MinOrderPrice { get; set; } = 60M;
        public TimeSpan? ClosingTimeSpan { get; set; }
        public DateTime CreationDate { get; set; }
        public RestaurantDTO Restaurant { get; set; }
        public CityDTO City { get; set; }
        public CountryDTO Country { get; set; }
        public List<RestaurantBranchScheduleDTO> BranchSchedules { get; set; }
    }
}
