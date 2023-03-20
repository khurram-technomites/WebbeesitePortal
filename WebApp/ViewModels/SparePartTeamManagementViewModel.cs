using HelperClasses.DTOs.SparePartsDealer;
using System;

namespace WebApp.ViewModels
{
    public class SparePartTeamManagementViewModel
    {
        public long Id { get; set; }
        public long SparePartDealerId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string ImagePath { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public DateTime CreationDate { get; set; }
        public SparePartsDealerViewModel SparePartDealer { get; set; }
    }
}
