using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Category : GeneralSchema
    {
        public Category()
        {
            Items = new HashSet<Item>();
            CouponCategories = new HashSet<CouponCategory>();
            MenuItems = new HashSet<MenuItem>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(50, ErrorMessage = "Name length must be less than 50 characters")]
        public string Name { get; set; }
        [MaxLength(50, ErrorMessage = "NameAR length must be less than 50 characters")]
        public string NameAR { get; set; }

        [MaxLength(4000, ErrorMessage = "Description length must be less than 4000 characters")]
        public string Description { get; set; }
        [MaxLength(4000, ErrorMessage = "DescriptionAR length must be less than 4000 characters")]
        public string DescriptionAR { get; set; }

        [MaxLength(50, ErrorMessage = "Slug length must be less than 4000 characters")]
        public string Slug { get; set; }

        [MaxLength(4000, ErrorMessage = "Image length must be less than 50 characters")]
        public string Image { get; set; }
        public long? ParentCategoryId { get; set; }
        public int Position { get; set; }
        public bool IsParentCategoryDeactivate { get; set; }
        public bool IsDefault { get; set; }
        public string Status { get; set; }

        [ForeignKey(nameof(Restaurant))]
        public long? RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public ICollection<MenuItem> MenuItems { get; set; }
        public ICollection<Item> Items { get; set; }
        public ICollection<CouponCategory> CouponCategories { get; set; }
    }
}
