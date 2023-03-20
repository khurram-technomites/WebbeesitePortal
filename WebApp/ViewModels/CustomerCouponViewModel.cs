namespace WebApp.ViewModels
{
    public class CustomerCouponViewModel
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public long CouponId { get; set; }
        public CustomerViewModel Customer { get; set; }
        public CouponViewModel Coupon { get; set; }
    }
}
