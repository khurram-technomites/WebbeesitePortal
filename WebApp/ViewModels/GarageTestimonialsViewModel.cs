using System;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Models;

namespace WebApp.ViewModels
{
    public class GarageTestimonialsViewModel
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string Testimonial { get; set; }
        public int Rating { get; set; }
        public string CustomerName { get; set; }
        public string CustomerImage { get; set; }
        public bool ShowOnWebsite { get; set; }
        public DateTime CreationDate { get; set; }
        public GarageViewModel Garage { get; set; }
    }
}
