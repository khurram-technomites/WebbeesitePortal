using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class ItemOptionValue : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(500, ErrorMessage = "Value length must be less than 500 characters")]
        public string Value { get; set; }
        [MaxLength(500, ErrorMessage = "ValueAr length must be less than 500 characters")]
        public string ValueAr { get; set; }
        [ForeignKey(nameof(ItemOption))]
        public long ItemOptionId { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public ItemOption ItemOption { get; set; }
    }
}
