using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.RestaurantDeliveryStaff
{
    public class RestaurantDeliveryStaffOrderFilterDTO
    {
        public PagingParameters Paging { get; set; }
        public bool RequireNewOrders { get; set; }
        public long DeliveryStaffId { get; set; }
    }
}
