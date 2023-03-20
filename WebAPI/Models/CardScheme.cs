using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public class CardScheme : GeneralSchema
	{
		public CardScheme()
		{
			RestaurantCardSchemes = new HashSet<RestaurantCardScheme>();
			Orders = new HashSet<Order>();
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
		[MaxLength(255, ErrorMessage = "Type length must be 255 less than characters")]
		public string Type { get; set; }
		[MaxLength(100, ErrorMessage = "Status length must be less than 100 characters")]
		public string Status { get; set; }
		public ICollection<RestaurantCardScheme> RestaurantCardSchemes { get; set; }
		public ICollection<Order> Orders { get; set; }
	}
}
