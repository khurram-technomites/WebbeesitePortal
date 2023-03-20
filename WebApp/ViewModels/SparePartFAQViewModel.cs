using HelperClasses.DTOs.SparePartsDealer;

namespace WebApp.ViewModels
{
    public class SparePartFAQViewModel
    {
        public long Id { get; set; }
        public long SparePartId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int Position { get; set; }
        public SparePartsDealerViewModel SparePart { get; set; }
    }
}
