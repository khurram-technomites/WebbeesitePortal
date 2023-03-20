using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class GarageDashboardViewModel
    {
        public long TotalProjects { get; set; } = 0;
        public long TotalAwards { get; set; } = 0;
        public long TotalPartners { get; set; } = 0;
        public long TotalMenus { get; set; } = 0;
        public long TotalTestimonials { get; set; } = 0;
        public double AverageRating { get; set; } = 0;

        public GarageViewModel garage { get; set; } = new GarageViewModel();
        public List<GarageCustomerAppointmentViewModel> CustomerAppointment { get; set; } = new List<GarageCustomerAppointmentViewModel>();

    }
}
