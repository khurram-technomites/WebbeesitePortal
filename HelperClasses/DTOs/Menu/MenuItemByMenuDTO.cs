using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Menu
{
    public class MenuItemByMenuDTO
    {
            public long CategoryId { get; set; }
            public string CategoryName { get; set; }
            public string CategoryStatus { get; set; }
            public string CategoryDescription { get; set; }
            public string CategoryImage { get; set; }
            public List<MenuItemByCategoryDTO> MenuItems { get; set; } = new List<MenuItemByCategoryDTO>();
    }

    public class MenuItemByCategoryDTO
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public int Position { get; set; }
    }
}
