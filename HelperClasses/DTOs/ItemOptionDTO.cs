using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class ItemOptionDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public long ItemId { get; set; }
        public long RestaurantId { get; set; }
        public bool IsRequired { get; set; }
        public bool IsPriceMain { get; set; }
        public bool IsRadioButton { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; }

        public ItemDTO Item { get; set; }
        public List<ItemOptionValueDTO> ItemOptionValues { get; set; }
    }
}
