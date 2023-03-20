using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.RestaurantDeliveryStaff
{
	public class RestaurantDeliveryStaffCashDTO
	{
		public long Id { get; set; }
		public long? DeliveryStaffId { get; set; }
		public decimal DeliveryStaffCash { get; set; }
		public string OrderNo { get; set; }
		public string DeliveryStaffName { get; set; }
	}
}
