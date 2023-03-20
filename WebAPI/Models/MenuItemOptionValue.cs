using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class MenuItemOptionValue : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(500, ErrorMessage = "Value must be less than 500 characters")]
        public string Value { get; set; }

        [MaxLength(500, ErrorMessage = "ValueAr must be less than 500 characters")]
        public string ValueAr { get; set; }

        [ForeignKey(nameof(MenuItemOption))]
        public long MenuItemOptionId { get; set; }        
        public string Image { get; set; }
        public decimal Price { get; set; }
        public bool IsPriceMain { get; set; }
        public MenuItemOption MenuItemOption { get; set; }
    }
}