using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public class RestaurantCashDenomination
	{
		public RestaurantCashDenomination()
		{
			RestaurantCashDenominationDetails = new HashSet<RestaurantCashDenominationDetail>();
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		public decimal TotalAmount { get; set; }
		public decimal PositiveVariance { get; set; }
		public decimal NegativeVariance { get; set; }

		/*Foreign Keys*/
		[ForeignKey(nameof(RestaurantBalanceSheet))]
		public long RestaurantBalanceSheetId { get; set; }

		/*Relationships*/
		public RestaurantBalanceSheet RestaurantBalanceSheet { get; set; }

		//ICollections
		public virtual ICollection<RestaurantCashDenominationDetail> RestaurantCashDenominationDetails { get; set; }
	}
}
