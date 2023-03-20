using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class DuplicateMenuItemViewModel
    {
        public long ItemId { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public long CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public string Image { get; set; }
        public List<DuplicateMenuItemOptionViewModel> MenuItemOptions { get; set; }
    }
}
