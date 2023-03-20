using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Order
{
    public class OrderDetailOptionsDTO
    {
        public string CustomerNote { get; set; }
        public string Option { get; set; }
        public List<OrderDetailsOptionValuesDTO> OptionValues { get; set; }
    }

    public class OrderDetailsOptionValuesDTO
    {
        public long Id { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string MenuItemOptionValue { get; set; }
    }
}
