using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageAndSPWebsiteResponses.WebsiteResponses
{
    public class WebsiteFooterContentResponseDTO
    {
        public string Logo { get; set; }
        public string AboutUS { get; set; }
        public string FooterImage { get; set; }
        public WebsiteHeaderSocial SocialMedia { get; set; }
        public WebsiteFooterContact ContactUs { get; set; }
        public List<WebsiteHeaderMenu> Explore { get; set; }
    }

    public class WebsiteFooterContact
    {
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Whatsapp { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
    }
    public class FooterAllowed
    {
        public bool IsServiceAllowed { get; set; }
        public bool IsBlogsAllowed { get; set; }
        public bool IsAppoinmentAllowed { get; set; }
        public bool IsTeamAllowed { get; set; }
        public bool IsFeedbackAllowed { get; set; }
        public bool IsCareersAllowed { get; set; }
    }
}
