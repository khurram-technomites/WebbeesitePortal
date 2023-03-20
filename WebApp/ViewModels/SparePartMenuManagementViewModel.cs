using HelperClasses.DTOs.SparePartCMS;
using HelperClasses.DTOs.SparePartsDealer;

namespace WebApp.ViewModels
{
    public class SparePartMenuManagementViewModel
    {
        public long Id { get; set; }
        public long SparePartDealerId { get; set; }
        public long SparePartMenuId { get; set; }
        public int Position { get; set; }
        public string Status { get; set; }
        public SparePartsDealerViewModel SparePartsDealer { get; set; }
        public SparePartMenuViewModel SparePartMenu { get; set; }
    }
}
