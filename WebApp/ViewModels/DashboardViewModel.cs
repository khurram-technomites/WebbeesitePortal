using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class DashboardViewModel
    {
        public long UserCount { get; set; } = 0;
        public long CustomerCount { get; set; } = 0;
        public long RestaurantCount { get; set; } = 0;
        public long GarageCount { get; set; } = 0;
        public long SparePartCount { get; set; } = 0;
        public long ServiceStaffCount { get; set; } = 0;
        public long DeliveryStaffCount { get; set; } = 0;
        public long TicketCount { get; set; } = 0;
        public long PendingGarageCount { get; set; } = 0;
        public decimal Earning { get; set; } = 0;


        public List<GarageViewModel> Garages { get; set; } = new List<GarageViewModel>();
        public List<SparePartsDealerViewModel> SparePartsDealers { get; set; } = new List<SparePartsDealerViewModel>();
    }
}
