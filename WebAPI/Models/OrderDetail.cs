using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public class OrderDetail
	{
		public OrderDetail()
		{
			OrderDetailOptionValues = new HashSet<OrderDetailOptionValue>();
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
		[ForeignKey(nameof(Order))]
		public long OrderId { get; set; }
		[ForeignKey(nameof(MenuItems))]
		public long MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal TaxPercent { get; set; }
		public decimal TaxAmount { get; set; }
		public decimal DiscountPercent { get; set; }
		public decimal DiscountAmount { get; set; }
		public decimal TotalPrice { get; set; }
		public string MenuItemOptionValue { get; set; }

		[MaxLength(500, ErrorMessage = "CustomerNote must be less than 500 characters")]
		public string CustomerNote { get; set; }
		[MaxLength(50, ErrorMessage = "Status must be less than 50 characters")]
		public string Status { get; set; }
		public bool? IsUpdated { get; set; }
		public int EditCount { get; set; }

		public MenuItem MenuItems { get; set; }
		public Order Order { get; set; }
		
		/*Foreign Keys*/
		[ForeignKey(nameof(Category))]
		public long? CategoryId { get; set; }

		/*Relationships*/
		public Category Category { get; set; }
		
		public ICollection<OrderDetailOptionValue> OrderDetailOptionValues { get; set; }
	}
}
