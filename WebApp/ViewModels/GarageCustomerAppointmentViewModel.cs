using HelperClasses.DTOs;
using System;

namespace WebApp.ViewModels
{
    public class GarageCustomerAppointmentViewModel
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string CustomerComments { get; set; }
        public string Status { get; set; }
        public GarageViewModel Garage { get; set; }
    }
}
