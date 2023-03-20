using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Blog
{
    public class BlogCardResponse
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string BannerImage { get; set; }
        public string CreatedDate { get; set; }
        public string Title { get; set; }
        public string TitleDescription { get; set; }
        public string Author { get; set; }
        public string AuthorImage { get; set; }
    }
}
