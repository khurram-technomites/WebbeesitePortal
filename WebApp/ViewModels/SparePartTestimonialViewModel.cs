using HelperClasses.DTOs.SparePartsDealer;
using System;

namespace WebApp.ViewModels
{
    public class SparePartTestimonialViewModel
    {
        public long Id { get; set; }
        public long SparePartDealerId { get; set; }
        public string Testimonial { get; set; }
        public int Rating { get; set; }
        public string CustomerName { get; set; }
        public string CustomerImage { get; set; }
        public bool ShowOnWebsite { get; set; }
        public DateTime CreationDate { get; set; }
        public SparePartsDealerViewModel SparePartDealer { get; set; }
    }
}
