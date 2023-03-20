using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.RestaurantKitchenManager
{
    public class RestaurantKitchenManagerOrderFilterDTO
    {
        public PagingParameters Paging { get; set; }
        public bool RequireNewOrders { get; set; }
        public long CashierStaffId { get; set; }
    }
}
