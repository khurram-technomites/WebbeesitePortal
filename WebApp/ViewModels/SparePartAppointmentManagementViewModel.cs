using HelperClasses.DTOs.SparePartsDealer;
using System;

namespace WebApp.ViewModels
{
    public class SparePartAppointmentManagementViewModel
    {
        public long Id { get; set; }
        public long SparePartDealerId { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Section01 { get; set; }
        public string Section02 { get; set; }
        public string Section03 { get; set; }
        public string Section04 { get; set; }
        public DateTime CreationDate { get; set; }
        public SparePartsDealerViewModel SparePartDealer { get; set; }
    }
}
