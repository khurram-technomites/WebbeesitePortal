using HelperClasses.DTOs.SparePartCMS;
using HelperClasses.DTOs.SparePartsDealer;
using System;
using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class SparePartExpertiseManagementViewModel
    {
        public long Id { get; set; }
        public long SparePartDealerId { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string ImagePath { get; set; }
        public SparePartsDealerViewModel SparePartDealer { get; set; }
        public List<SparePartExpertiseViewModel> SparePartExpertise { get; set; }
    }
}
