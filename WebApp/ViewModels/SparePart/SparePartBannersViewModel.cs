using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels.SparePart
{
    public class SparePartBannersViewModel
    {
        public List<SparePartBannerSettingViewModel> Banners { get; set; }
        public List<SparePartBannerSettingViewModel> PromoBanners { get; set; }
    }
}
