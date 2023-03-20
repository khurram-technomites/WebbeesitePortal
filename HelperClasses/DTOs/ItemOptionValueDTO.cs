using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class ItemOptionValueDTO
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public string ValueAr { get; set; }
        public long ItemOptionId { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public ItemOptionDTO ItemOption { get; set; }
    }
}
