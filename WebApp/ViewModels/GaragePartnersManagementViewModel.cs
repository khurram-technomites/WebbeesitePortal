using HelperClasses.DTOs;

namespace WebApp.ViewModels
{
    public class GaragePartnersManagementViewModel
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public int Position { get; set; }
        public GarageViewModel Garage { get; set; }
    }
}
