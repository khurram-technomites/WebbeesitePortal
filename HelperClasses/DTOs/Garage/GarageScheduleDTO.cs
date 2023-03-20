using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Garage
{
    public class GarageScheduleDTO
    {
        public long Id { get; set; }
        public long GarageId { get; set; }
        public string Day { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public bool IsBreak { get; set; }
        public TimeSpan? BreakTimeStart { get; set; }
        public TimeSpan? BreakTimeEnd { get; set; }
        public string FormattedOpeningTime { get; set; }
        public string FormattedClosingTime { get; set; }
        public string FormattedBreakTimeStart
        {
            get
            {
                return BreakTimeStart.HasValue ? (DateTime.Today + BreakTimeStart.Value).ToShortTimeString() : "-";
            }
            set { }
        }
        public string FormattedBreakTimeEnd
        {
            get
            {
                return BreakTimeStart.HasValue ? (DateTime.Today + BreakTimeEnd.Value).ToShortTimeString() : "-";
            }
            set { }
        }
    }
}
