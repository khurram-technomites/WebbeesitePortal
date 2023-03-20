using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Order
{
    public class DashboardStatsDTO
    {
        public decimal Cash { get; set; }
        public decimal Card { get; set; }
        public decimal Aggregator { get; set; }
        public decimal Partial { get; set; }
        public decimal Credit { get; set; }
    }
}
