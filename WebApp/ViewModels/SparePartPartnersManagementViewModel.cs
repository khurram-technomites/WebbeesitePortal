using HelperClasses.DTOs.SparePartsDealer;

namespace WebApp.ViewModels
{
    public class SparePartPartnersManagementViewModel
    {
        public long Id { get; set; }
        public long SparePartDealerId { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public int Position { get; set; }
        public SparePartsDealerViewModel SparePartDealer { get; set; }
    }
}
