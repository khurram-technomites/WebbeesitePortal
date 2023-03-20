using HelperClasses.DTOs;
using System;

namespace WebApp.ViewModels
{
    public class GarageCustomerFeedbackViewModel
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string CustomerEmail { get; set; }
        public int Rating { get; set; }
        public DateTime CreationDate { get; set; }
        public GarageViewModel Garage { get; set; }
    }
}
