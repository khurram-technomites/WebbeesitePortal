using HelperClasses.DTOs.SparePartsDealer;
using System;

namespace WebApp.ViewModels
{
    public class SparePartServiceManagementViewModel
    {
        public long Id { get; set; }
        public long SparePartDealerId { get; set; }
        public string BannerImagePath { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public SparePartsDealerViewModel SparePartDealer { get; set; }
    }
}
