using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Blogs : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(225,ErrorMessage = "Title must be less than 225 characters")]
        public string Title { get; set; }

        [MaxLength(225, ErrorMessage = "Title must be less than 225 characters")]
        public string TitleAr { get; set; }
        [MaxLength(225, ErrorMessage = "Slug must be less than 225 characters")]
        public string Slug { get; set; }
        public string TitleDescription { get; set; }
        [MaxLength(8000, ErrorMessage = "TitleDescriptionAr must be less than 8000 characters")]
        public string TitleDescriptionAr { get; set; }
        [MaxLength(4000, ErrorMessage = "MobileDescription must be less than 4000 characters")]
        public string MobileDescription { get; set; }
        [MaxLength(4000, ErrorMessage = "MobileDescriptionAr must be less than 4000 characters")]
        public string MobileDescriptionAr { get; set; }
        [MaxLength(2000, ErrorMessage = "BannerImage must be less than 2000 characters")]
        public string BannerImage { get; set; }
        [MaxLength(2000, ErrorMessage = "Video must be less than 2000 characters")]
        public string Video { get; set; }
        [MaxLength(225, ErrorMessage = "Author must be less than 225 characters")]
        public string Author { get; set; }
        [MaxLength(2000, ErrorMessage = "TwitterUrl must be less than 2000 characters")]
        public string TwitterUrl { get; set; }
        [MaxLength(2000, ErrorMessage = "FacebookUrl must be less than 2000 characters")]
        public string FacebookUrl { get; set; }
        [MaxLength(2000, ErrorMessage = "LinkedInUrl must be less than 2000 characters")]
        public string LinkedInUrl { get; set; }
        [MaxLength(2000, ErrorMessage = "InstagramUrl must be less than 2000 characters")]
        public string InstagramUrl { get; set; }
        [MaxLength(2000, ErrorMessage = "Email must be less than 20 characters")]
        public string Email { get; set; }
        public DateTime? PublishedOn { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime PostedDate { get; set; }
        public bool IsFeatured { get; set; }
        public int Position { get; set; }
        public string Status { get; set; }
    }
    
}
