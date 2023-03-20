using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Models;

namespace WebApp.ViewModels
{
    public class GarageAppointmentManagementViewModel
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Section01 { get; set; }
        public string Section02 { get; set; }
        public string Section03 { get; set; }
        public string Section04 { get; set; }
        public GarageViewModel Garage { get; set; }
    }
}
