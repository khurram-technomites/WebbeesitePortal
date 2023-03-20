using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public partial class RestaurantCategoryWiseSale
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
		public decimal Amount { get; set; }
		public int Quantity { get; set; }

		/*Foreign Keys*/
		[ForeignKey(nameof(RestaurantBalanceSheet))]
		public long RestaurantBalanceSheetId { get; set; }
		[ForeignKey(nameof(OrderDetail))]
		public long OrderDetailId { get; set; }
		[ForeignKey(nameof(Category))]
		public long CategoryId { get; set; }

		/*Relationships*/
		public virtual RestaurantBalanceSheet RestaurantBalanceSheet { get; set; }
		public virtual OrderDetail OrderDetail { get; set; }
		public virtual Category Category { get; set; }

	}
}
