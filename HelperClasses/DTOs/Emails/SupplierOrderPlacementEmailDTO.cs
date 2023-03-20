using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Emails
{
    public class SupplierOrderPlacementEmailDTO
    {
        public string Email { get; set; }
        public RestaurantDTO Restaurant { get; set; }
        public SupplierOrderDTO Order { get; set; }
    }
}
