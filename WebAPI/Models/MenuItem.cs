using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class MenuItem : GeneralSchema
    {
        public MenuItem()
        {
            MenuItemOptions = new HashSet<MenuItemOption>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Item))]
        public long ItemId { get; set; }
        [ForeignKey(nameof(Menu))]
        public long MenuId { get; set; }
        [MaxLength(200, ErrorMessage = "Name length must be less than 200 characters")]
        public string Name { get; set; }
        [MaxLength(200, ErrorMessage = "NameAr length must be less than 200 characters")]
        public string NameAr { get; set; }
        [MaxLength(4000, ErrorMessage = "Description length must be less than 4000 characters")]
        public string Description { get; set; }
        [MaxLength(4000, ErrorMessage = "DescriptionAr length must be less than 4000 characters")]
        public string DescriptionAr { get; set; }
        [ForeignKey(nameof(Category))]
        public long CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public string Image { get; set; }
        public int Position { get; set; }
        public int CategoryPosition { get; set; }
        public Menu Menu { get; set; }    
        public Category Category { get; set; }
        public ICollection<MenuItemOption> MenuItemOptions { get; set; }
        public Item Item { get; set; }

    }
}
