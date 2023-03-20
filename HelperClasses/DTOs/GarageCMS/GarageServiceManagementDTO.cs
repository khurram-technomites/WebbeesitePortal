using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageCMS
{
    public class GarageServiceManagementDTO
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string BannerImagePath { get; set; }
        public string Thumbnail { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Slug { get; set; }
        public DateTime CreationDate { get; set; }
        public GarageDTO Garage { get; set; }
    }
}
