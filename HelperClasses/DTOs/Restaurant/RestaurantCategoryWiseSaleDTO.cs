using HelperClasses.DTOs.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
	public class RestaurantCategoryWiseSaleDTO
	{
		public long Id { get; set; }
		public decimal Amount { get; set; }
		public int Quantity { get; set; }

		public long? RestaurantBalanceSheetId { get; set; }
		public long? OrderDetailId { get; set; }
		public long? CategoryId { get; set; }
		public string CategoryName { get; set; }
		public RestaurantBalanceSheetDTO RestaurantBalanceSheet { get; set; }
		public OrderDetailDTO OrderDetail { get; set; }
		public CategoryDTO Category { get; set; }
	}

	public class RestaurantCategoryWiseSaleReportDTO
	{
		public long Id { get; set; }
		public decimal Amount { get; set; }
		public int Quantity { get; set; }
		public string CategoryName { get; set; }
	}
}
