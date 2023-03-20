using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class RestaurantBranch : GeneralSchema
    {
        public RestaurantBranch()
        {
            BranchSchedules = new HashSet<RestaurantBranchSchedule>();
            RestaurantServiceStaffs = new HashSet<RestaurantServiceStaff>();
            RestaurantDeliveryStaffs = new HashSet<RestaurantDeliveryStaff>();
            CustomerFavouriteBranches = new HashSet<CustomerFavouriteBranches>();
            RestaurantCashierStaffs = new HashSet<RestaurantCashierStaff>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }
        [MaxLength(200, ErrorMessage = "NameAsPerTradeLicense length must be less than 200 characters")]
        public string NameAsPerTradeLicense { get; set; }
        [MaxLength(200, ErrorMessage = "NameArAsPerTradeLicense length must be less than 200 characters")]
        public string NameArAsPerTradeLicense { get; set; }
        [MaxLength(100, ErrorMessage = "Email length must be less than 100 characters")]
        public string Email { get; set; }
        [MaxLength(20, ErrorMessage = "OrderingPhoneNumber length must be less than 20 characters")]
        public string OrderingPhoneNumber { get; set; }
        [MaxLength(250, ErrorMessage = "Address length must be less than 250 characters")]
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        [ForeignKey(nameof(City))]
        public long? CityId { get; set; }

        [ForeignKey(nameof(Country))]
        public long? CountryId { get; set; }
        public bool IsClose { get; set; }
        public TimeSpan? ClosingTimeSpan { get; set; }
        public decimal ServiceDistance { get; set; }
        public string DeliveryType { get; set; }
        public decimal DeliveryCharges { get; set; }
        public int DeliveryMinutes { get; set; }
        public string Slug { get; set; }
        public string Status { get; set; }
        public bool IsMainBranch { get; set; }
        public string UniqueKey { get; set; }
        public decimal MinOrderPrice { get; set; }

        public Restaurant Restaurant { get; set; }
        public City City { get; set; }
        public Country Country { get; set; }
        public ICollection<RestaurantBranchSchedule> BranchSchedules { get; set; }
        public ICollection<RestaurantServiceStaff> RestaurantServiceStaffs { get; set; }
        public ICollection<RestaurantDeliveryStaff> RestaurantDeliveryStaffs { get; set; }
        public ICollection<CustomerFavouriteBranches> CustomerFavouriteBranches { get; set; }
        public ICollection<RestaurantCashierStaff> RestaurantCashierStaffs { get; set; }
    }
}
