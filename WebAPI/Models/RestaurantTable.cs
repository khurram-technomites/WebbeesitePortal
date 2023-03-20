using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
	public class RestaurantTable : GeneralSchema
	{
		public RestaurantTable()
		{
			RestaurantTableReservations = new HashSet<RestaurantTableReservation>();
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
		[MaxLength(50, ErrorMessage = "Name length must be less than 50 characters")]
		public string Name { get; set; }
		[MaxLength(50, ErrorMessage = "Type length must be less than 50 characters")]
		public string Type { get; set; }
		[MaxLength(100, ErrorMessage = "Status length must be less than 100 characters")]
		public string Status { get; set; }
		[MaxLength(100, ErrorMessage = "ActiveStatus length must be less than 100 characters")]
		public string ActiveStatus { get; set; }
		public int Serving { get; set; }
        public DateTime CreationDate { get; set; }
       
		/*Foreign Keys*/
        [ForeignKey("Restaurant")]
		public long RestaurantId { get; set; }
		[ForeignKey(nameof(RestaurantBranch))]
		public long RestaurantBranchId { get; set; }

		/*Relationships*/
		public Restaurant Restaurant { get; set; }
		public RestaurantBranch RestaurantBranch { get; set; }

		/*ICollections*/
		public ICollection<RestaurantTableReservation> RestaurantTableReservations { get; set; }

	}
}
