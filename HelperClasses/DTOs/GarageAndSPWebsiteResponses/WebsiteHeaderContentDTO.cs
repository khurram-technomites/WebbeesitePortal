using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageAndSPWebsiteResponses.WebsiteResponses
{
    public class WebsiteHeaderContentDTO
    {
        public WebsiteHeaderContentDTO()
        {
            Banners = new List<WebsiteHeaderBanners>();
        }

        public long GarageId { get; set; }
        public string Logo { get; set; }
        public string ThemeColor { get; set; }
        public string Favicon { get; set; }
        public string Title {get;set;}
        public WebsiteHeaderContact ContactDetails { get; set; }
        public List<WebsiteHeaderBanners> Banners { get; set; }
        public List<WebsiteHeaderMenu> Menu { get; set; }
    }

    public class WebsiteHeaderMenu
    {
        public string Title { get; set; }
        public string Route { get; set; }
        public int Position { get; set; }
    }

    public class WebsiteHeaderContact
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public WebsiteHeaderSocial SocialMedia { get; set; }
    }

    public class WebsiteHeaderSocial
    {
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
    }

    public class WebsiteHeaderBanners
    {
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
    }

    public class WebsitePromoHeaderBanners
    {
        public string ImagePath { get; set; }

        public string Thumbnail { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
    }
}
