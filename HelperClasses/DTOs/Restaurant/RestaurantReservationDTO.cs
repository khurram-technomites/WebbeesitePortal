using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelperClasses.DTOs.Order;

namespace HelperClasses.DTOs.Restaurant
{
	public class RestaurantReservationDTO
	{
		public long Id { get; set; }
		public DateTime ReservationDate { get; set; }
		public string FormattedDate
		{
			get
			{
				return ReservationDate.ToString("F");
			}
			set { }
		}
		public TimeSpan ReservationTime { get; set; }
		public DateTime? ReservationTimeDate { get; set; } // for mobile app
		public string FormattedTime
		{
			get
			{
				return $"{(ReservationTime.Hours >= 12 ? ReservationTime.Hours - 12 : ReservationTime.Hours)}:{ReservationTime.Minutes}:{ReservationTime.Seconds} " + (ReservationTime.Hours >= 12 ? "PM" : "AM");
			}
			set { }
		}
		public int SeatsCount { get; set; }
		[MaxLength(2000, ErrorMessage = "Note length must be less than 2000 characters")]
		public string Note { get; set; }
		[MaxLength(255, ErrorMessage = "Name length must be less than 50 characters")]
		public string Name { get; set; }
		[MaxLength(20, ErrorMessage = "Contact length must be less than 20 characters")]
		public string Contact { get; set; }
		[MaxLength(50, ErrorMessage = "Status length must be less than 50 characters")]
		public string Status { get; set; }
		public long RestaurantCashierStaffId { get; set; }
		public long RestaurantBranchId { get; set; }
		public long RestaurantId { get; set; }
	}
}
