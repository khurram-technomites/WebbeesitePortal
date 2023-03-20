using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class CouponRedemptionDTO
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public long CouponId { get; set; }
        public string PhoneNumber { get; set; }
        public long OrderId { get; set; }
    }
}
