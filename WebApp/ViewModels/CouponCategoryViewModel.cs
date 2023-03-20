namespace WebApp.ViewModels
{
    public class CouponCategoryViewModel
    {
        public long Id { get; set; }
        public long CouponId { get; set; }
        public long CategoryId { get; set; }
        public CouponViewModel Coupon { get; set; }
        public CategoryViewModel Category { get; set; }
    }
}
