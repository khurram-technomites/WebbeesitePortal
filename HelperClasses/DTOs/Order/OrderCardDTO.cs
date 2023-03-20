using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Order
{
	public class OrderCardDTO
	{
		public string Status { get; set; }
		public List<OrderCardDetailsDTO> OrderCardDetails { get; set; }
	}

	public class OrderCardDetailsDTO
	{
		public long Id { get; set; }
		public string RestaurantLogo { get; set; }
		public string RestaurantBranchName { get; set; }
		public string Date { get; set; }
		public string Address { get; set; }
		public string Status { get; set; }
		public string OrderType { get; set; }
		public string OrderNo { get; set; }
		public decimal OrderAmount { get; set; }
		public int EstimatedDeliveryMinutes { get; set; }
		public bool IsPaid { get; set; }
	}
}
