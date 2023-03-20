using HelperClasses.DTOs.SparePartsDealer;
using System;

namespace WebApp.ViewModels
{
    public class SparePartCareerViewModel
    {
        public long Id { get; set; }
        public long SparePartDealerId { get; set; }
        public string FulName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string Education { get; set; }
        public string Position { get; set; }
        public string Experience { get; set; }
        public string CVPath { get; set; }
        public DateTime CreationDate { get; set; }
        public SparePartsDealerViewModel SparePartDealer { get; set; }
    }
}
