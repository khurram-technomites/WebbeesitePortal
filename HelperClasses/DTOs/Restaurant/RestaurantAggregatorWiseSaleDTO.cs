using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelperClasses.DTOs.Order;
using HelperClasses.DTOs.Aggregators;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantAggregatorWiseSaleDTO
    {
        public long Id { get; set; }
        public string PaymentStatus { get; set; }
        public decimal Amount { get; set; }
        public long? RestaurantBalanceSheetId { get; set; }
        public long? OrderId { get; set; }
        public long? RestaurantAggregatorId { get; set; }
        public string RestaurantAggregatorName { get; set; }
        public RestaurantBalanceSheetDTO RestaurantBalanceSheet { get; set; }
        public OrderDTO Order { get; set; }
        public AggregatorDTO RestaurantAggregator { get; set; }
    }

    public class RestaurantAggregatorWiseSaleReportDTO
    {
        public long Id { get; set; }
        public string PaymentStatus { get; set; }
        public decimal Amount { get; set; }
        public string RestaurantAggregatorName { get; set; }
    }
}
