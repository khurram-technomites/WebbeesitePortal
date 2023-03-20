
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelperClasses.DTOs.RestaurantCashierStaff;
using HelperClasses.DTOs.Order;

namespace HelperClasses.DTOs.Restaurant
{
	public class RestaurantBalanceSheetDTO
	{
		public RestaurantBalanceSheetDTO()
		{
			RestaurantAggregatorWiseSales = new List<RestaurantAggregatorWiseSaleDTO>();
			RestaurantProductWiseSales = new List<RestaurantProductWiseSaleDTO>();
			RestaurantCategoryWiseSales = new List<RestaurantCategoryWiseSaleDTO>();
			Orders = new List<OrderDTO>();
		}
		public long Id { get; set; }
		public string DeviceId { get; set; }
		public decimal OpeningBalance { get; set; }
		public decimal ClosingBalance { get; set; }
		public decimal TotalCashSale { get; set; }
		public decimal TotalCardSale { get; set; }
		public decimal AggregatorCashSale { get; set; }
		public decimal AggregatorOnlineSale { get; set; }
		public decimal TotalCreditSale { get; set; }
		public decimal NetTotal { get; set; }   //TotalSaleWithoutTax //newly added
		public decimal GrandTotal { get; set; }
		public long TotalItemQuantity { get; set; } //newly added
		public long TotalCategoryCount { get; set; } //newly added
		public decimal TotalTax { get; set; }
		public decimal DeliveryCharges { get; set; }    //newly added
		public decimal TotalDiscount { get; set; }  //newly added

		public decimal TotalSaleWithoutTax { get; set; } //newly added
		public decimal GrossSaleWithTax { get; set; } //newly added

		public bool IsUpdation { get; set; }
		public string Status { get; set; }
		public DateTime? OpeningDate { get; set; }
		public DateTime? ClosingDate { get; set; }
		public long RestaurantId { get; set; }
		public long RestaurantBranchId { get; set; }
		public long RestaurantCashierStaffId { get; set; }
		public RestaurantDTO Restaurant { get; set; }
		public RestaurantBranchDTO RestaurantBranch { get; set; }
		public RestaurantCashierStaffDTO restaurantCashierStaff { get; set; }
		public RestaurantCashDenominationDTO RestaurantCashDenomination { get; set; }
		public List<RestaurantAggregatorWiseSaleDTO> RestaurantAggregatorWiseSales { get; set; }
		public List<RestaurantProductWiseSaleDTO> RestaurantProductWiseSales { get; set; }
		public List<RestaurantCategoryWiseSaleDTO> RestaurantCategoryWiseSales { get; set; }
		public List<OrderDTO> Orders { get; set; }
	}

	public class RestaurantBalanceSheetReportDTO
	{
		public RestaurantBalanceSheetReportDTO()
		{
			RestaurantAggregatorWiseSales = new List<RestaurantAggregatorWiseSaleDTO>();
			RestaurantProductWiseSales = new List<RestaurantProductWiseSaleDTO>();
			RestaurantCategoryWiseSales = new List<RestaurantCategoryWiseSaleDTO>();
		}
		public long Id { get; set; }
		public string DeviceId { get; set; }
		public decimal OpeningBalance { get; set; }
		public decimal ClosingBalance { get; set; }
		public decimal TotalCashSale { get; set; }
		public decimal TotalCardSale { get; set; }
		public decimal AggregatorCashSale { get; set; }
		public decimal AggregatorOnlineSale { get; set; }
		public decimal TotalCreditSale { get; set; }
		public decimal NetTotal { get; set; }   //TotalSaleWithoutTax //newly added
		public decimal GrandTotal { get; set; }
		public long TotalOrderCount { get; set; } //newly added
		public long CompletedOrderCount { get; set; } //newly added
		public long CanceledOrderCount { get; set; } //newly added
		public long TotalItemQuantity { get; set; } //newly added
		public long TotalCategoryCount { get; set; } //newly added
		public decimal TotalTax { get; set; }
		public decimal DeliveryCharges { get; set; }    //newly added
		public decimal TotalDiscount { get; set; }  //newly added
		public decimal TotalSaleWithoutTax { get; set; } //newly added
		public decimal GrossSaleWithTax { get; set; } //newly added
		public string Status { get; set; }
		public DateTime? OpeningDate { get; set; }
		public DateTime? ClosingDate { get; set; }
		public long RestaurantId { get; set; }
		public long RestaurantBranchId { get; set; }
		public long RestaurantCashierStaffId { get; set; }
		public RestaurantCashDenominationDTO RestaurantCashDenomination { get; set; }
		public RestaurantPrinterSettingDTO RestaurantPrinterSetting { get; set; }
		public List<RestaurantAggregatorWiseSaleDTO> RestaurantAggregatorWiseSales { get; set; }
		public List<RestaurantProductWiseSaleDTO> RestaurantProductWiseSales { get; set; }
		public List<RestaurantCategoryWiseSaleDTO> RestaurantCategoryWiseSales { get; set; }
	}

	public class RestaurantBalanceSheetLogsDTO
	{
		public long Id { get; set; }
		public string DeviceId { get; set; }
		public decimal OpeningBalance { get; set; }
		public decimal ClosingBalance { get; set; }
		public decimal TotalCashSale { get; set; }
		public decimal TotalCardSale { get; set; }
		public decimal TotalAggregatorOnlineSale { get; set; }
		public decimal GrandTotal { get; set; }
		public decimal TotalTax { get; set; }
		public decimal AggregatorCashSale { get; set; }
		public decimal TotalCreditSale { get; set; }
		public DateTime? OpeningDate { get; set; }
		public DateTime? ClosingDate { get; set; }
		public string Status { get; set; }

		public long RestaurantId { get; set; }

		public long RestaurantBranchId { get; set; }
		public string BranchName { get; set; }

		public long RestaurantCashierStaffId { get; set; }
		public string CashierStaffName { get; set; }
		public string CashierStaffLogo { get; set; }
	}
}
