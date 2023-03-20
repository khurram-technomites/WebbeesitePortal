using System;

namespace WebApp.ViewModels
{
    public class SupplierCouponViewModel
    {
        public long Id { get; set; }
        public Nullable<long> SupplierId { get; set; }
        public string Name { get; set; }
        public string CouponCode { get; set; }
        public Nullable<long> Frequency { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<DateTime> Expiry { get; set; }
        public int RemainingDays { get; set; }
        public string Type { get; set; }
        public Nullable<decimal> MaxAmount { get; set; }
        public string CoverImage { get; set; }
        public string Module { get; set; }
        public bool IsOpenToAll { get; set; }
        public string Status { get; set; }
        public int MaxUsage { get; set; }
        public DateTime CreationDate { get; set; }
        public SupplierViewModel Supplier { get; set; }
    }
}
