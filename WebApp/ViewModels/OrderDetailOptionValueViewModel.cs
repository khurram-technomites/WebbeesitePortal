using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class OrderDetailOptionValueViewModel
    {
		public long? Id { get; set; }
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal TotalPrice { get; set; }
		public long? MenuItemOptionId { get; set; }
		public string MenuItemOption { get; set; }
		public long? MenuItemOptionValueId { get; set; }
		public string MenuItemOptionValue { get; set; }
	}
}
