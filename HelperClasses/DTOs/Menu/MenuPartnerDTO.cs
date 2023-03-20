using HelperClasses.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Menu
{
    public class MenuPartnerDTO
    {
        public long MenuId { get; set; }
        public string MenuName { get; set; }
        public string Status { get; set; }
        public long ItemCount
        {
            get
            {
                long count = 0;
                Categories.ForEach((x) => { count += x.Items.Count; });

                return count;
            }
            set { }
        }
        public List<MenuCategoryPartnerDTO> Categories { get; set; } = new List<MenuCategoryPartnerDTO>();
    }

    public class MenuCategoryPartnerDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Status
        {
            get
            {
                return Items.All(x => x.Status == Enum.GetName(typeof(Status), Classes.Status.Inactive)) ? Enum.GetName(typeof(Status), Classes.Status.Inactive) : Enum.GetName(typeof(Status), Classes.Status.Active);
            }
            set { }
        }
        public bool IsActive { get { return Status == Enum.GetName(typeof(Status), Classes.Status.Active); } set { } }
        public List<MenuCategoryItemDTO> Items { get; set; }
    }

    public class MenuCategoryItemDTO
    {
        public long Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public bool IsActive { get { return Status == Enum.GetName(typeof(Status), Classes.Status.Active); } set { } }
        public List<MenuCategoryItemOptionsDTO> MenuItemOptions { get; set; }
    }

    public class MenuCategoryItemOptionsDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public bool IsRequired { get; set; }
        public bool IsPriceMain { get; set; }
        public List<MenuCategoryItemOptionValuesDTO> MenuItemOptionValues { get; set; }
    }

    public class MenuCategoryItemOptionValuesDTO
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public decimal Price { get; set; }
    }
}
