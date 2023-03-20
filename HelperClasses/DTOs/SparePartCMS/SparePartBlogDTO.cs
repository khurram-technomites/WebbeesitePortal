using HelperClasses.DTOs.SparePartsDealer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.SparePartCMS
{
    public class SparePartBlogDTO
    {
        public long Id { get; set; }
        public long SparePartDealerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int EstimatedReadingMinutes { get; set; }
        public string Status { get; set; }
        public string Slug { get; set; }
        public DateTime CreationDate { get; set; }

        public SparePartsDealerDTO SparePartDealer { get; set; }
    }
}
