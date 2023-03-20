using System;

namespace WebApp.ViewModels
{
    public class RestaurantBalanceSheetViewModel
    {
		public long Id { get; set; }
		public string DeviceId { get; set; }
		public decimal OpeningBalance { get; set; }
		public decimal ClosingBalance { get; set; }
		public decimal TotalCashSale { get; set; }
		public decimal TotalCardSale { get; set; }
		public decimal AggregatorCashSale { get; set; }
		public decimal AggregatorOnlineSale { get; set; }
		public decimal NetTotal { get; set; }   //TotalSaleWithoutTax //newly added
		public decimal GrandTotal { get; set; }
		public long TotalItemQuantity { get; set; } //newly added
		public long TotalCategoryCount { get; set; } //newly added
		public decimal TotalTax { get; set; }
		public decimal TotalCreditSale { get; set; }
		public decimal DeliveryCharges { get; set; }    //newly added
		public decimal TotalDiscount { get; set; }  //newly added

		public decimal TotalSaleWithoutTax { get; set; } //newly added
		public decimal GrossSaleWithTax { get; set; } //newly added
	
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