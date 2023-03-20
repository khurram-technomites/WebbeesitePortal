using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.RestaurantCashierStaff
{
    public class RestaurantCashierStaffOrderFilterDTO
    {
        public PagingParameters Paging { get; set; }
        public bool RequireNewOrders { get; set; }
        public long CashierStaffId { get; set; }
    }
}
