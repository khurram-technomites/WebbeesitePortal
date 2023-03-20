using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Item : GeneralSchema
    {
        public Item()
        {
            MenuItems = new HashSet<MenuItem>();
            ItemOptions = new HashSet<ItemOption>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey(nameof(Restaurant))]
        public long? RestaurantId { get; set; }

        [ForeignKey(nameof(Category))]
        public long CategoryId { get; set; }

        [MaxLength(200, ErrorMessage = "Name length must be less than 200 characters")]
        public string Name { get; set; }
        [MaxLength(200, ErrorMessage = "NameAr length must be less than 200 characters")]
        public string NameAr { get; set; }
        [MaxLength(4000, ErrorMessage = "Description length must be less than 4000 characters")]
        public string Description { get; set; }
        [MaxLength(4000, ErrorMessage = "DescriptionAr length must be less than 4000 characters")]
        public string DescriptionAr { get; set; }
        public string Image { get; set; }
        [MaxLength(50, ErrorMessage = "Name length must be less than 50 characters")]
        public string Status { get; set; }
        public decimal Price { get; set; }

        public Category Category { get; set; }
        public Restaurant Restaurant { get; set; }
        public ICollection<MenuItem> MenuItems { get; set; }
        public ICollection<ItemOption> ItemOptions { get; set; }
    }
}
