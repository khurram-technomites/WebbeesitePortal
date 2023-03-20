using HelperClasses.DTOs.SparePartsDealer;

namespace WebApp.ViewModels
{
    public class SparePartSubscriberViewModel
    {
        public long Id { get; set; }
        public long SparePartDealerId { get; set; }
        public string Email { get; set; }
        public SparePartsDealerViewModel SparePartDealer { get; set; }
    }
}
