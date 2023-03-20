using HelperClasses.DTOs.SparePartsDealer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.SparePartCMS
{
    public class SparePartBannerSettingDTO
    {
        public long Id { get; set; }

        public long SparePartDealerId { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public string BannerType { get; set; }
        public DateTime CreationDate { get; set; }
        public SparePartsDealerDTO SparePartsDealer { get; set; }
    }
}
