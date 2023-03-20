﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Supplier
{
    public class SupplierDashboardDTO
    {
        public long UserCount { get; set; } = 0;
        public long CustomerCount { get; set; } = 0;
        public long ItemsCount { get; set; } = 0;
        public long CategoriesCount { get; set; } = 0;
        public long CouponsCount { get; set; } = 0;
        public long OrdersCount { get; set; } = 0;
        public long CanceledCount { get; set; } = 0;
        public long FoodReadyCount { get; set; } = 0;
        public long PreparingCount { get; set; } = 0;
        public long ConfirmedCount { get; set; } = 0;
        public long OnTheWayCount { get; set; } = 0;
        public long DeliveredCount { get; set; } = 0;
        public long PendingCount { get; set; } = 0;
    }
}
