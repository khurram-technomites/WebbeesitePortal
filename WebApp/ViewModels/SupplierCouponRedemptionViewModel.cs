using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class SupplierCouponRedemptionViewModel
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public long SupplierCouponId { get; set; }
        public string PhoneNumber { get; set; }
        public long SupplierOrderId { get; set; }
        public SupplierCouponViewModel SupplierCoupon { get; set; }
        public SupplierOrderViewModel SupplierOrder { get; set; }
        public UserViewModel User { get; set; }
    }
}
