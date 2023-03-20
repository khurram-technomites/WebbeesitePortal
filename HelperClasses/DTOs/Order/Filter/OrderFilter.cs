using HelperClasses.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Order.Filter
{
	public class OrderFilter
	{
		public PagingParameters Paging { get; set; }
		public Nullable<OrderStatus> OrderStatus { get; set; }
		public bool OnlyNewOrders { get; set; }
		public long BranchID { get; set; }
		public long RestaurantID { get; set; }
		public int SortBy { get; set; }

	}
}
