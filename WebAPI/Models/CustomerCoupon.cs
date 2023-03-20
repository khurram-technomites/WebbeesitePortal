using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class CustomerCoupon
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Customer))]
        public long CustomerId { get; set; }

        [ForeignKey(nameof(Coupon))]
        public long CouponId { get; set; }
        public Customer Customer { get; set; }
        public Coupon Coupon { get; set; }
    }
}
