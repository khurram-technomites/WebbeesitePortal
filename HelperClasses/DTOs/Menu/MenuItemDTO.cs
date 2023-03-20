using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Menu
{
	public class MenuItemDTO
	{
        public long Id { get; set; }
        public long ItemId { get; set; }
        public long MenuId { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public long CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public string Image { get; set; }
        public int Position { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal DiscountPercentage { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 1;
        public int CategoryPosition { get; set; }
        public bool RequiredCheck { get; set; }
        public MenuDTO Menu { get; set; }
        public CategoryDTO Category { get; set; }
        public List<MenuItemOptionDTO> MenuItemOptions { get; set; }
        public ItemDTO Item { get; set; }
    }
}
