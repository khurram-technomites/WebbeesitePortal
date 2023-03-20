using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SupplierCouponCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(SupplierCoupon))]
        public long SupplierCouponId { get; set; }
        [ForeignKey(nameof(SupplierItemCategory))]
        public long SupplierItemCategoryId { get; set; }
        public SupplierCoupon SupplierCoupon { get; set; }
        public SupplierItemCategory SupplierItemCategory { get; set; }
    }
}
