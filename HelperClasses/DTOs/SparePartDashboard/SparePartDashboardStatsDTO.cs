using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.SparePartDashboard
{
    public class SparePartDashboardStatsDTO 
    {
        public long TotalProjects { get; set; } = 0;
        public long TotalAwards { get; set; } = 0;
        public long TotalPartners { get; set; } = 0;
        public long TotalMenus { get; set; } = 0;
        public long TotalTestimonials { get; set; } = 0;
        public double AverageRating { get; set; } = 0;
    }
}
