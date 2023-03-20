using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantPendingPaymentsDTO
    {
		public long Id { get; set; }
		public string OrderNo { get; set; }
		public decimal TotalAmount { get; set; }
		public string PaidStatus { get; set; }
	}
}
