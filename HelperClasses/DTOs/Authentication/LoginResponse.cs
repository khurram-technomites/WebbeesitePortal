using HelperClasses.DTOs.DeliveryStaff;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.RestaurantCashierStaff;
using HelperClasses.DTOs.RestaurantKitchenManager;
using HelperClasses.DTOs.RestaurantServiceStaff;
using HelperClasses.DTOs.ServiceStaff;
using HelperClasses.DTOs.SparePartsDealer;
using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Authentication
{
    public class LoginResponse
    {
        public UserAuthData AuthData { get; set; }
        public StoreDTO Store { get; set; }
        public GarageDTO Garage { get; set; }
        public VendorDTO Vendor { get; set; }
        public SparePartsDealerDTO SparePartsDealer { get; set; }
        public RestaurantDTO Restaurant { get; set; }
        public ServiceStaffDTO ServiceStaff { get; set; }
        public DeliveryStaffDTO DeliveryStaff { get; set; }
        public RestaurantServiceStaffDTO RestaurantServiceStaff { get; set; }
        public RestaurantDeliveryStaffDTO RestaurantDeliveryStaff { get; set; }
        public RestaurantCashierStaffDTO RestaurantCashierStaff { get; set; }
        public RestaurantKitchenManagerDTO RestaurantKitchenManager { get; set; }
        public CustomerDTO Customer { get; set; }
        public SupplierDTO Supplier { get; set; }

        public LoginResponse()
        {
            AuthData = new();
            Store = new();
            Garage = new();
            SparePartsDealer = new();
            Restaurant = new();
            ServiceStaff = new();
            DeliveryStaff = new();
            RestaurantServiceStaff = new();
            Customer = new();
            RestaurantDeliveryStaff = new();
            RestaurantCashierStaff = new();
            RestaurantKitchenManager = new();
        }

        public LoginResponse(bool _success, string _httpStatusCode, string _message, UserAuthData data)
        {
            AuthData = data;
        }
    }
}
