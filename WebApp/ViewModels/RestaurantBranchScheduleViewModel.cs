using System;

namespace WebApp.ViewModels
{
    public class RestaurantBranchScheduleViewModel
    {
        public long Id { get; set; }
        public long RestaurantBranchId { get; set; }
        public string Day { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public string FormattedOpeningTime { get; set; }
        public string FormattedClosingTime { get; set; }
        public bool IsInsert { get; set; }
        public bool IsUpdated { get; set; }
    }
}
