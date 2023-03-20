using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelperClasses.DTOs.RestaurantCashierStaff;

namespace HelperClasses.DTOs.Restaurant
{
	public class RestaurantBalanceSheetVarianceDTO
	{
		public bool IsPositiveVariance { get; set; } = false;
		public decimal TotalCashSale { get; set; } = 0;
		public decimal TotalAmount { get; set; } = 0;
		public decimal NegativeVariance { get; set; } = 0;
		public decimal PositiveVariance { get; set; } = 0;
	}
}
