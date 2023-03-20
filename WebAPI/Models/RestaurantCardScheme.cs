using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public class RestaurantCardScheme
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		/*Foreign Keys*/
		[ForeignKey("Restaurant")]
		public long RestaurantId { get; set; }
		[ForeignKey(nameof(RestaurantBranch))]
		public long RestaurantBranchId { get; set; }
		[ForeignKey(nameof(CardScheme))]
		public long? CardSchemeId { get; set; }

		/*Relationships*/
		public Restaurant Restaurant { get; set; }
		public RestaurantBranch RestaurantBranch { get; set; }
		public CardScheme CardScheme { get; set; }
	}
}
