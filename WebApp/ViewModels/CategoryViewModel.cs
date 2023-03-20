using System;

namespace WebApp.ViewModels
{
    public class CategoryViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string NameAR { get; set; }
        public string Description { get; set; }
        public string DescriptionAR { get; set; }
        public string Slug { get; set; }
        public string Image { get; set; }
        public Nullable<long> ParentCategoryId { get; set; }
        public int Position { get; set; }
        public bool IsParentCategoryDeactivate { get; set; }
        public bool IsDefault { get; set; }
        public string Status { get; set; }
        public long? RestaurantId { get; set; }
        public DateTime CreationDate { get; set; }
        public CategoryViewModel Category { get; set; }
    }
}
