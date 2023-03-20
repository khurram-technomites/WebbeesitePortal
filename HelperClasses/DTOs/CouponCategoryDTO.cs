using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class CouponCategoryDTO
    {
        public long Id { get; set; }
        public long CouponId { get; set; }
        public long CategoryId { get; set; }
        public CouponDTO Coupon { get; set; }
        public CategoryDTO Category { get; set; }
    }
}
