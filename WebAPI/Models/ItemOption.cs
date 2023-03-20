using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class ItemOption : GeneralSchema
    {
        public ItemOption()
        {
            ItemOptionValues = new HashSet<ItemOptionValue>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(500, ErrorMessage = "Title length must be less than 500 characters")]
        public string Title { get; set; }
        [MaxLength(500, ErrorMessage = "TitleAr length must be less than 500 characters")]
        public string TitleAr { get; set; }
        [ForeignKey(nameof(Item))]
        public long ItemId { get; set; }

        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }
        public bool IsRequired { get; set; }
        public bool IsPriceMain { get; set; }
        public bool IsRadioButton { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; }


        public Item Item { get; set; }
        public Restaurant Restaurant { get; set; }
        public ICollection<ItemOptionValue> ItemOptionValues { get; set; }
    }
}
