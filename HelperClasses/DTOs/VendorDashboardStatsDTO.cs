using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class VendorDashboardStatsDTO
    {
        
        public long GarageCount { get; set; } = 0;
        public decimal Earning { get; set; } = 0;
        public long PendingGarageCount { get; set; } = 0;
    }
}
