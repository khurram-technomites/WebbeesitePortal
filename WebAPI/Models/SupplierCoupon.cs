using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class SupplierCoupon : GeneralSchema
    {
        public SupplierCoupon()
        {
            SupplierCouponRedemptions = new HashSet<SupplierCouponRedemption>();
            SupplierCouponCategories = new HashSet<SupplierCouponCategory>();
            RestaurantCoupons = new HashSet<RestaurantCoupon>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey(nameof(Supplier))]
        public Nullable<long> SupplierId { get; set; }
        [MaxLength(100, ErrorMessage = "Name length must be less than 100 characters")]
        public string Name { get; set; }
        [MaxLength(200, ErrorMessage = "Coupon Code length must be less than 200 characters")]
        public string CouponCode { get; set; }
        public Nullable<long> Frequency { get; set; }
        [MaxLength(200, ErrorMessage = "Coupon Code length must be less than 200 characters")]
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<DateTime> Expiry { get; set; }
        [MaxLength(50, ErrorMessage = "Type length must be less than 50 characters")]
        public string Type { get; set; }
        public Nullable<decimal> MaxAmount { get; set; }
        [MaxLength(4000, ErrorMessage = "Cover Image length must be less than 4000 characters")]
        public string CoverImage { get; set; }

        [MaxLength(50, ErrorMessage = "Module length must be less than 50 characters")]
        public string Module { get; set; }
        public bool IsOpenToAll { get; set; }
        public string Status { get; set; }
        public int MaxUsage { get; set; }
        public string Description { get; set; }
        public string TermsAndConditions { get; set; }
        public Supplier Supplier { get; set; }
        public ICollection<RestaurantCoupon> RestaurantCoupons { get; set; }
        public ICollection<SupplierCouponRedemption> SupplierCouponRedemptions { get; set; }
        public ICollection<SupplierCouponCategory> SupplierCouponCategories { get; set; }
    }
}
