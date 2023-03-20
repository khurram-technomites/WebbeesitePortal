using HelperClasses.DTOs.SparePartsDealer;
using System;

namespace WebApp.ViewModels
{
    public class SparePartCustomerFeedbackViewModel
    {
        public long Id { get; set; }
        public long SparePartDealerId { get; set; }
        public string CustomerEmail { get; set; }
        public int Rating { get; set; }
        public DateTime CreationDate { get; set; }

        public SparePartsDealerViewModel SparePartDealer { get; set; }
    }
}
