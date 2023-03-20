using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class SparePartsDealerScheduleViewModel
    {
        public long Id { get; set; }
        public long SparePartsDealerId { get; set; }
        public string Day { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public bool IsBreak { get; set; }
        public TimeSpan? BreakTimeStart { get; set; }
        public TimeSpan? BreakTimeEnd { get; set; }
        public string FormattedOpeningTime { get; set; }
        public string FormattedClosingTime { get; set; }
    }
}
