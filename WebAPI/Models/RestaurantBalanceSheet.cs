using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public class RestaurantBalanceSheet : GeneralSchema
	{
		public RestaurantBalanceSheet()
		{
			RestaurantAggregatorWiseSales = new HashSet<RestaurantAggregatorWiseSale>();
			RestaurantProductWiseSales = new HashSet<RestaurantProductWiseSale>();
			RestaurantCategoryWiseSales = new HashSet<RestaurantCategoryWiseSale>();
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		[MaxLength(255, ErrorMessage = "DeviceId length must be less than 255 characters")]
		public string DeviceId { get; set; }

		public decimal OpeningBalance { get; set; }
		public decimal ClosingBalance { get; set; }
		public decimal TotalCashSale { get; set; }
		public decimal TotalCardSale { get; set; }
		public decimal AggregatorCashSale { get; set; }
		public decimal AggregatorOnlineSale { get; set; }
		public decimal TotalCreditSale { get; set; }
		public decimal NetTotal { get; set; } //TotalSaleWithoutTax	//newly added
		public decimal GrandTotal { get; set; }
		public decimal TotalTax { get; set; }
		public long TotalItemQuantity { get; set; } //newly added
		public long TotalCategoryCount { get; set; } //newly added
		public decimal DeliveryCharges { get; set; } //newly added
		public decimal TotalDiscount { get; set; }  //newly added

		[MaxLength(50, ErrorMessage = "Status length must be less than 50 characters")]
		public string Status { get; set; }

		public DateTime? OpeningDate { get; set; }
		public DateTime? ClosingDate { get; set; }

		/*Foreign Keys*/
		[ForeignKey("Restaurant")]
		public long RestaurantId { get; set; }
		[ForeignKey(nameof(RestaurantBranch))]
		public long RestaurantBranchId { get; set; }
		[ForeignKey(nameof(restaurantCashierStaff))]
		public long RestaurantCashierStaffId { get; set; }

		/*Relationships*/
		public Restaurant Restaurant { get; set; }
		public RestaurantBranch RestaurantBranch { get; set; }
		public RestaurantCashierStaff restaurantCashierStaff { get; set; }
		public RestaurantCashDenomination RestaurantCashDenomination { get; set; }

		//ICollections
		public virtual ICollection<RestaurantAggregatorWiseSale> RestaurantAggregatorWiseSales { get; set; }
		public virtual ICollection<RestaurantProductWiseSale> RestaurantProductWiseSales { get; set; }
		public virtual ICollection<RestaurantCategoryWiseSale> RestaurantCategoryWiseSales { get; set; }

	}
}
