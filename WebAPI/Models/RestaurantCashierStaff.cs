using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public class RestaurantCashierStaff : GeneralSchema
	{
		public RestaurantCashierStaff()
		{
			RestaurantBalanceSheets = new HashSet<RestaurantBalanceSheet>();
			Orders = new HashSet<Order>();
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		/*Foreign Keys*/
		[ForeignKey(nameof(User))]
		public string UserId { get; set; }
		[ForeignKey("Restaurant")]
		public long RestaurantId { get; set; }
		[ForeignKey(nameof(RestaurantBranch))]
		public long RestaurantBranchId { get; set; }

		[MaxLength(20, ErrorMessage = "FirstName length must be 20 less than characters")]
		public string FirstName { get; set; }
		[MaxLength(20, ErrorMessage = "LastName length must be 20 less than characters")]
		public string LastName { get; set; }
		[MaxLength(50, ErrorMessage = "Email length must be 50 less than characters")]
		public string Email { get; set; }
		[MaxLength(20, ErrorMessage = "Phone Number length must be 20 less than characters")]
		public string PhoneNumber { get; set; }
		[MaxLength(100, ErrorMessage = "Status length must be less than 100 characters")]
		public string Status { get; set; }
		public string Password { get; set; }
		public string Logo { get; set; }
		
		public bool IsPrinterAllowed { get; set; } // toggle added in mobile app

		/*Relationships*/
		public AppUser User { get; set; }
		public Restaurant Restaurant { get; set; }
		public RestaurantBranch RestaurantBranch { get; set; }

		//ICollections
		public ICollection<RestaurantBalanceSheet> RestaurantBalanceSheets { get; set; }
		public ICollection<Order> Orders { get; set; }
	}
}
