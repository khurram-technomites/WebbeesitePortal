using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Menu
{
	public class MenuItemOptionDTO
	{

		//public MenuItemOptionDTO()
		//{
		//	MenuItemOptionValues = new HashSet<MenuItemOptionValueDTO>();
		//}
		public long Id { get; set; }
		public long MenuItemId { get; set; }
		public string Title { get; set; }
		public string TitleAr { get; set; }
		public bool IsRadioButton { get; set; }
		public bool IsRequired { get; set; }
		public bool IsPriceMain { get; set; }
		public int Minimum { get; set; }
		public int Maximum { get; set; }

		public List<MenuItemOptionValueDTO> MenuItemOptionValues { get; set; }
	}
}
