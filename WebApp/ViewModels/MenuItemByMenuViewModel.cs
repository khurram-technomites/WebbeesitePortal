using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class MenuItemByMenuViewModel
    {

        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryStatus { get; set; }
        public string CategoryDescription { get; set; }
        public string CategoryImage { get; set; }
        public List<MenuItemByCategoryViewModel> MenuItems { get; set; } = new List<MenuItemByCategoryViewModel>();
    }

    public class MenuItemByCategoryViewModel
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Position { get; set; }
        public string Status { get; set; }
    }

}
