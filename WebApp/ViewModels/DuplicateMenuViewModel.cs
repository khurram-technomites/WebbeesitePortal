﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class DuplicateMenuViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public double MenuItemCount { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? BreakFastStartTime { get; set; }
        public DateTime? BreakFastEndTime { get; set; }

        public DateTime? LunchStartTime { get; set; }
        public DateTime? LunchEndTime { get; set; }

        public DateTime? DinnerStartTime { get; set; }
        public DateTime? DinnerEndTime { get; set; }
        public string Status { get; set; }
        public List<DuplicateMenuItemViewModel> MenuItem { get; set; }
        public bool IsPeriodic { get; set; }
        public long? RestaurantBranchId { get; set; }
        public long RestaurantId { get; set; }
    }
}
