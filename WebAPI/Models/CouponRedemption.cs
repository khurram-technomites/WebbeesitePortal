using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class CouponRedemption : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        [ForeignKey(nameof(Coupon))]
        public long CouponId { get; set; }
        public string PhoneNumber { get; set; }
        public long OrderId { get; set; }
        public Coupon Coupon { get; set; }
        public AppUser User { get; set; }
    }

}
