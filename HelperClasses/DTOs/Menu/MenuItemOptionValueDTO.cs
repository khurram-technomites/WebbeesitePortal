using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Menu
{
	public class MenuItemOptionValueDTO
	{
		public long Id { get; set; }
		public string Value { get; set; }
		public string ValueAr { get; set; }
		public string Image { get; set; }
		public decimal Price { get; set; }
		public bool IsPriceMain { get; set; }
		public long MenuItemOptionId { get; set; }
		public MenuItemOptionDTO MenuItemOption { get; set; }

	}
}
