using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class RestaurantBannerSettingDTO
    {
        public long Id { get; set; }
        public long RestaurantId { get; set; }
        public string Lang { get; set; }
        public string ImagePath { get; set; }
        public string Url { get; set; }
        public string BannerType { get; set; }
    }
}
