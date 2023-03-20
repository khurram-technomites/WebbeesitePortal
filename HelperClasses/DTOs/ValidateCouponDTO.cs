using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class ValidateCouponDTO
    {
        public long RestaurantId { get; set; }
        public long? CustomerId { get; set; }
        public string CouponCode { get; set; }
        public long? CategoryId { get; set; }
    }
}
