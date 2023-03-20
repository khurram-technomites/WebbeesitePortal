using HelperClasses.DTOs.Menu;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Order
{
	public class OrderDetailDTO
	{
		public OrderDetailDTO()
		{
			OrderDetailOptionValues = new List<OrderDetailOptionValueDTO>();
		}

		public long Id { get; set; }
		public long OrderId { get; set; }
		public long MenuItemId { get; set; }
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

		public CategoryDTO Category { get; set; }
		public MenuItemDTO MenuItems { get; set; }
		public List<OrderDetailOptionValueDTO> OrderDetailOptionValues { get; set; }

	}
}
