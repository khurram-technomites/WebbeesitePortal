using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class CustomerCouponDTO
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public long CouponId { get; set; }
        public CustomerDTO Customer { get; set; }
        public CouponDTO Coupon { get; set; }
    }
}
