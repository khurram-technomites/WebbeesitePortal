using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public class RestaurantTableReservation : GeneralSchema
	{
		public RestaurantTableReservation()
		{
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
		[MaxLength(50, ErrorMessage = "Name length must be less than 50 characters")]
		public string Name { get; set; }
		[MaxLength(20, ErrorMessage = "Contact length must be less than 20 characters")]
		public string Contact { get; set; }
		[MaxLength(100, ErrorMessage = "Status length must be less than 100 characters")]
		public string Status { get; set; }
		public string Note { get; set; }
		public int SeatsReserved { get; set; }
		public int SeatsAvailable { get; set; }
		[MaxLength(20, ErrorMessage = "Merge Table must be less than 20 characters")]
		public string MergeTableIds { get; set; }
		public DateTime? ReservationDate { get; set; }
		public TimeSpan? ReservationTime { get; set; }

		/*Foreign Keys*/
		[ForeignKey(nameof(RestaurantTable))]
		public long RestaurantTableId { get; set; }
		[ForeignKey(nameof(Order))]
		public long? OrderId { get; set; }

		/*Relationships*/
		public RestaurantTable RestaurantTable { get; set; }
		public Order Order { get; set; }

		//ICollections

	}
}
