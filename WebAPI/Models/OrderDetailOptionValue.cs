using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public class OrderDetailOptionValue
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal TotalPrice { get; set; }
		public long? MenuItemOptionId { get; set; }
		public string MenuItemOption { get; set; }

		public long? MenuItemOptionValueId { get; set; }
		public string MenuItemOptionValue { get; set; }
		[ForeignKey(nameof(OrderDetail))]
		public long OrderDetailId { get; set; }
		public OrderDetail OrderDetail { get; set; }

	}
}
