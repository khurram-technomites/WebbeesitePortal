using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class GarageBlog : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Garage))]
        public long GarageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int EstimatedReadingMinutes { get; set; }
        public string Status { get; set; }
        public string Slug { get; set; }
        [ForeignKey(nameof(BlogCategory))]
        public long? BlogCategoryId { get; set; }
        public Garage Garage { get; set; }
        public BlogCategory BlogCategory { get; set; }
    }
}
