using System.Collections.Generic;
using WebApp.ViewModels;

namespace WebApp.Models
{
    public class BannerViewModel
    {
        public RestaurantBannerSettingViewModel Banner { get; set; }

        public List<RestaurantBannerSettingViewModel> PromotionBanner { get; set; }
    }
}
