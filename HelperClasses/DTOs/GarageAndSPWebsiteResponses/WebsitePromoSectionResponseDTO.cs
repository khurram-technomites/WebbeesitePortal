using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageAndSPWebsiteResponses.WebsiteResponses
{
    public class WebsitePromoSectionResponseDTO
    {
        public string FirstSection { get; set; }
        public int FirstSectionCount { get; set; }
        public string SecondSection { get; set; }
        public int SecondSectionCount { get; set; }
        public string ThirdSection { get; set; }
        public int ThirdSectionCount { get; set; }
        public List<WebsitePromoHeaderBanners> PromoBanners { get; set; }

    }
}
