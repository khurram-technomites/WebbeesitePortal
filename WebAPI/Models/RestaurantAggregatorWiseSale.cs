using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public partial class RestaurantAggregatorWiseSale
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		[MaxLength(20, ErrorMessage = "PaymentStatus length must be less than 20 characters")]
		public string PaymentStatus { get; set; }

		public decimal Amount { get; set; }

		/*Foreign Keys*/
		[ForeignKey(nameof(RestaurantBalanceSheet))]
		public long RestaurantBalanceSheetId { get; set; }
		[ForeignKey(nameof(Order))]
		public long OrderId { get; set; }
		[ForeignKey(nameof(RestaurantAggregator))]
		public long RestaurantAggregatorId { get; set; }

		/*Relationships*/
		public virtual RestaurantBalanceSheet RestaurantBalanceSheet { get; set; }
		public virtual Order Order { get; set; }
		public virtual Aggregator RestaurantAggregator { get; set; }

	}
}
