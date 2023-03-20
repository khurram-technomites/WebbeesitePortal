using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Supplier
{
    public class SupplierCouponRedemptionDTO
    {
  
        public long Id { get; set; }
        public string UserId { get; set; }
        public long SupplierCouponId { get; set; }
        public string PhoneNumber { get; set; }
        public long SupplierOrderId { get; set; }
        public SupplierCouponDTO SupplierCoupon { get; set; }
        public SupplierOrderDTO SupplierOrder { get; set; }
        public UserDTO User { get; set; }
    }
}
