using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class MenuItemOption : GeneralSchema
    {
        public MenuItemOption()
        {
            MenuItemOptionValues = new HashSet<MenuItemOptionValue>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(500 , ErrorMessage = "Title must be less than 500 characters")]
        public string Title { get; set; }

        [MaxLength(500, ErrorMessage = "TitleAr must be less than 500 characters")]
        public string TitleAr { get; set; }
        public bool IsRequired { get; set; }
        public bool IsPriceMain { get; set; }
        public bool IsRadioButton { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; }

        [ForeignKey(nameof(MenuItem))]
        public long MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }
        public ICollection<MenuItemOptionValue> MenuItemOptionValues { get; set; }
    }
}
