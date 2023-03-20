using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantCashDenominationDTO
    {
        public RestaurantCashDenominationDTO()
        {
            RestaurantCashDenominationDetails = new List<RestaurantCashDenominationDetailDTO>();
        }
        public long Id { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PositiveVariance { get; set; }
        public decimal NegativeVariance { get; set; }
        public long RestaurantBalanceSheetId { get; set; }
        //public RestaurantBalanceSheetDTO RestaurantBalanceSheet { get; set; }
        public List<RestaurantCashDenominationDetailDTO> RestaurantCashDenominationDetails { get; set; }
    }
}
