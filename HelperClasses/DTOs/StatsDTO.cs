using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    class StatsDTO
    {
        public List<UserDTO> UserCount { get; set; }
        public List<CustomerDTO> CustomerCount { get; set; }
        public List<Restaurant.RestaurantDTO> RestaurantCount { get; set; }
        public List<GarageDTO> GarageCount { get; set; }
        public List<SparePartsDealer.SparePartsDealerDTO> SparePartCount { get; set; }
        public List<ServiceStaff.ServiceStaffDTO> ServiceStaffCount { get; set; }
        public List<DeliveryStaff.DeliveryStaffDTO> DeliveryStaffCount { get; set; }
    }
}
