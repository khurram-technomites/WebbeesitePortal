using HelperClasses.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class RestaurantOrderDetailViewModel
    {
		public long Id { get; set; }
		public long OrderId { get; set; }
		public long MenuItemId { get; set; }
		public string MenuItem { get; set; }
		public string MenuItemName { get; set; }
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal TaxPercent { get; set; }
		public decimal TaxAmount { get; set; }
		public decimal DiscountPercent { get; set; }
		public decimal DiscountAmount { get; set; }
		public decimal TotalPrice { get; set; }
		public string CustomerNote { get; set; }
		public string Status { get; set; }
		public bool? IsUpdated { get; set; }
		public int EditCount { get; set; }
		public long? CategoryId { get; set; }

		public CategoryViewModel Category { get; set; }
		public MenuItemViewModel MenuItems { get; set; }
		public List<OrderDetailOptionValueViewModel> OrderDetailOptionValues { get; set; }
	}
}
