using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class RestaurantDashboardViewModel
    {
        public long RestaurantId { get; set; }
        public long UserCount { get; set; } = 0;
        public long CustomerCount { get; set; } = 0;
        public long RestaurantBranchCount { get; set; } = 0;
        public long ItemsCount { get; set; } = 0;
        public long MenusCount { get; set; } = 0;
        public long CouponsCount { get; set; } = 0;
        public long OrdersCount { get; set; } = 0;
        public long DeliveryStaffCount { get; set; } = 0;
        public long CashierStaffCount { get; set; } = 0;
        public long CategoriesCount { get; set; } = 0;
        public long CanceledCount { get; set; } = 0;
        public long FoodReadyCount { get; set; } = 0;
        public long PreparingCount { get; set; } = 0;
        public long ConfirmedCount { get; set; } = 0;
        public long OnTheWayCount { get; set; } = 0;
        public long DeliveredCount { get; set; } = 0;
        public long PendingCount { get; set; } = 0;

        public long ServiceStaffCount { get; set; } = 0;


        public List<RestaurantOrderViewModel> Orders { get; set; } = new List<RestaurantOrderViewModel>();
     }
}
