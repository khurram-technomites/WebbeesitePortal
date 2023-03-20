using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Order
{
	public class OrderPlacementDTO
	{
		[Required]
		public long RestaurantBranchId { get; set; }
		public long? OrderId { get; set; }
		public decimal DiscountPercentage { get; set; }
		public decimal DiscountAmount { get; set; }
		public string CouponCode { get; set; }
		public decimal Subtotal { get; set; }
		public decimal DeliveryCharges { get; set; }
		public decimal TaxAmount { get; set; }
		public decimal TaxPercentage { get; set; }
		public decimal TotalAmount { get; set; }
		public string PaidTo { get; set; }
		public string MergeTableIds { get; set; }
		public long? CardSchemeId { get; set; }
		public long? AggregatorId { get; set; }
		public decimal CardAmount { get; set; }
		public decimal CardReceived { get; set; }
		public decimal Change { get; set; }
		public string Origin { get; set; }
		public string OrderNote { get; set; }

		[Required]
		public string PaymentMethod { get; set; }
		public string Address { get; set; }
		public long? CustomerId { get; set; }
		public long? TableId { get; set; }

		public long? RestaurantCashierStaffId { get; set; }
		public long? RestaurantWaiterId { get; set; }
		public long? DeliveryStaffId { get; set; }
		public decimal DeliveryStaffCash { get; set; }

		//  [Required]
		public string CustomerName { get; set; }
		// [Required]
		public string CustomerContact { get; set; }
		// [Required]
		public string CustomerEmail { get; set; }
		public string Floor { get; set; }
		public string DeliveryAddress { get; set; }
		public decimal? Latitude { get; set; }
		public decimal? Longitude { get; set; }
		public string NoteToRider { get; set; }
		public string Street { get; set; }
		public string Type { get; set; }
		[Required]
		public string DeliveryType { get; set; }
		[EnsureMinimumElements(1, ErrorMessage = "Cart cannot be empty. Atleast one item is required")]
		public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
	}

	public class OrderItem
	{
		public long Id { get; set; } //OderDetailId
		[Required]
		public long MenuItemId { get; set; }
		[Required]
		public int Quantity { get; set; }
		public decimal DiscountAmount { get; set; }
		public decimal DiscountPercent { get; set; }
		public decimal TotalPrice { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal Price { get; set; }
		public string CustomerNote { get; set; }
		public string Status { get; set; }
		public bool? IsUpdated { get; set; } = false;
		public int EditCount { get; set; } = 0;
		public List<OrderItemOption> OrderItemOptions { get; set; } = new List<OrderItemOption>();
	}

	public class OrderItemOption
	{
		[Required]
		public long MenuItemOptionId { get; set; }
		[Required]
		public long MenuItemOptionValueId { get; set; }
	}

	public class EnsureMinimumElementsAttribute : ValidationAttribute
	{
		private readonly int _minElements;
		public EnsureMinimumElementsAttribute(int minElements)
		{
			_minElements = minElements;
		}

		public override bool IsValid(object value)
		{
			if (value is IList list)
			{
				return list.Count >= _minElements;
			}
			return false;
		}
	}
}
