using HelperClasses.DTOs.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class CategoryDTO
    {
        public CategoryDTO()
		{
            MenuItems = new List<MenuItemDTO>();
            Items = new List<ItemDTO>();
            CouponCategories = new List<CouponCategoryDTO>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string NameAR { get; set; }
        public string Description { get; set; }
        public string DescriptionAR { get; set; }
        public string Image { get; set; }
        public string Slug { get; set; }
        public long? ParentCategoryId { get; set; }
        public int Position { get; set; }
        public bool IsParentCategoryDeactivate { get; set; }
        public bool IsDefault { get; set; }
        public string Status { get; set; }
        public long? RestaurantId { get; set; }
        public DateTime CreationDate { get; set; }

        public List<MenuItemDTO> MenuItems { get; set; }
        public List<ItemDTO> Items { get; set; }
        public List<CouponCategoryDTO> CouponCategories { get; set; }

    }
}
