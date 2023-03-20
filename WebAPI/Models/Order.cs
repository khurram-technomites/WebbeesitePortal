using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public class Order : GeneralSchema
	{
		public Order()
		{
			OrderDetails = new HashSet<OrderDetail>();
			RestaurantRatings = new HashSet<RestaurantRating>();
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		[MaxLength(50, ErrorMessage = "OrderNo must be less than 50 characters")]
		public string OrderNo { get; set; }

		[ForeignKey(nameof(RestaurantBranch))]
		public long RestaurantBranchId { get; set; }

		[ForeignKey(nameof(Restaurant))]
		public long? RestaurantId { get; set; }

		public decimal Amount { get; set; }
		public decimal TaxPercent { get; set; }
		public decimal TaxAmount { get; set; }
		public decimal DiscountPercent { get; set; }
		public decimal DiscountAmount { get; set; }
		public decimal DeliveryCharges { get; set; }
		public string DeliveryType { get; set; }
		public decimal RedeemAmount { get; set; }
		public decimal TotalAmount { get; set; }

		[MaxLength(20, ErrorMessage = "Payment method must be less than 20 characters")]
		public string PaymentMethod { get; set; }
		public string CouponCode { get; set; }
		public decimal CouponDiscount { get; set; }

		[ForeignKey(nameof(Customer))]
		public long? CustomerId { get; set; }

		public string CustomerName { get; set; }
		public string CustomerContact { get; set; }
		public string CustomerEmail { get; set; }
		[MaxLength(50, ErrorMessage = "Address method must be less than 50 characters")]
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }
		[MaxLength(20, ErrorMessage = "Type length must be less than 20 characters")]
		public string Type { get; set; }
		public string Street { get; set; }
		public string Floor { get; set; }
		public string NoteToRider { get; set; }
		public string OrderNote { get; set; }
		public string Address { get; set; }
		[MaxLength(500, ErrorMessage = "CancelationReason must be less than 500 characters")]
		public string CancelationReason { get; set; }
		[MaxLength(20, ErrorMessage = "Status must be less than 20 characters")]
		public string Status { get; set; }
		public bool IsPaid { get; set; }
		[MaxLength(5, ErrorMessage = "Currency must be less than 5 characters")]
		public string Currency { get; set; }
		public bool PaymentRef { get; set; }
		public bool PaymentCaptured { get; set; }
		public string InvoiceRef { get; set; }
		public bool IsEarningCaptured { get; set; }
		[MaxLength(20, ErrorMessage = "OrderRef must be less than 20 characters")]
		public string OrderRef { get; set; }
		public DateTime? OrderPlacementDateTime { get; set; }
		public DateTime? FoodReadyDateTime { get; set; }
		public DateTime? PickedByRiderDateTime { get; set; }
		public DateTime? DeliveryDateTime { get; set; }
		[ForeignKey(nameof(DeliveryStaff))]
		public long? DeliveryStaffId { get; set; }
		public int EstimatedDeliveryMinutes { get; set; }
		public RestaurantBranch RestaurantBranch { get; set; }
		public Restaurant Restaurant { get; set; }
		public Customer Customer { get; set; }
		public RestaurantDeliveryStaff DeliveryStaff { get; set; }

		/*POS Update*/
		[MaxLength(100, ErrorMessage = "MergeTableIds must be less than 100 characters")]
		public string MergeTableIds { get; set; }
		public bool? IsAmended { get; set; }
		public string AmendReason { get; set; }
		public decimal CashReceived { get; set; }
		public decimal Change { get; set; }
		public decimal CardAmount { get; set; }
		[MaxLength(100, ErrorMessage = "PaidTo must be less than 100 characters")]
		public string PaidTo { get; set; }
		[MaxLength(100, ErrorMessage = "Origin must be less than 50 characters")]
		public string Origin { get; set; }
		public decimal DeliveryStaffCash { get; set; }
		public int EditCount { get; set; }
		public bool IsOnline { get; set; }

		/*Foreign Keys*/
		[ForeignKey(nameof(RestaurantTable))]
		public long? RestaurantTableId { get; set; }
		[ForeignKey(nameof(Aggregator))]
		public long? AggregatorId { get; set; }

		[ForeignKey(nameof(CardScheme))]
		public long? CardSchemeId { get; set; }

		[ForeignKey(nameof(RestaurantCashierStaff))]
		public long? RestaurantCashierStaffId { get; set; }

		[ForeignKey(nameof(RestaurantWaiter))]
		public long? RestaurantWaiterId { get; set; }

		/*Relationships*/
		public RestaurantTable RestaurantTable { get; set; }
		public Aggregator Aggregator { get; set; }
		public CardScheme CardScheme { get; set; }
		public RestaurantCashierStaff RestaurantCashierStaff { get; set; }
		public RestaurantWaiter RestaurantWaiter { get; set; }

		public ICollection<OrderDetail> OrderDetails { get; set; }
		public ICollection<RestaurantRating> RestaurantRatings { get; set; }

	}
}
