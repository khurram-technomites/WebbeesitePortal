using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Supplier
{
    public class SupplierCouponDTO
    {
        public SupplierCouponDTO()
        {
            CouponCategories = new List<CouponCategoryDTO>();
            CouponRedemptions = new List<CouponRedemptionDTO>();
        }
        public long Id { get; set; }
        public Nullable<long> SupplierId { get; set; }
        public string Name { get; set; }
        public string CouponCode { get; set; }
        public Nullable<long> Frequency { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<DateTime> Expiry { get; set; }
        public string Type { get; set; }
        public Nullable<decimal> MaxAmount { get; set; }
        public string CoverImage { get; set; }
        public string Module { get; set; }
        public bool IsOpenToAll { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public int MaxUsage { get; set; }
        public DateTime CreationDate { get; set; }
        public string TermsAndConditions { get; set; }
        public bool IsUsed
        {
            get
            {
                return Frequency == CouponRedemptions.Count;
            }
            set { }
        }
        public int DaysLeft
        {
            get
            {
                return (Expiry.Value - DateTime.UtcNow).Days;
            }
            set { }
        }
        public SupplierDTO Supplier { get; set; }
        public List<CouponCategoryDTO> CouponCategories { get; set; }
        public List<CouponRedemptionDTO> CouponRedemptions { get; set; }
    }
}
