using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels.Garage
{
    public class GarageBannersViewModel
    {
        public List<GarageBannerSettingViewModel> Banners { get; set; }
        public GarageBannerSettingViewModel PromoBanners { get; set; }
    }
}
