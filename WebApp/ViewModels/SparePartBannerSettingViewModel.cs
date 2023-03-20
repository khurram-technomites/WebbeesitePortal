using HelperClasses.DTOs.SparePartsDealer;
using System;

namespace WebApp.ViewModels
{
    public class SparePartBannerSettingViewModel
    {
        public long Id { get; set; }

        public long SparePartDealerId { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public string BannerType { get; set; }
        public DateTime CreationDate { get; set; }
        public SparePartsDealerViewModel SparePartsDealer { get; set; }
    }
}
