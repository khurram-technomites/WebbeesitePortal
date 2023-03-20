using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantCouponDTO
    {
        public long Id { get; set; }
        public long RestaurantId { get; set; }
        public long SupplierCouponId { get; set; }
        public RestaurantDTO Restaurant { get; set; }
        public SupplierCouponDTO SupplierCoupon { get; set; }
    }
}
