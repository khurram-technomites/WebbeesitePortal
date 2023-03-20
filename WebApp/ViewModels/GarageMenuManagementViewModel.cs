using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs;

namespace WebApp.ViewModels
{
    public class GarageMenuManagementViewModel
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public long GarageMenuId { get; set; }
        public int Position { get; set; }
        public string Status { get; set; }
        public GarageViewModel Garage { get; set; }
        public GarageMenuViewModel GarageMenu { get; set; }
    }
}
