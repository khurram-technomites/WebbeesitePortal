using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public class RestaurantManager : GeneralSchema
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
		[MaxLength(255, ErrorMessage = "Name length must be 255 less than characters")]
		public string Name { get; set; }
		[MaxLength(20, ErrorMessage = "LastName length must be 20 less than characters")]
		public string Email { get; set; }
		[MaxLength(20, ErrorMessage = "Contact length must be 20 less than characters")]
		public string Contact { get; set; }
		[MaxLength(20, ErrorMessage = "Contact2 length must be 20 less than characters")]
		public string Contact2 { get; set; }
		public string Address { get; set; }
		[MaxLength(100, ErrorMessage = "Status length must be less than 100 characters")]
		public string Status { get; set; }
		/*Foreign Keys*/
		[ForeignKey("Restaurant")]
		public long RestaurantId { get; set; }
		[ForeignKey(nameof(RestaurantBranch))]
		public long RestaurantBranchId { get; set; }

		/*Relationships*/
		public Restaurant Restaurant { get; set; }
		public RestaurantBranch RestaurantBranch { get; set; }
	}
}
