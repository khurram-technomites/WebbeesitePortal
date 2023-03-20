using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageAndSPWebsiteResponses.WebsiteResponses
{
    public class WebsiteBlogResponseDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int EstimatedReadingMinutes { get; set; }
        public string CreationDate { get; set; }
        public string Slug { get; set; }
    }
}
