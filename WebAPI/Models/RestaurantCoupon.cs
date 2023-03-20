using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class RestaurantCoupon
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }

        [ForeignKey(nameof(SupplierCoupon))]
        public long SupplierCouponId { get; set; }
        public Restaurant Restaurant { get; set; }
        public SupplierCoupon SupplierCoupon { get; set; }
    }
}
