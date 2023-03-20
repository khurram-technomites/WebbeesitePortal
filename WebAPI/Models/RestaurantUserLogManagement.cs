using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public class RestaurantUserLogManagement : GeneralSchema
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		public DateTime? LoginTime { get; set; }
		public DateTime? LogoutTime { get; set; }
		[MaxLength(255, ErrorMessage = "DeviceID length must be less than 255 characters")]
		public string DeviceID { get; set; }
		[MaxLength(20, ErrorMessage = "Status length must be less than 20 characters")]
		public string Status { get; set; }
        [MaxLength(255, ErrorMessage = "User Type must be less than 255 characters")]
		public long? UserDetailId { get; set; }
		public string UserType { get; set; }

		/*Foreign Keys*/
		[ForeignKey(nameof(User))]
		public string UserId { get; set; }
		[ForeignKey("Restaurant")]
		public long RestaurantId { get; set; }
		[ForeignKey(nameof(RestaurantBranch))]
		public long RestaurantBranchId { get; set; }
		//[ForeignKey(nameof(ServiceStaff))]
		//public long? ServiceStaffId { get; set; }

		/*Relationships*/
		public Restaurant Restaurant { get; set; }
		public RestaurantBranch RestaurantBranch { get; set; }
		public AppUser User { get; set; }

		//public ServiceStaff ServiceStaff { get; set; }
	}
}
