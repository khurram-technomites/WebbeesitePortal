using HelperClasses.DTOs.SparePartsDealer;

namespace WebApp.ViewModels
{
    public class SparePartContentManagementViewModel
    {
        public long Id { get; set; }
        public long SparePartDealerId { get; set; }
        public string AboutUsImage { get; set; }
        public string AboutUsDescription { get; set; }
        public string ShortIntro { get; set; }
        public string InnerBanner { get; set; }
        public string CEOMessage { get; set; }
        public string CEOName { get; set; }
        public string CEOImagePath { get; set; }
        public string Mission { get; set; }
        public string Vision { get; set; }
        public string Values { get; set; }
        public string FooterImage { get; set; }
        public string PrivacyPolicy { get; set; }
        public string TermsAndConditions { get; set; }
        public string PromoSection01 { get; set; }
        public int PromoSection01Count { get; set; }
        public string PromoSection02 { get; set; }
        public int PromoSection02Count { get; set; }
        public string PromoSection03 { get; set; }
        public int PromoSection03Count { get; set; }
        public SparePartsDealerViewModel SparePartDealer { get; set; }
    }
}
