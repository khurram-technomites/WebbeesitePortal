using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.RestaurantKitchenManager
{
	public class RestaurantKitchenManagerDTO
	{
		public long Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Status { get; set; }
		public string Password { get; set; }
		public string PhoneNumber { get; set; }
		public bool RequirePhoneNumberConfirmation { get; set; }
		public string Logo { get; set; }
		public string UserId { get; set; }
		public DateTime CreationDate { get; set; }

		public long RestaurantId { get; set; }
		public long RestaurantBranchId { get; set; }
		public string RestaurantBranchName { get; set; }
		public bool IsClose { get; set; }

		/*Relation*/

		//public RestaurantDTO Restaurant { get; set; }
		//public RestaurantBranchDTO RestaurantBranch { get; set; }
		public AppUserDTO User { get; set; }
	}
}
