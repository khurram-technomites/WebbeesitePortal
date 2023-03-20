using HelperClasses.DTOs.SparePartsDealer;
using System;

namespace WebApp.ViewModels
{
    public class SparePartCustomerAppointmentViewModel
    {
        public long Id { get; set; }
        public long SparePartDealerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string CustomerComments { get; set; }
        public string Status { get; set; }
        public SparePartsDealerViewModel SparePartDealer { get; set; }
    }
}
