using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class MenuItemViewModel
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
        public int CategoryPosition { get; set; }
        public MenuViewModel Menu { get; set; }
        public CategoryViewModel Category { get; set; }
        public List<MenuItemOptionViewModel> MenuItemOptions { get; set; }
        public ItemViewModel Item { get; set; }
    }
}
