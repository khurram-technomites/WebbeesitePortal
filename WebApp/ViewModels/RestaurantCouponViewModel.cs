using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class RestaurantCouponViewModel
    {
        public long Id { get; set; }
        public long RestaurantId { get; set; }
        public long SupplierCouponId { get; set; }
        public RestaurantViewModel Restaurant { get; set; }
        public SupplierCouponViewModel SupplierCoupon { get; set; }
    }
}
