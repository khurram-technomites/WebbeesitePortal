using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Blog
{
    public class BlogDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public string Slug { get; set; }
        public string TitleDescription { get; set; }
        public string TitleDescriptionAr { get; set; }
        public string MobileDescription { get; set; }
        public string MobileDescriptionAr { get; set; }
        public string BannerImage { get; set; }
        public string Video { get; set; }
        public string Author { get; set; }
        public string TwitterUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string Email { get; set; }
        public DateTime? PublishedOn { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime PostedDate { get; set; }
        public bool IsFeatured { get; set; }
        public int Position { get; set; }

        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public string FormattedDate { get; set; }
    }
}
