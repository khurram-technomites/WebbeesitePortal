using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SupplierCouponRedemption
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        [ForeignKey(nameof(SupplierCoupon))]
        public long SupplierCouponId { get; set; }
        public string PhoneNumber { get; set; }
        public long SupplierOrderId { get; set; }
        public SupplierCoupon SupplierCoupon { get; set; }
        public SupplierOrder SupplierOrder { get; set; }
        public AppUser User { get; set; }
    }
}
