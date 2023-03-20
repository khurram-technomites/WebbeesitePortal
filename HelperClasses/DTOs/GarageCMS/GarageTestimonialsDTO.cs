using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageCMS
{
    public class GarageTestimonialsDTO
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string Testimonial { get; set; }
        public int Rating { get; set; }
        public string CustomerName { get; set; }
        public string CustomerImage { get; set; }
        public bool ShowOnWebsite { get; set; }
        public DateTime CreationDate { get; set; }
        public GarageDTO Garage { get; set; }
    }
}
