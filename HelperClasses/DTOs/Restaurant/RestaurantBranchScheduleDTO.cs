using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantBranchScheduleDTO
    {
        public long Id { get; set; }
        public long RestaurantBranchId { get; set; }
        public string Day { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public string FormattedOpeningTime { get; set; }
        public string FormattedClosingTime { get; set; }
    }
}
