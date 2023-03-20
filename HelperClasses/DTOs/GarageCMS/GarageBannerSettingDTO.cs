using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageCMS
{
    public class GarageBannerSettingDTO
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string ImagePath { get; set; }

        public string Thumbnail { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public string BannerType { get; set; }
        public GarageDTO Garage { get; set; }
    }
}
