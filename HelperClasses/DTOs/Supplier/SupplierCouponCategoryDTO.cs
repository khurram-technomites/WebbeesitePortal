using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class SupplierCouponCategoryDTO
    {
        public long Id { get; set; }
        public long SupplierCouponId { get; set; }
        public long SupplierItemCategoryId { get; set; }
        public SupplierCouponDTO SupplierCoupon { get; set; }
        public SupplierItemCategoryDTO SupplierItemCategory { get; set; }
    }
}
