using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class ItemDTO
    {
        public long Id { get; set; }
        public long? RestaurantId { get; set; }
        public long CategoryId { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public DateTime CreationDate { get; set; }
        public CategoryDTO Category { get; set; }
        public List<ItemOptionDTO> ItemOptions { get; set; }
    }
}
