using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels.Restaurant;

namespace WebApp.ViewModels
{
	public class RestaurantOrderViewModel
	{
		public RestaurantOrderViewModel()
		{
			OrderDetails = new List<RestaurantOrderDetailViewModel>();
		}
		public long Id { get; set; }
		public string OrderNo { get; set; }
		public long RestaurantBranchId { get; set; }
		public long RestaurantId { get; set; }
		public long? RestaurantTableId { get; set; }
		public decimal Amount { get; set; }
		public decimal TaxPercent { get; set; }
		public decimal TaxAmount { get; set; }
		public decimal DiscountPercent { get; set; }
		public decimal DiscountAmount { get; set; }
		public decimal DeliveryCharges { get; set; }
		public string InvoiceRef { get; set; }
		public string MergeTableIds { get; set; }
		public bool IsAmended { get; set; }
		public string AmendReason { get; set; }
		public string PaidTo { get; set; }
		public string Origin { get; set; }
        public decimal DeliveryStaffCash { get; set; }
		public long? AggregatorId { get; set; }
		public long? CardSchemeId { get; set; }
		public long? RestaurantCashierStaffId { get; set; }
		public long? RestaurantWaiterId { get; set; }
		public decimal CashReceived { get; set; }
		public decimal Change { get; set; }
		public decimal CardAmount { get; set; }
		public decimal RedeemAmount { get; set; }
		public decimal TotalAmount { get; set; }
		public string PaymentMethod { get; set; }
		public string CouponCode { get; set; }
		public decimal CouponDiscount { get; set; }
		public string DeliveryType { get; set; }
		public long? CustomerId { get; set; }
		public string CustomerName { get; set; }
		public string CustomerContact { get; set; }
		public string CustomerEmail { get; set; }
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }
		public string Type { get; set; }
		public string Street { get; set; }
		public string Floor { get; set; }
		public string NoteToRider { get; set; }
		public string OrderNote { get; set; }
		public string Address { get; set; }
		public string CancelationReason { get; set; }
		public string Status { get; set; }
		public bool IsPaid { get; set; }
		public string PaidStatus { get; set; }
		public string Currency { get; set; }
		public bool PaymentRef { get; set; }
		public bool PaymentCaptured { get; set; }
		public bool IsEarningCaptured { get; set; }
		public string OrderRef { get; set; }
		public long? DeliveryStaffId { get; set; }
		public DateTime? CreationDate { get; set; }
		public DateTime? OrderPlacementDateTime { get; set; }
		public DateTime? FoodReadyDateTime { get; set; }
		public DateTime? PickedByRiderDateTime { get; set; }
		public bool IsOnline { get; set; }
		public string FormattedDate
		{
			get
			{
				if (OrderPlacementDateTime.HasValue)
					return OrderPlacementDateTime.Value.ToString("F");

				return "";
			}
			set { }
		}
		public DateTime? DeliveryDateTime { get; set; }
		public int EstimatedDeliveryMinutes { get; set; }
		public int EditCount { get; set; }
		public RestaurantBranchViewModel RestaurantBranch { get; set; }
		public RestaurantViewModel Restaurant { get; set; }
		public CustomerViewModel Customer { get; set; }
		public RestaurantTableViewModel RestaurantTable { get; set; }
		public List<CustomerTransactionHistoryViewModel> Transactions { get; set; }
		public RestaurantDeliveryStaffViewModel DeliveryStaff { get; set; }
		public RestaurantCashierStaffViewModel CashierStaff { get; set; }
		public RestaurantWaiterViewModel RestaurantWaiter { get; set; }
		public AggregatorViewModel Aggregator { get; set; }
		public CardSchemeViewModel CardScheme { get; set; }
		public RestaurantRatingViewModel RestaurantRating { get; set; }
		public List<RestaurantOrderDetailViewModel> OrderDetails { get; set; }
		//public List<RestaurantOrderViewModel> Orders { get; set; }
	}
}
