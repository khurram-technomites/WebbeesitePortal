using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class CouponCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Coupon))]
        public long CouponId { get; set; }
        [ForeignKey(nameof(Category))]
        public long CategoryId { get; set; }
        public Coupon Coupon { get; set; }
        public Category Category { get; set; }
    }
}
