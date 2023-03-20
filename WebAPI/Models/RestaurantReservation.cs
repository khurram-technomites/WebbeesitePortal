using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public class RestaurantReservation : GeneralSchema
	{
		//public RestaurantReservation()
		//{
		//}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
		public DateTime ReservationDate { get; set; }
		public TimeSpan ReservationTime { get; set; }
		[Required(ErrorMessage = "No of Seats cannot be null")]
		public int SeatsCount { get; set; }
		[MaxLength(2000, ErrorMessage = "Note length must be less than 2000 characters")]
		public string Note { get; set; }
		[Required(ErrorMessage = "Name cannot be null")]
		[MaxLength(255, ErrorMessage = "Name length must be less than 50 characters")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Contact cannot be null")]
		[MaxLength(20, ErrorMessage = "Contact length must be less than 20 characters")]
		public string Contact { get; set; }
		[Required(ErrorMessage = "Status cannot be null")]
		[MaxLength(50, ErrorMessage = "Status length must be less than 50 characters")]
		public string Status { get; set; }

		/*Foreign Keys*/
		[ForeignKey(nameof(RestaurantCashierStaff))]
		public long RestaurantCashierStaffId { get; set; }
		
		[ForeignKey(nameof(RestaurantBranch))]
		public long RestaurantBranchId { get; set; }

		[ForeignKey(nameof(Restaurant))]
		public long RestaurantId { get; set; }
		
		/*Relationships*/
		public RestaurantCashierStaff RestaurantCashierStaff { get; set; }
		public RestaurantBranch RestaurantBranch { get; set; }
		public Restaurant Restaurant { get; set; }
		//ICollections

	}
}
