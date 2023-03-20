using HelperClasses.DTOs.Menu;
using HelperClasses.DTOs.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantProductWiseSaleDTO
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public long? RestaurantBalanceSheetId { get; set; }
        public long? OrderDetailId { get; set; }
        public long? MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public RestaurantBalanceSheetDTO RestaurantBalanceSheet { get; set; }
        public OrderDetailDTO OrderDetail { get; set; }
        public MenuItemDTO MenuItem { get; set; }
    }

    public class RestaurantProductWiseSaleReportDTO
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public string MenuItemName { get; set; }
    }
}
