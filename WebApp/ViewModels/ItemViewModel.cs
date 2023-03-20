using Fingers10.ExcelExport.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class ItemViewModel
    {
        public long Id { get; set; }
        public long? RestaurantId { get; set; }
        public long CategoryId { get; set; }
        [IncludeInReport(Order = 1)]
        [Display(Name = "Item Name")]
        [DisplayName("Item Name")]
        public string Name { get; set; }
        [IncludeInReport(Order = 1)]
        [Display(Name = "Item Name")]
        [DisplayName("Item Name")]
        public string NameAr { get; set; }
        [IncludeInReport(Order = 1)]
        [Display(Name = "Item Name Ar")]
        [DisplayName("Item Name Ar")]
        public string Description { get; set; }
        [IncludeInReport(Order = 1)]
        [Display(Name = "Description")]
        [DisplayName("Description")]
        public string DescriptionAr { get; set; }
        public string Image { get; set; }
        [IncludeInReport(Order = 1)]
        [Display(Name = "Status")]
        [DisplayName("Status")]
        public string Status { get; set; }
        [IncludeInReport(Order = 1)]
        [Display(Name = "Price")]
        [DisplayName("Price")]
        public decimal Price { get; set; }
        public DateTime CreationDate { get; set; }
        public CategoryViewModel Category { get; set; }
        public List<ItemOptionViewModel> ItemOptions { get; set; }
    }
}
