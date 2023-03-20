using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class GarageTestimonials : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Garage))]
        public long GarageId { get; set; }
        public string Testimonial { get; set; }
        public int Rating { get; set; }
        public string CustomerName { get; set; }
        public string CustomerImage { get; set; }
        public bool ShowOnWebsite { get; set; }
        public Garage Garage { get; set; }
    }
}
