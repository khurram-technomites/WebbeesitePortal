using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class AdminDashboardStatsDTO
    {
        public long UserCount { get; set; } = 0;
        public long CustomerCount { get; set; } = 0;
        public long RestaurantCount { get; set; } = 0;
        public long GarageCount { get; set; } = 0;
        public long SparePartCount { get; set; } = 0;
        public long ServiceStaffCount { get; set; } = 0;
        public long DeliveryStaffCount { get; set; } = 0;
        public long TicketCount { get; set; } = 0;
    }
}
